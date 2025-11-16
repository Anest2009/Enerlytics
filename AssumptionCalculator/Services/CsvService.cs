using AssumptionCalculator.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AssumptionCalculator.Services
{
    /// <summary>
    /// Service for reading and parsing CSV files
    /// </summary>
    public class CsvService
    {
        /// <summary>
        /// Reads a CSV file and returns a dictionary keyed by (Timestamp, GroupId)
        /// Supports both LONG format (timestamp,group_id,value) and WIDE format (timestamp,group1,group2,...)
        /// </summary>
        /// <param name="filePath">Path to the CSV file</param>
        /// <returns>Dictionary with composite key (DateTime, string) and value as double</returns>
        public Dictionary<(DateTime, string), double> ReadCsvFile(string filePath)
        {
            try
            {
                // Peek at the header to determine format
                using (var peekReader = new StreamReader(filePath))
                {
                    var headerLine = peekReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(headerLine))
                    {
                        throw new Exception($"CSV file '{Path.GetFileName(filePath)}' is empty.");
                    }

                    // Check if it's wide format (has numeric columns) or long format (has group_id)
                    var headers = headerLine.Split(',').Select(h => h.Trim().Trim('"')).ToArray();

                    // Long format: has "group_id" column
                    bool isLongFormat = headers.Any(h =>
                        h.Equals("group_id", StringComparison.OrdinalIgnoreCase) ||
                        h.Equals("groupid", StringComparison.OrdinalIgnoreCase) ||
                        h.Equals("group", StringComparison.OrdinalIgnoreCase) ||
                        h.Equals("id", StringComparison.OrdinalIgnoreCase));

                    if (isLongFormat)
                    {
                        return ReadLongFormatCsv(filePath);
                    }
                    else
                    {
                        // Assume wide format if first column is timestamp-like and others are groups
                        return ReadWideFormatCsv(filePath);
                    }
                }
            }
            catch (Exception ex) when (!ex.Message.Contains("CSV file"))
            {
                throw new Exception($"Error reading CSV file '{Path.GetFileName(filePath)}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reads LONG format CSV: timestamp,group_id,value
        /// </summary>
        private Dictionary<(DateTime, string), double> ReadLongFormatCsv(string filePath)
        {
            var result = new Dictionary<(DateTime, string), double>();
            var skippedRecords = new List<string>();
            int totalRecords = 0;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<CsvRecord>();

                foreach (var record in records)
                {
                    totalRecords++;
                    try
                    {
                        var timestamp = record.GetTimestamp();
                        var groupId = record.GroupId;
                        var value = record.GetValue();

                        var key = (timestamp, groupId);

                        if (!result.ContainsKey(key))
                        {
                            result[key] = value;
                        }
                        else
                        {
                            skippedRecords.Add($"Duplicate key: {timestamp:yyyy-MM-dd HH:mm:ss}, {groupId}");
                        }
                    }
                    catch (FormatException ex)
                    {
                        skippedRecords.Add($"Line {totalRecords}: {ex.Message}");
                    }
                }
            }

            ValidateResults(filePath, totalRecords, result.Count, skippedRecords);
            return result;
        }

        /// <summary>
        /// Reads WIDE format CSV: timestamp,group1,group2,group3,...
        /// Converts to LONG format internally
        /// </summary>
        private Dictionary<(DateTime, string), double> ReadWideFormatCsv(string filePath)
        {
            var result = new Dictionary<(DateTime, string), double>();
            var skippedRecords = new List<string>();
            int totalRows = 0;
            int totalRecords = 0;

            using (var reader = new StreamReader(filePath))
            {
                // Read header
                var headerLine = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(headerLine))
                {
                    throw new Exception($"CSV file '{Path.GetFileName(filePath)}' has no header row.");
                }

                var headers = headerLine.Split(',').Select(h => h.Trim().Trim('"', '\uFEFF', ' ')).ToArray();

                // First column should be timestamp
                var timestampColumnName = headers[0];
                var groupColumns = headers.Skip(1).ToArray();

                if (groupColumns.Length == 0)
                {
                    throw new Exception($"CSV file '{Path.GetFileName(filePath)}' has only one column. Expected: timestamp + group columns.");
                }

                // Read data rows
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    totalRows++;
                    var values = line.Split(',');

                    if (values.Length < 2)
                    {
                        skippedRecords.Add($"Row {totalRows}: Insufficient columns");
                        continue;
                    }

                    // Parse timestamp (first column)
                    if (!DateTime.TryParse(values[0].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
                    {
                        skippedRecords.Add($"Row {totalRows}: Invalid timestamp '{values[0]}'");
                        continue;
                    }

                    // Parse each group column
                    for (int i = 1; i < values.Length && i < headers.Length; i++)
                    {
                        totalRecords++;
                        var groupId = groupColumns[i - 1];
                        var valueString = values[i].Trim();

                        if (string.IsNullOrWhiteSpace(valueString))
                        {
                            skippedRecords.Add($"Row {totalRows}, Group {groupId}: Empty value");
                            continue;
                        }

                        if (double.TryParse(valueString, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            var key = (timestamp, groupId);
                            if (!result.ContainsKey(key))
                            {
                                result[key] = value;
                            }
                            else
                            {
                                skippedRecords.Add($"Row {totalRows}, Group {groupId}: Duplicate key");
                            }
                        }
                        else
                        {
                            skippedRecords.Add($"Row {totalRows}, Group {groupId}: Invalid value '{valueString}'");
                        }
                    }
                }
            }

            ValidateResults(filePath, totalRows, result.Count, skippedRecords);

            System.Diagnostics.Debug.WriteLine($"Wide CSV: Read {totalRows} rows × {result.Select(r => r.Key.Item2).Distinct().Count()} groups = {result.Count} records");

            return result;
        }

        /// <summary>
        /// Validates that CSV reading was successful
        /// </summary>
        private void ValidateResults(string filePath, int totalRecords, int successfulRecords, List<string> skippedRecords)
        {
            if (totalRecords == 0)
            {
                throw new Exception($"CSV file '{Path.GetFileName(filePath)}' appears to be empty or contains only headers.");
            }

            if (successfulRecords == 0 && totalRecords > 0)
            {
                var errorSummary = string.Join("\n", skippedRecords.Take(10));
                throw new Exception($"CSV file '{Path.GetFileName(filePath)}' has {totalRecords} rows but 0 were successfully parsed.\n\nFirst errors:\n{errorSummary}");
            }

            if (skippedRecords.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"CSV Warning: Skipped {skippedRecords.Count} records out of {totalRecords}.");
                foreach (var msg in skippedRecords.Take(5))
                {
                    System.Diagnostics.Debug.WriteLine($"  - {msg}");
                }
            }
        }

        /// <summary>
        /// Matches forecast and actual data, creating DataPoint objects for matched records
        /// </summary>
        /// <param name="forecastData">Dictionary of forecast data</param>
        /// <param name="actualData">Dictionary of actual data</param>
        /// <returns>List of matched DataPoint objects with calculated offsets</returns>
        public List<DataPoint> MatchData(
            Dictionary<(DateTime, string), double> forecastData,
            Dictionary<(DateTime, string), double> actualData,
            out int unmatchedForecast,
            out int unmatchedActual)
        {
            var result = new List<DataPoint>();
            var matchedKeys = new HashSet<(DateTime, string)>();

            // Find all matching records
            foreach (var kvp in forecastData)
            {
                var key = kvp.Key;
                var forecastValue = kvp.Value;

                if (actualData.TryGetValue(key, out double actualValue))
                {
                    var dataPoint = new DataPoint
                    {
                        Timestamp = key.Item1,
                        GroupId = key.Item2,
                        Forecast = forecastValue,
                        Actual = actualValue
                    };

                    dataPoint.CalculateOffsets();
                    result.Add(dataPoint);
                    matchedKeys.Add(key);
                }
            }

            // Count unmatched records
            unmatchedForecast = forecastData.Count - matchedKeys.Count;
            unmatchedActual = actualData.Keys.Count(k => !matchedKeys.Contains(k));

            return result;
        }
    }
}
