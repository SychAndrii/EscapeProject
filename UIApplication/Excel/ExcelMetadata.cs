namespace UIApplication.Excel
{
    public class ExcelMetadata
    {
        private List<string> columns;

        public string Destination
        {
            get;
        }

        public List<string> Columns
        {
            get => columns;
            set
            {
                if (value.Count() <= 0)
                {
                    throw new Exception($"Number of columns for Excel worksheet must be > 0");
                }
                if (value.ToHashSet().Count() != value.Count)
                {
                    throw new Exception($"Columns in Excel worksheet must be unique");
                }
                columns = value;
            }
        }

        public ExcelMetadata(string destination, List<string> columns)
        {
            Destination = destination;
            Columns = columns;
        }
    }
}
