namespace AssumptionCalculator.Models
{
    /// <summary>
    /// Represents a single data point with forecast and actual values,
    /// along with calculated offset metrics.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// The timestamp of the measurement
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The group identifier (e.g., region, category, etc.)
        /// </summary>
        public string GroupId { get; set; } = string.Empty;

        /// <summary>
        /// The forecasted value
        /// </summary>
        public double Forecast { get; set; }

        /// <summary>
        /// The actual observed value
        /// </summary>
        public double Actual { get; set; }

        /// <summary>
        /// The difference in units (Forecast - Actual)
        /// </summary>
        public double OffsetUnits { get; set; }

        /// <summary>
        /// The percentage offset: (OffsetUnits / Actual) * 100
        /// </summary>
        public double OffsetPercent { get; set; }

        /// <summary>
        /// The absolute value of OffsetPercent for easier aggregation
        /// </summary>
        public double OffsetPercentAbs { get; set; }

        /// <summary>
        /// Calculates the offset metrics based on forecast and actual values
        /// </summary>
        public void CalculateOffsets()
        {
            OffsetUnits = Forecast - Actual;

            // Avoid division by zero
            if (Actual != 0)
            {
                OffsetPercent = (OffsetUnits / Actual) * 100;
                OffsetPercentAbs = Math.Abs(OffsetPercent);
            }
            else
            {
                OffsetPercent = 0;
                OffsetPercentAbs = 0;
            }
        }
    }
}
