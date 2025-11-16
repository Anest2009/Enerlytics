# Assumption Accuracy Calculator - User Guide

## Overview

The Assumption Accuracy Calculator is a Windows Forms application that compares forecast data against actual data to measure prediction accuracy. It calculates offset percentages and provides statistical analysis including Mean Absolute Percentage Error (MAPE).

## Features

- **CSV File Import**: Load both forecast and actual CSV files
- **Automatic Data Matching**: Matches records based on timestamp and group ID
- **Accuracy Metrics**: Calculates offset units, offset percentages, and MAPE
- **Visual Results**: Displays results in a sortable data grid
- **Summary Statistics**: Shows overall and per-group MAPE
- **Export Capability**: Export analysis results to CSV format

## CSV File Requirements

### Required Columns

Both forecast and actual CSV files must contain these three columns (column names are case-insensitive and support variations):

1. **Timestamp/DateTime Column**
   - Supported column names: `timestamp`, `datetime`, `date`, `time`, `Timestamp`, `DateTime`, `Date`, `Time`
   - Format: Any standard date/time format (e.g., `2024-08-08 23:00`, `2024-08-08 23:00:00`)
   - Must be parseable by C# DateTime.TryParse()

2. **Group ID Column**
   - Supported column names: `group_id`, `groupid`, `group`, `id`, `GroupId`, `Group`, `ID`
   - Format: Text/string identifier
   - Examples: `GroupA`, `Region1`, `Category_X`

3. **Value Column**
   - Supported column names: `value`, `units`, `amount`, `quantity`, `Value`, `Units`, `Amount`, `Quantity`
   - Format: Numeric value (integer or decimal)
   - Examples: `8`, `7.7`, `125.5`

### Example CSV Structure

**Forecast.csv:**
```csv
timestamp,group_id,value
2024-08-08 23:00,GroupA,8.0
2024-08-08 23:00,GroupB,15.5
2024-08-09 00:00,GroupA,10.2
```

**Actual.csv:**
```csv
timestamp,group_id,value
2024-08-08 23:00,GroupA,7.7
2024-08-08 23:00,GroupB,16.0
2024-08-09 00:00,GroupA,9.8
```

### Alternative Column Names Example

The application supports various column name variations:

```csv
DateTime,Group,Units
2024-08-08 23:00,GroupA,8.0
```

or

```csv
date,id,amount
2024-08-08 23:00,GroupA,8.0
```

## How to Use the Application

### Step 1: Launch the Application

1. Open Visual Studio and load the `AssumptionCalculator.sln` solution
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5)

Alternatively, run the compiled executable:
- Navigate to: `AssumptionCalculator/bin/Debug/net8.0-windows/`
- Double-click: `AssumptionCalculator.exe`

### Step 2: Load CSV Files

1. **Select Forecast CSV:**
   - Click the "Browse..." button next to "Forecast CSV:"
   - Navigate to your forecast CSV file
   - Click "Open"
   - The selected filename will appear next to the button

2. **Select Actual CSV:**
   - Click the "Browse..." button next to "Actual CSV:"
   - Navigate to your actual CSV file
   - Click "Open"
   - The selected filename will appear next to the button

### Step 3: Run Analysis

1. Click the **"Run Accuracy Analysis"** button (becomes enabled after both files are selected)
2. The application will:
   - Parse both CSV files
   - Match records by timestamp and group ID
   - Calculate offset metrics
   - Display results in the grid
3. A success message will show:
   - Number of matched records
   - Number of unmatched forecast records
   - Number of unmatched actual records

### Step 4: Review Results

#### Summary Statistics Panel

The summary panel displays:
- **Overall MAPE**: Mean Absolute Percentage Error across all records
- **Total Records**: Number of matched data points
- **Per-Group MAPE**: MAPE broken down by each group ID

Example:
```
Overall MAPE: 5.23%   |   Total Records: 150

Per-Group MAPE:
  GroupA: 4.12%
  GroupB: 6.34%
  GroupC: 5.89%
```

#### Results Data Grid

The data grid shows all matched records with the following columns:

| Column | Description |
|--------|-------------|
| **Timestamp** | Date and time of the measurement |
| **Group ID** | The group identifier |
| **Forecast** | The predicted value |
| **Actual** | The actual observed value |
| **Offset Units** | Difference (Forecast - Actual) |
| **Offset %** | Percentage offset: (OffsetUnits / Actual) × 100 |
| **Offset % (Abs)** | Absolute value of Offset % |

