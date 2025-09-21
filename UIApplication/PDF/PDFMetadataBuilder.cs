namespace UIApplication.PDF
{
    public class PDFMetadataBuilder
    {
        private string? destination;
        private float? horizontalMargin;
        private float? verticalMargin;
        private float? pageWidth;
        private float? pageHeight;

        public PDFMetadataBuilder WithDestination(string dest)
        {
            destination = dest;
            return this;
        }

        public PDFMetadataBuilder WithMargins(float horizontal, float vertical)
        {
            horizontalMargin = horizontal;
            verticalMargin = vertical;
            return this;
        }

        public PDFMetadataBuilder WithDimensions(float width, float height)
        {
            pageWidth = width;
            pageHeight = height;
            return this;
        }

        public PDFMetadata Build()
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new InvalidOperationException("Destination must be provided before building PDFMetadata.");
            }
            return horizontalMargin == null || verticalMargin == null
                ? throw new InvalidOperationException("Both horizontal and vertical margins must be set before building PDFMetadata.")
                : pageWidth == null || pageHeight == null
                ? throw new InvalidOperationException("Both page width and page height must be set before building PDFMetadata.")
                : new PDFMetadata(
                destination!,
                horizontalMargin.Value,
                verticalMargin.Value,
                pageWidth.Value,
                pageHeight.Value
            );
        }
    }
}
