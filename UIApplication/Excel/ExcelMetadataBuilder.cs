namespace UIApplication.Excel
{
    public class ExcelMetadataBuilder
    {
        private string? destination;
        private int? rows;
        private List<string>? columns;

        public ExcelMetadataBuilder WithDestination(string dest)
        {
            destination = dest;
            return this;
        }

        public ExcelMetadataBuilder WithRowsPerSheet(int rows)
        {
            this.rows = rows;
            return this;
        }

        public ExcelMetadataBuilder WithColumns(List<string> columns)
        {
            this.columns = columns;
            return this;
        }

        public ExcelMetadata Build()
        {
            return string.IsNullOrWhiteSpace(destination)
                ? throw new InvalidOperationException("Destination must be provided before building ExcelMetadata.")
                : rows == null || columns == null
                ? throw new InvalidOperationException("Both worksheet rows and columns must be set before building ExcelMetadata.")
                : new ExcelMetadata(
                destination!,
                rows.Value,
                columns
            );
        }
    }
}
