using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace AssumptionCalculator.Models
{
    /// <summary>
    /// Represents a row from the CSV file.
    /// Maps common column names for timestamp, group_id, and value.
    /// </summary>
    public class CsvRecord
    {
        /// <summary>
        /// Timestamp field - supports multiple column name variations
        /// Common names: timestamp, datetime, date, time
        /// </summary>
        [Name("timestamp", "datetime", "date", "time", "Timestamp", "DateTime", "Date", "Time")]
        public string TimestampString { get; set; } = string.Empty;

        /// <summary>
        /// Group ID field - supports multiple column name variations
        /// Common names: group_id, groupid, group, id
        /// </summary>
        [Name("group_id", "groupid", "group", "id", "GroupId", "Group", "ID")]
        public string GroupId { get; set; } = string.Empty;

        /// <summary>
        /// Value field - supports multiple column name variations
        /// Common names: value, units, amount, quantity
        /// </summary>
        [Name("value", "units", "amount", "quantity", "Value", "Units", "Amount", "Quantity")]
        public string ValueString { get; set; } = string.Empty;

        /// <summary>
        /// Parses the timestamp string to DateTime using InvariantCulture
        /// </summary>
        public DateTime GetTimestamp()
        {
            if (DateTime.TryParse(TimestampString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new FormatException($"Unable to parse timestamp: '{TimestampString}'");
        }

        /// <summary>
        /// Parses the value string to double using InvariantCulture
        /// </summary>
        public double GetValue()
        {
            if (double.TryParse(ValueString, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            throw new FormatException($"Unable to parse value: '{ValueString}'");
        }
    }
}
