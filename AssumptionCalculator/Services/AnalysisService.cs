using AssumptionCalculator.Models;

namespace AssumptionCalculator.Services
{
    /// <summary>
    /// Service for performing accuracy analysis calculations
    /// </summary>
    public class AnalysisService
    {
        /// <summary>
        /// Performs complete accuracy analysis on matched data points
        /// </summary>
        /// <param name="dataPoints">List of matched data points with calculated offsets</param>
        /// <param name="unmatchedForecast">Number of unmatched forecast records</param>
        /// <param name="unmatchedActual">Number of unmatched actual records</param>
        /// <returns>AnalysisResult containing all metrics and statistics</returns>
        public AnalysisResult PerformAnalysis(
            List<DataPoint> dataPoints,
            int unmatchedForecast,
            int unmatchedActual)
        {
            var result = new AnalysisResult
            {
                DataPoints = dataPoints,
                TotalRecords = dataPoints.Count,
                UnmatchedForecastRecords = unmatchedForecast,
                UnmatchedActualRecords = unmatchedActual
            };

            if (dataPoints.Count == 0)
            {
                result.OverallMAPE = 0;
                return result;
            }

            // Calculate Overall MAPE
            result.OverallMAPE = CalculateMAPE(dataPoints);

            // Calculate Per-Group MAPE
            result.GroupMAPE = CalculateGroupMAPE(dataPoints);

            return result;
        }

        /// <summary>
        /// Calculates Mean Absolute Percentage Error for a list of data points
        /// </summary>
        /// <param name="dataPoints">List of data points</param>
        /// <returns>MAPE as a percentage</returns>
        private double CalculateMAPE(List<DataPoint> dataPoints)
        {
            if (dataPoints.Count == 0)
                return 0;

            double sum = dataPoints.Sum(dp => dp.OffsetPercentAbs);
            return sum / dataPoints.Count;
        }

        /// <summary>
        /// Calculates MAPE for each group separately
        /// </summary>
        /// <param name="dataPoints">List of data points</param>
        /// <returns>Dictionary mapping GroupId to MAPE</returns>
        private Dictionary<string, double> CalculateGroupMAPE(List<DataPoint> dataPoints)
        {
            var result = new Dictionary<string, double>();

            var groups = dataPoints.GroupBy(dp => dp.GroupId);

            foreach (var group in groups)
            {
                var groupDataPoints = group.ToList();
                var mape = CalculateMAPE(groupDataPoints);
                result[group.Key] = mape;
            }

            return result;
        }

        /// <summary>
        /// Exports data points to a CSV file
        /// </summary>
        /// <param name="dataPoints">List of data points to export</param>
        /// <param name="filePath">Destination file path</param>
        public void ExportToCSV(List<DataPoint> dataPoints, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    // Write header
                    writer.WriteLine("Timestamp,GroupId,Forecast,Actual,OffsetUnits,OffsetPercent,OffsetPercentAbs");

                    // Write data rows
                    foreach (var dp in dataPoints)
                    {
                        writer.WriteLine($"{dp.Timestamp:yyyy-MM-dd HH:mm:ss},{dp.GroupId},{dp.Forecast},{dp.Actual},{dp.OffsetUnits},{dp.OffsetPercent},{dp.OffsetPercentAbs}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting to CSV: {ex.Message}", ex);
            }
        }
    }
}
