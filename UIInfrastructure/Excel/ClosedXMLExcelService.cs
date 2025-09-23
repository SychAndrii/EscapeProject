using System.Runtime.InteropServices;
using ClosedXML.Excel;
using UIApplication.Excel;
using UIDomain.Select;
using UIDomain.Text;

namespace UIInfrastructure.Excel
{
    public class ClosedXMLExcelService : ExcelService
    {
        private XLWorkbook workbook;
        private Dictionary<string, IXLWorksheet> worksheetMap;
        private IXLWorksheet currentSheet;
        private readonly string hiddenOptionsSheetName = "__Options__";

        public ClosedXMLExcelService(ExcelMetadataBuilder metadataBuilder)
            : base(metadataBuilder)
        {
        }

        protected override void OnMetadataSet()
        {
            workbook = new XLWorkbook();
            worksheetMap = [];

            IXLWorksheet hiddenOptionsSheet = workbook.Worksheets.Add(hiddenOptionsSheetName);
            hiddenOptionsSheet.Hide();
        }

        protected override void OnCurrentWorksheetChanged()
        {
            currentSheet = worksheetMap[CurrentWorksheet];
        }

        protected override void OnNewWorksheetCreated(string worksheet)
        {
            IXLWorksheet sheet = workbook.Worksheets.Add(worksheet);
            worksheetMap[worksheet] = sheet;

            // Add column headers
            int colIdx = 1;
            foreach (var colSettingsBuilder in ExcelMetadata.Columns)
            {
                TextSettings colSettings = colSettingsBuilder.Build();
                IXLCell cell = sheet.Cell(1, colIdx);
                cell.Value = colSettings.Text;

                // Apply font
                var font = cell.Style.Font;
                font.FontSize = colSettings.FontSize;
                font.FontName = colSettings.Font.ToString();
                font.Bold = colSettings.FontWeight == TextWeight.BOLD;
                font.Italic = colSettings.FontStyle == TextStyle.ITALIC;
                cell.Style.Protection.SetLocked(true);
                colIdx++;
            }
        }

        protected override void OnWorksheetDeleted(string worksheet)
        {
            workbook.Worksheet(worksheet).Delete();
            worksheetMap.Remove(worksheet);
        }

        public override void RenderText(TextSettingsBuilder settingsBuilder)
        {
            var settings = settingsBuilder.Build();
            string value = settings.Text ?? string.Empty;

            int row = CurrentPos.row + 2;
            List<string> columnValues = [];
            foreach (var columnSettingsBuilder in ExcelMetadata.Columns)
            {
                columnValues.Add(columnSettingsBuilder.Build().Text);
            }
            int col = columnValues.IndexOf(CurrentPos.col) + 1;

            IXLCell cell = currentSheet.Cell(row, col);
            cell.Value = value;

            // Apply font
            var font = cell.Style.Font;
            font.FontSize = settings.FontSize;
            font.FontName = settings.Font.ToString();
            font.Bold = settings.FontWeight == TextWeight.BOLD;
            font.Italic = settings.FontStyle == TextStyle.ITALIC;
            cell.Style.Protection.SetLocked(true);
        }

        [Obsolete]
        public override void RenderSelect(SelectSettingsBuilder settingsBuilder)
        {
            var settings = settingsBuilder.Build();
            int row = CurrentPos.row + 2;
            List<string> columnValues = [];
            foreach (var columnSettingsBuilder in ExcelMetadata.Columns)
            {
                columnValues.Add(columnSettingsBuilder.Build().Text);
            }
            int col = columnValues.IndexOf(CurrentPos.col) + 1;

            IXLCell cell = currentSheet.Cell(row, col);

            // Get selected text and settings
            var selectedTextSettings = settings.Options[settings.SelectedOptionIndex].Build();
            cell.Value = selectedTextSettings.Text;

            // Apply font styling to the selected option cell
            var font = cell.Style.Font;
            font.FontSize = selectedTextSettings.FontSize;
            font.FontName = selectedTextSettings.Font.ToString();
            font.Bold = selectedTextSettings.FontWeight == TextWeight.BOLD;
            font.Italic = selectedTextSettings.FontStyle == TextStyle.ITALIC;

            // Allow editing for dropdowns
            cell.Style.Protection.Locked = false;

            // Write options to hidden sheet
            IXLWorksheet optionsSheet = workbook.Worksheet(hiddenOptionsSheetName);
            string namedRange = $"Options_{Guid.NewGuid():N}";

            int optionsStartRow = optionsSheet.LastRowUsed()?.RowNumber() + 1 ?? 1;
            for (int i = 0; i < settings.Options.Count; i++)
            {
                optionsSheet.Cell(optionsStartRow + i, 1).Value = settings.Options[i].Build().Text;
            }

            var optionsRange = optionsSheet.Range(optionsStartRow, 1,
                optionsStartRow + settings.Options.Count - 1, 1);
            workbook.NamedRanges.Add(namedRange, optionsRange);

            // Add data validation dropdown
            var validation = cell.DataValidation;
            validation.AllowedValues = XLAllowedValues.List;
            validation.InCellDropdown = true;
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "Invalid Selection";
            validation.ErrorMessage = "Please select a value from the dropdown list.";
            validation.List($"={namedRange}");
        }

        public override void Close()
        {
            foreach (var sheet in workbook.Worksheets)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    sheet.Columns().AdjustToContents(); // This causes a lot of problems on Linux
                }
                sheet.Protect();
            }

            workbook.SaveAs(ExcelMetadata.Destination);
            workbook.Dispose();
        }
    }
}