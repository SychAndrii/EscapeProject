using UIDomain.Checkbox;
using UIDomain.Text;

namespace UIApplication.Excel
{
    public abstract class ExcelService
    {
        public const string DEFAULT_WORKSHEET = "Sheet 1";

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
                currentWorksheet = value;
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
                if (value.row < 0)
                {
                    throw new Exception("Excel row coordinate in Excel document must be >= 0");
                }
                if (!ExcelMetadata.Columns.Contains(value.col))
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
            CreateNewWorksheet(DEFAULT_WORKSHEET);
            GoToWorksheet(DEFAULT_WORKSHEET);
        }

        /* Render methods */
        public abstract void RenderText(TextSettingsBuilder settingsBuilder);
        public abstract void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder);

        /* Excel document state manipulation methods */
        public void GoToWorksheet(string worksheet)
        {
            CurrentWorksheet = worksheet;
            CurrentPos = (0, ExcelMetadata.Columns[0]);
            OnCurrentWorksheetChanged();
        }
        public void CreateNewWorksheet(string worksheet)
        {
            Worksheets.Add(worksheet);
            OnNewWorksheetCreated(worksheet);
        }
        public void RemoveWorksheet(string worksheet)
        {
            bool containsWorksheet = Worksheets.Contains(worksheet);
            if (!containsWorksheet)
            {
                throw new Exception($"[{worksheet}] worksheet does not exist in Excel file");
            }
            if (containsWorksheet && Worksheets.Count == 1)
            {
                throw new Exception($"Cannot delete the only existing worksheet in Excel file");
            }
            Worksheets.Remove(worksheet);
            OnWorksheetDeleted(worksheet);
        }

        protected abstract void OnCurrentWorksheetChanged();
        protected abstract void OnNewWorksheetCreated(string worksheet);
        protected abstract void OnWorksheetDeleted(string worksheet);
        protected abstract void OnMetadataSet();
        public abstract void Close();
    }
}
