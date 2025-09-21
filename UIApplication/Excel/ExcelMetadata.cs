namespace UIApplication.Excel
{
    public class ExcelMetadata
    {
        private (int rows, List<string> columns) dimensions;

        public string Destination
        {
            get;
        }

        public (int rows, List<string> columns) Dimensions
        {
            get => dimensions;
            set
            {
                if (value.rows <= 0)
                {
                    throw new Exception($"Number of rows for Excel worksheet must be > 0");
                }
                if (value.columns.Count() <= 0)
                {
                    throw new Exception($"Number of columns for Excel worksheet must be > 0");
                }
                if (value.columns.ToHashSet().Count() != value.columns.Count)
                {
                    throw new Exception($"Columns in Excel worksheet must be unique");
                }
                dimensions = value;
            }
        }

        public ExcelMetadata(string destination, int rows, List<string> columns)
        {
            Destination = destination;
            Dimensions = (rows, columns);
        }
    }
}
