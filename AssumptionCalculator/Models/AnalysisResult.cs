namespace AssumptionCalculator.Models
{
    /// <summary>
    /// Holds the results of an accuracy analysis, including all matched data points
    /// and summary statistics.
    /// </summary>
    public class AnalysisResult
    {
        /// <summary>
        /// List of all matched data points with calculated offsets
        /// </summary>
        public List<DataPoint> DataPoints { get; set; } = new List<DataPoint>();

        /// <summary>
        /// Overall Mean Absolute Percentage Error across all data points
        /// </summary>
        public double OverallMAPE { get; set; }

        /// <summary>
        /// MAPE calculated per group
        /// Dictionary key: GroupId, Dictionary value: MAPE for that group
        /// </summary>
        public Dictionary<string, double> GroupMAPE { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Total number of matched records
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Number of records in forecast that didn't have a match in actual
        /// </summary>
        public int UnmatchedForecastRecords { get; set; }

        /// <summary>
        /// Number of records in actual that didn't have a match in forecast
        /// </summary>
        public int UnmatchedActualRecords { get; set; }
    }
}
