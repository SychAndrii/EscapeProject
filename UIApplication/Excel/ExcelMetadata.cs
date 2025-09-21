namespace UIApplication.Excel
{
    public class ExcelMetadata
    {
        private (int rows, string[] columns) dimensions;

        public string Destination
        {
            get;
        }

        public (int rows, string[] columns) Dimensions
        {
            get => dimensions;
            set
            {
                if (value.rows <= 0)
                {
                    throw new Exception($"Number of rows for Excel worksheet must be > 0");
                }
                if (value.columns.Length <= 0)
                {
                    throw new Exception($"Number of columns for Excel worksheet must be > 0");
                }
                dimensions = value;
            }
        }

        public ExcelMetadata(string destination, int rows, string[] columns)
        {
            Destination = destination;
            Dimensions = (rows, columns);
        }
    }
}
