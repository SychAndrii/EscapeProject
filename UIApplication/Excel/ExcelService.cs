using UIDomain.Checkbox;
using UIDomain.Text;

namespace UIApplication.Excel
{
    public abstract class ExcelService
    {
        /* Excel document state */
        public ExcelMetadata ExcelMetadata
        {
            get;
        }

        private string currentWorksheet;
        public string CurrentWorksheet
        {
            get => currentWorksheet;
            private set
            {
                if (!Worksheets.Contains(value))
                {
                    throw new Exception($"Cannot switch to worksheet [{value}] because it does not exist");
                }
                CurrentWorksheet = value;
            }
        }

        private List<string> worksheets = [];
        public List<string> Worksheets
        {
            get => worksheets;
            private set
            {
                if (value.Count == 0)
                {
                    throw new Exception($"There must be at least worksheet");
                }
                if (value.ToHashSet().Count != value.Count)
                {
                    throw new Exception("All worksheet in excel must be unique");
                }
                Worksheets = value;
            }
        }

        private (int row, string col) currentPos;

        public (int row, string col) CurrentPos
        {
            get => currentPos;
            set
            {
                if (value.row < 0 || value.row >= ExcelMetadata.Dimensions.rows)
                {
                    throw new Exception("Excel row coordinate in Excel document is out of bounds");
                }
                if (!ExcelMetadata.Dimensions.columns.Contains(value.col))
                {
                    throw new Exception("Excel column coordinate in Excel document does not exist");
                }
                currentPos = value;
            }
        }

        protected ExcelService(ExcelMetadataBuilder metadataBuilder)
        {
            ExcelMetadata = metadataBuilder.Build();
            OnMetadataSet();
            string defaultWorksheet = "Sheet 1";
            CreateNewWorksheet(defaultWorksheet);
            GoToWorksheet(defaultWorksheet);
        }

        /* Render methods */
        public abstract void RenderText(TextSettingsBuilder settingsBuilder);
        public abstract void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder);

        /* Excel document state manipulation methods */
        public void GoToWorksheet(string worksheet)
        {
            CurrentWorksheet = worksheet;
            CurrentPos = (0, ExcelMetadata.Dimensions.columns[0]);
            OnCurrentWorksheetChanged();
        }
        public void CreateNewWorksheet(string worksheet)
        {
            Worksheets.Add(worksheet);
            OnNewWorksheetCreated();
        }

        protected abstract void OnCurrentWorksheetChanged();
        protected abstract void OnNewWorksheetCreated();
        protected abstract void OnMetadataSet();
        public abstract void Close();
    }
}
