namespace UIApplication.Excel
{
    public class ExcelMetadataBuilder
    {
        private string? destination;
        private int? rows;
        private int? columns;

        public ExcelMetadataBuilder WithDestination(string dest)
        {
            destination = dest;
            return this;
        }

        public ExcelMetadataBuilder WithDimensions(int rows, int columns)
        {
            this.rows = rows;
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
                rows.Value
            );
        }
    }
}
