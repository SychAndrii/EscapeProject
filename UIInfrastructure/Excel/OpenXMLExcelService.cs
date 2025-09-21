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
        public OpenXMLExcelService(ExcelMetadataBuilder metadataBuilder) : base(metadataBuilder) { }

        public override void Close()
        {
            workbookPart?.Workbook.Save();  // save the workbook definition

            document?.Dispose(); // important: Close, not Dispose
            document = null;
            workbookPart = null;
            sheets = null;
        }

        public override void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder)
        {
            throw new NotImplementedException();
        }

        public override void RenderText(TextSettingsBuilder settingsBuilder)
        {
            throw new NotImplementedException();
        }

        protected override void OnCurrentWorksheetChanged()
        {

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

        protected override void OnWorksheetDeleted(string worksheet)
        {
            throw new NotImplementedException();
        }
    }
}
