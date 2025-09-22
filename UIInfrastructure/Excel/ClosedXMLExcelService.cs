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
            foreach (var colName in ExcelMetadata.Columns)
            {
                sheet.Cell(1, colIdx).Value = colName;
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
            int col = ExcelMetadata.Columns.IndexOf(CurrentPos.col) + 1;

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
            int col = ExcelMetadata.Columns.IndexOf(CurrentPos.col) + 1;
            IXLCell cell = currentSheet.Cell(row, col);
            cell.Style.Protection.SetLocked(false);

            var selectedText = settings.Options[settings.SelectedOptionIndex].Build().Text;
            cell.Value = selectedText;

            // Dropdown options on hidden sheet
            IXLWorksheet optionsSheet = workbook.Worksheet(hiddenOptionsSheetName);
            string namedRange = $"Options_{Guid.NewGuid():N}";

            int optionsStartRow = optionsSheet.LastRowUsed()?.RowNumber() + 1 ?? 1;
            for (int i = 0; i < settings.Options.Count; i++)
            {
                optionsSheet.Cell(optionsStartRow + i, 1).Value = settings.Options[i].Build().Text;
            }

            // Create named range
            var optionsRange = optionsSheet.Range(optionsStartRow, 1, optionsStartRow + settings.Options.Count - 1, 1);
            workbook.NamedRanges.Add(namedRange, optionsRange);

            // Create the data validation correctly
            var dataValidations = currentSheet.DataValidations;

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
                sheet.Columns().AdjustToContents();
                sheet.Protect();
            }

            workbook.SaveAs(ExcelMetadata.Destination);
            workbook.Dispose();
        }
    }
}