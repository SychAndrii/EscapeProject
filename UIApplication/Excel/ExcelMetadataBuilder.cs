using UIDomain.Text;

namespace UIApplication.Excel
{
    public class ExcelMetadataBuilder
    {
        private string? destination;
        private List<TextSettingsBuilder>? columns;

        public ExcelMetadataBuilder WithDestination(string dest)
        {
            destination = dest;
            return this;
        }

        public ExcelMetadataBuilder WithColumns(List<TextSettingsBuilder> columns)
        {
            this.columns = columns;
            return this;
        }

        public ExcelMetadata Build()
        {
            return string.IsNullOrWhiteSpace(destination)
                ? throw new InvalidOperationException("Destination must be provided before building ExcelMetadata.")
                : columns == null
                ? throw new InvalidOperationException("Worksheet columns must be set before building ExcelMetadata.")
                : new ExcelMetadata(
                destination!,
                columns
            );
        }
    }
}
