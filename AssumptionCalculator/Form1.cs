using AssumptionCalculator.Models;
using AssumptionCalculator.Services;

namespace AssumptionCalculator
{
    public partial class Form1 : Form
    {
        private string? forecastFilePath;
        private string? actualFilePath;
        private AnalysisResult? currentAnalysisResult;

        private readonly CsvService csvService;
        private readonly AnalysisService analysisService;

        public Form1()
        {
            InitializeComponent();
            csvService = new CsvService();
            analysisService = new AnalysisService();
        }

        /// <summary>
        /// Browse for forecast CSV file
        /// </summary>
        private void BtnBrowseForecast_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.Title = "Select Forecast CSV File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    forecastFilePath = openFileDialog.FileName;
                    lblForecastPath.Text = Path.GetFileName(forecastFilePath);
                    CheckIfReadyToAnalyze();
                }
            }
        }

        /// <summary>
        /// Browse for actual CSV file
        /// </summary>
        private void BtnBrowseActual_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.Title = "Select Actual CSV File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    actualFilePath = openFileDialog.FileName;
                    lblActualPath.Text = Path.GetFileName(actualFilePath);
                    CheckIfReadyToAnalyze();
                }
            }
        }

        /// <summary>
        /// Enables the Run Analysis button if both files are selected
        /// </summary>
        private void CheckIfReadyToAnalyze()
        {
            btnRunAnalysis.Enabled = !string.IsNullOrEmpty(forecastFilePath) &&
                                     !string.IsNullOrEmpty(actualFilePath);
        }

        /// <summary>
        /// Runs the accuracy analysis
        /// </summary>
        private void BtnRunAnalysis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(forecastFilePath) || string.IsNullOrEmpty(actualFilePath))
            {
                MessageBox.Show("Please select both forecast and actual CSV files.",
                    "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Show progress cursor
                this.Cursor = Cursors.WaitCursor;
                btnRunAnalysis.Enabled = false;

                // Read forecast CSV file
                lblSummaryStats.Text = "Reading forecast file...";
                Application.DoEvents();
                var forecastData = csvService.ReadCsvFile(forecastFilePath);

                if (forecastData == null || forecastData.Count == 0)
                {
                    throw new Exception("Forecast file was read but contains no valid data. Please check the file format.");
                }

                // Read actual CSV file
                lblSummaryStats.Text = $"Forecast loaded: {forecastData.Count} records\nReading actual file...";
                Application.DoEvents();
                var actualData = csvService.ReadCsvFile(actualFilePath);

                if (actualData == null || actualData.Count == 0)
                {
                    throw new Exception("Actual file was read but contains no valid data. Please check the file format.");
                }

                // Match data
                lblSummaryStats.Text = $"Forecast: {forecastData.Count} records\nActual: {actualData.Count} records\nMatching data...";
                Application.DoEvents();
                var matchedData = csvService.MatchData(
                    forecastData,
                    actualData,
                    out int unmatchedForecast,
                    out int unmatchedActual);

                if (matchedData == null || matchedData.Count == 0)
                {
                    MessageBox.Show($"No matching records found between forecast and actual files.\n\n" +
                        $"Forecast records: {forecastData.Count}\n" +
                        $"Actual records: {actualData.Count}\n\n" +
                        $"Please ensure both files have matching timestamps and group IDs.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblSummaryStats.Text = "No matching data found.";
                    return;
                }

                // Perform analysis
                lblSummaryStats.Text = "Performing analysis...";
                Application.DoEvents();
                currentAnalysisResult = analysisService.PerformAnalysis(
                    matchedData,
                    unmatchedForecast,
                    unmatchedActual);

                // Display results
                DisplayResults();

                // Enable export button
                btnExport.Enabled = true;

                MessageBox.Show($"Analysis complete!\n\n" +
                    $"Forecast records loaded: {forecastData.Count}\n" +
                    $"Actual records loaded: {actualData.Count}\n" +
                    $"Matched records: {currentAnalysisResult.TotalRecords}\n" +
                    $"Unmatched forecast records: {unmatchedForecast}\n" +
                    $"Unmatched actual records: {unmatchedActual}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblSummaryStats.Text = "Error occurred during analysis.";
                MessageBox.Show($"Error during analysis:\n\n{ex.Message}\n\n" +
                    $"File paths:\n" +
                    $"Forecast: {Path.GetFileName(forecastFilePath)}\n" +
                    $"Actual: {Path.GetFileName(actualFilePath)}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRunAnalysis.Enabled = true;
            }
        }

        /// <summary>
        /// Displays the analysis results in the UI
        /// </summary>
        private void DisplayResults()
        {
            if (currentAnalysisResult == null)
                return;

            // Update summary statistics
            var summaryText = $"Overall MAPE: {currentAnalysisResult.OverallMAPE:F2}%   |   " +
                             $"Total Records: {currentAnalysisResult.TotalRecords}\n\n" +
                             $"Per-Group MAPE:\n";

            foreach (var kvp in currentAnalysisResult.GroupMAPE.OrderBy(x => x.Key))
            {
                summaryText += $"  {kvp.Key}: {kvp.Value:F2}%\n";
            }

            lblSummaryStats.Text = summaryText;

            // Bind data to grid
            dataGridViewResults.DataSource = null;
            dataGridViewResults.DataSource = currentAnalysisResult.DataPoints;

            // Format columns
            FormatDataGrid();
        }

        /// <summary>
        /// Formats the data grid view for better readability
        /// </summary>
        private void FormatDataGrid()
        {
            if (dataGridViewResults.Columns.Count == 0)
                return;

            // Format timestamp column
            if (dataGridViewResults.Columns["Timestamp"] != null)
            {
                dataGridViewResults.Columns["Timestamp"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                dataGridViewResults.Columns["Timestamp"]!.Width = 150;
            }

            // Format numeric columns to 2 decimal places
            var numericColumns = new[] { "Forecast", "Actual", "OffsetUnits", "OffsetPercent", "OffsetPercentAbs" };
            foreach (var colName in numericColumns)
            {
                if (dataGridViewResults.Columns[colName] != null)
                {
                    dataGridViewResults.Columns[colName]!.DefaultCellStyle.Format = "F2";
                    dataGridViewResults.Columns[colName]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // Set column headers
            if (dataGridViewResults.Columns["OffsetUnits"] != null)
                dataGridViewResults.Columns["OffsetUnits"]!.HeaderText = "Offset Units";

            if (dataGridViewResults.Columns["OffsetPercent"] != null)
                dataGridViewResults.Columns["OffsetPercent"]!.HeaderText = "Offset %";

            if (dataGridViewResults.Columns["OffsetPercentAbs"] != null)
                dataGridViewResults.Columns["OffsetPercentAbs"]!.HeaderText = "Offset % (Abs)";

            if (dataGridViewResults.Columns["GroupId"] != null)
                dataGridViewResults.Columns["GroupId"]!.HeaderText = "Group ID";
        }

        /// <summary>
        /// Exports the analysis results to a CSV file
        /// </summary>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (currentAnalysisResult == null || currentAnalysisResult.DataPoints.Count == 0)
            {
                MessageBox.Show("No results to export. Please run analysis first.",
                    "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Analysis Results";
                saveFileDialog.FileName = $"AccuracyAnalysis_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        analysisService.ExportToCSV(currentAnalysisResult.DataPoints, saveFileDialog.FileName);
                        MessageBox.Show($"Results exported successfully to:\n{saveFileDialog.FileName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting results:\n\n{ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
