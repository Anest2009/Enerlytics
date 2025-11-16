namespace AssumptionCalculator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblForecast = new System.Windows.Forms.Label();
            this.btnBrowseForecast = new System.Windows.Forms.Button();
            this.lblForecastPath = new System.Windows.Forms.Label();
            this.lblActual = new System.Windows.Forms.Label();
            this.btnBrowseActual = new System.Windows.Forms.Button();
            this.lblActualPath = new System.Windows.Forms.Label();
            this.btnRunAnalysis = new System.Windows.Forms.Button();
            this.groupBoxSummary = new System.Windows.Forms.GroupBox();
            this.lblSummaryStats = new System.Windows.Forms.Label();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBoxSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            //
            // lblForecast
            //
            this.lblForecast.AutoSize = true;
            this.lblForecast.Location = new System.Drawing.Point(12, 15);
            this.lblForecast.Name = "lblForecast";
            this.lblForecast.Size = new System.Drawing.Size(82, 15);
            this.lblForecast.TabIndex = 0;
            this.lblForecast.Text = "Forecast CSV:";
            //
            // btnBrowseForecast
            //
            this.btnBrowseForecast.Location = new System.Drawing.Point(100, 11);
            this.btnBrowseForecast.Name = "btnBrowseForecast";
            this.btnBrowseForecast.Size = new System.Drawing.Size(100, 25);
            this.btnBrowseForecast.TabIndex = 1;
            this.btnBrowseForecast.Text = "Browse...";
            this.btnBrowseForecast.UseVisualStyleBackColor = true;
            this.btnBrowseForecast.Click += new System.EventHandler(this.BtnBrowseForecast_Click);
            //
            // lblForecastPath
            //
            this.lblForecastPath.AutoSize = true;
            this.lblForecastPath.Location = new System.Drawing.Point(210, 15);
            this.lblForecastPath.Name = "lblForecastPath";
            this.lblForecastPath.Size = new System.Drawing.Size(43, 15);
            this.lblForecastPath.TabIndex = 2;
            this.lblForecastPath.Text = "(none)";
            //
            // lblActual
            //
            this.lblActual.AutoSize = true;
            this.lblActual.Location = new System.Drawing.Point(12, 45);
            this.lblActual.Name = "lblActual";
            this.lblActual.Size = new System.Drawing.Size(69, 15);
            this.lblActual.TabIndex = 3;
            this.lblActual.Text = "Actual CSV:";
            //
            // btnBrowseActual
            //
            this.btnBrowseActual.Location = new System.Drawing.Point(100, 41);
            this.btnBrowseActual.Name = "btnBrowseActual";
            this.btnBrowseActual.Size = new System.Drawing.Size(100, 25);
            this.btnBrowseActual.TabIndex = 4;
            this.btnBrowseActual.Text = "Browse...";
            this.btnBrowseActual.UseVisualStyleBackColor = true;
            this.btnBrowseActual.Click += new System.EventHandler(this.BtnBrowseActual_Click);
            //
            // lblActualPath
            //
            this.lblActualPath.AutoSize = true;
            this.lblActualPath.Location = new System.Drawing.Point(210, 45);
            this.lblActualPath.Name = "lblActualPath";
            this.lblActualPath.Size = new System.Drawing.Size(43, 15);
            this.lblActualPath.TabIndex = 5;
            this.lblActualPath.Text = "(none)";
            //
            // btnRunAnalysis
            //
            this.btnRunAnalysis.Enabled = false;
            this.btnRunAnalysis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRunAnalysis.Location = new System.Drawing.Point(12, 75);
            this.btnRunAnalysis.Name = "btnRunAnalysis";
            this.btnRunAnalysis.Size = new System.Drawing.Size(776, 35);
            this.btnRunAnalysis.TabIndex = 6;
            this.btnRunAnalysis.Text = "Run Accuracy Analysis";
            this.btnRunAnalysis.UseVisualStyleBackColor = true;
            this.btnRunAnalysis.Click += new System.EventHandler(this.BtnRunAnalysis_Click);
            //
            // groupBoxSummary
            //
            this.groupBoxSummary.Controls.Add(this.lblSummaryStats);
            this.groupBoxSummary.Location = new System.Drawing.Point(12, 120);
            this.groupBoxSummary.Name = "groupBoxSummary";
            this.groupBoxSummary.Size = new System.Drawing.Size(776, 100);
            this.groupBoxSummary.TabIndex = 7;
            this.groupBoxSummary.TabStop = false;
            this.groupBoxSummary.Text = "Summary Statistics";
            //
            // lblSummaryStats
            //
            this.lblSummaryStats.AutoSize = true;
            this.lblSummaryStats.Location = new System.Drawing.Point(15, 25);
            this.lblSummaryStats.Name = "lblSummaryStats";
            this.lblSummaryStats.Size = new System.Drawing.Size(217, 15);
            this.lblSummaryStats.TabIndex = 0;
            this.lblSummaryStats.Text = "No analysis performed yet. Load CSV files and click Run.";
            //
            // dataGridViewResults
            //
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Location = new System.Drawing.Point(12, 230);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            this.dataGridViewResults.RowHeadersWidth = 51;
            this.dataGridViewResults.Size = new System.Drawing.Size(776, 300);
            this.dataGridViewResults.TabIndex = 8;
            //
            // btnExport
            //
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(12, 540);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(776, 30);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export Results to CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 585);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.groupBoxSummary);
            this.Controls.Add(this.btnRunAnalysis);
            this.Controls.Add(this.lblActualPath);
            this.Controls.Add(this.btnBrowseActual);
            this.Controls.Add(this.lblActual);
            this.Controls.Add(this.lblForecastPath);
            this.Controls.Add(this.btnBrowseForecast);
            this.Controls.Add(this.lblForecast);
            this.Name = "Form1";
            this.Text = "Assumption Accuracy Calculator";
            this.groupBoxSummary.ResumeLayout(false);
            this.groupBoxSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblForecast;
        private System.Windows.Forms.Button btnBrowseForecast;
        private System.Windows.Forms.Label lblForecastPath;
        private System.Windows.Forms.Label lblActual;
        private System.Windows.Forms.Button btnBrowseActual;
        private System.Windows.Forms.Label lblActualPath;
        private System.Windows.Forms.Button btnRunAnalysis;
        private System.Windows.Forms.GroupBox groupBoxSummary;
        private System.Windows.Forms.Label lblSummaryStats;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Button btnExport;
    }
}