You can:
- **Sort** by clicking column headers
- **Scroll** through all results
- **Resize** columns by dragging column borders

### Step 5: Export Results

1. Click the **"Export Results to CSV"** button
2. Choose a save location and filename
3. Click "Save"
4. The results will be exported with all columns

**Default filename format:** `AccuracyAnalysis_YYYYMMDD_HHMMSS.csv`

## Calculation Formulas

The application uses these formulas:

```
OffsetUnits = Forecast - Actual

OffsetPercent = (OffsetUnits / Actual) × 100

OffsetPercentAbs = |OffsetPercent|

MAPE = Average(OffsetPercentAbs) for all data points
```

### Example Calculation

Given:
- Forecast: 8.0 units
- Actual: 7.7 units

Calculations:
- OffsetUnits = 8.0 - 7.7 = 0.3
- OffsetPercent = (0.3 / 7.7) × 100 = 3.90%
- OffsetPercentAbs = 3.90%

## Data Matching Rules

1. **Composite Key**: Records are matched using a composite key of (Timestamp + Group ID)
2. **Both Must Match**: Only records that exist in BOTH files with identical timestamp and group ID are analyzed
3. **Unmatched Records**: Records that exist in only one file are counted but not included in calculations
4. **Duplicate Keys**: If duplicate keys exist in a file, only the first occurrence is used

## Troubleshooting

### "Unable to parse timestamp" Error

**Problem**: The timestamp format in your CSV isn't recognized

**Solutions**:
- Ensure timestamps are in a standard format: `YYYY-MM-DD HH:MM:SS` or `YYYY-MM-DD HH:MM`
- Check for extra spaces or special characters
- Verify the date separator (use `-` or `/`)
- Example valid formats:
  - `2024-08-08 23:00:00`
  - `2024-08-08 23:00`
  - `2024/08/08 23:00`

### "Unable to parse value" Error

**Problem**: The value column contains non-numeric data

**Solutions**:
- Ensure all values are numbers (integers or decimals)
- Remove any currency symbols ($, €, etc.)
- Remove any commas from large numbers (use `1000` not `1,000`)
- Use decimal point (`.`) not comma for decimals

### No Records Matched

**Problem**: Zero matched records after analysis

**Possible Causes**:
1. Timestamps don't match exactly between files
2. Group IDs are spelled differently
3. Date formats differ between files

**Solutions**:
- Check timestamp formatting is identical in both files
- Verify group ID spelling matches exactly (case-sensitive)
- Ensure both files have overlapping time periods

### Wrong Column Names

**Problem**: Application can't find the required columns

**Solution**: Ensure your CSV has columns matching one of these patterns:
- Timestamp: `timestamp`, `datetime`, `date`, `time`
- Group: `group_id`, `groupid`, `group`, `id`
- Value: `value`, `units`, `amount`, `quantity`

If you need to use different column names, you can modify the `CsvRecord.cs` file:

**Location**: `AssumptionCalculator/Models/CsvRecord.cs`

**Lines to modify**:
```csharp
// Line 14: Add your timestamp column name
[Name("timestamp", "datetime", "YOUR_COLUMN_NAME")]

// Line 22: Add your group ID column name
[Name("group_id", "groupid", "YOUR_COLUMN_NAME")]

// Line 30: Add your value column name
[Name("value", "units", "YOUR_COLUMN_NAME")]
```

## Project Structure

```
AssumptionCalculator/
├── Models/
│   ├── DataPoint.cs          - Data model for matched records
│   ├── AnalysisResult.cs     - Container for analysis results
│   └── CsvRecord.cs          - CSV parsing model (MODIFY HERE for column names)
├── Services/
│   ├── CsvService.cs         - CSV reading and matching logic
│   └── AnalysisService.cs    - MAPE calculation and export logic
├── Form1.cs                  - Main UI logic and event handlers
├── Form1.Designer.cs         - UI layout and controls
└── Program.cs                - Application entry point
```

## Technical Details

- **Framework**: .NET 8.0 (Windows)
- **UI Framework**: Windows Forms
- **CSV Library**: CsvHelper 33.0.1
- **Minimum Requirements**: Windows 10 or later, .NET 8.0 Runtime

## Support

For issues or questions:
1. Check this user guide
2. Review error messages carefully
3. Verify CSV file format matches requirements
4. Check the project files in the repository

## License

This application is part of the Missing-Date-Checker repository.
