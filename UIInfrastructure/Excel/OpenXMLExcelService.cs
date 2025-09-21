using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using UIApplication.Excel;
using UIDomain.Checkbox;
using UIDomain.Text;

namespace UIInfrastructure.Excel
{
    public class OpenXMLExcelService : ExcelService
    {
        private SpreadsheetDocument? document;
        private WorkbookPart? workbookPart;
        private Sheets? sheets;
        private Sheet? currentSheet;
        public OpenXMLExcelService(ExcelMetadataBuilder metadataBuilder) : base(metadataBuilder) { }

        public override void Close()
        {
            workbookPart?.Workbook.Save();
            document?.Dispose();
            document = null;
            workbookPart = null;
            sheets = null;
        }

        public override void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder)
        {
            if (currentSheet == null)
            {
                throw new InvalidOperationException("No active sheet.");
            }

            var settings = settingsBuilder.Build();
            string checkboxChar = settings.DefaultValue == CheckboxState.ON ? "☑" : "☐";

            WorksheetPart wsPart = (WorksheetPart)workbookPart.GetPartById(currentSheet.Id!);
            SheetData sheetData = wsPart.Worksheet.GetFirstChild<SheetData>();

            // ensure row exists
            Row row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == (uint)CurrentPos.row);
            if (row == null)
            {
                row = new Row { RowIndex = (uint)CurrentPos.row };
                sheetData.Append(row);
            }

            // map column name → index
            int colIndex = ExcelMetadata.Columns.IndexOf(CurrentPos.col) + 1;
            if (colIndex <= 0)
            {
                throw new Exception($"Column '{CurrentPos.col}' not found in metadata.");
            }

            string colName = GetExcelColumnName(colIndex);
            string cellRef = $"{colName}{CurrentPos.row}";

            Cell cell = new Cell
            {
                CellReference = cellRef,
                DataType = CellValues.String,
                CellValue = new CellValue(checkboxChar)
            };

            row.Append(cell);
            wsPart.Worksheet.Save();
        }

        public override void RenderText(TextSettingsBuilder settingsBuilder)
        {
            if (currentSheet == null)
            {
                throw new InvalidOperationException("No active sheet.");
            }

            var settings = settingsBuilder.Build();
            string text = settings.Text ?? string.Empty;

            WorksheetPart wsPart = (WorksheetPart)workbookPart.GetPartById(currentSheet.Id!);
            SheetData sheetData = wsPart.Worksheet.GetFirstChild<SheetData>();

            // ensure row exists
            Row row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == (uint)CurrentPos.row);
            if (row == null)
            {
                row = new Row { RowIndex = (uint)CurrentPos.row };
                sheetData.Append(row);
            }

            // map column name → index (A,B,C,…)
            int colIndex = ExcelMetadata.Columns.IndexOf(CurrentPos.col) + 1;
            if (colIndex <= 0)
            {
                throw new Exception($"Column '{CurrentPos.col}' not found in metadata.");
            }

            string colName = GetExcelColumnName(colIndex);
            string cellRef = $"{colName}{CurrentPos.row}";

            Cell cell = new Cell
            {
                CellReference = cellRef,
                DataType = CellValues.String,
                CellValue = new CellValue(text)
            };

            row.Append(cell);
            wsPart.Worksheet.Save();
        }

        protected override void OnCurrentWorksheetChanged()
        {
            currentSheet = workbookPart.Workbook.Sheets
                .Elements<Sheet>()
                .FirstOrDefault(s => s.Name == CurrentWorksheet);
        }

        protected override void OnMetadataSet()
        {
            document = SpreadsheetDocument.Create(
                ExcelMetadata.Destination,
                SpreadsheetDocumentType.Workbook);

            workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            sheets = workbookPart.Workbook.AppendChild(new Sheets());
        }

        protected override void OnNewWorksheetCreated(string worksheetName)
        {
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            uint sheetId = (uint)(sheets.Count() + 1);
            Sheet sheet = new Sheet()
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = sheetId,
                Name = worksheetName
            };
            sheets.Append(sheet);

            // Get the sheetData
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Add a header row
            Row row = new Row() { RowIndex = 1 };

            int columnsCount = ExcelMetadata.Columns.Count;
            for (int i = 0; i < columnsCount; i++)
            {
                string columnName = GetExcelColumnName(i + 1); // e.g. 1->A, 2->B, 27->AA
                string cellRef = $"{columnName}1";

                Cell newCell = new Cell
                {
                    CellReference = cellRef,
                    DataType = CellValues.String,
                    CellValue = new CellValue(ExcelMetadata.Columns[i])
                };

                row.Append(newCell);
            }
            sheetData.Append(row);
        }

        protected override void OnWorksheetDeleted(string worksheet)
        {
            // 1. Find the <sheet> element by name
            Sheet sheet = workbookPart.Workbook.Sheets
                .Elements<Sheet>()
                .FirstOrDefault(s => s.Name == worksheet);

            // 2. Get the target WorksheetPart from the Id
            WorksheetPart worksheetPart =
                (WorksheetPart)workbookPart.GetPartById(sheet.Id!);

            // 3. Remove the <sheet> entry from the workbook
            sheet.Remove();

            // 4. Delete the part itself
            workbookPart.DeletePart(worksheetPart);

            // 5. Save changes to the workbook
            workbookPart.Workbook.Save();
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }
            return columnName;
        }
    }
}
