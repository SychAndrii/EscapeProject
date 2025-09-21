namespace EscapeProjectApplication.Output.PDF
{
    public class PDFMetadata
    {
        private (float pageWidth, float pageHeight) dimensions;
        private (float horizontal, float vertical) margins;

        public string Destination
        {
            get;
        }

        public (float horizontal, float vertical) Margins
        {
            get => margins;
            private set
            {
                if (value.horizontal < 0)
                {
                    throw new Exception($"Horizontal margin for PDF file must be >= 0");
                }
                if (value.vertical < 0)
                {
                    throw new Exception($"Vertical margin for PDF file must be >= 0");
                }
                margins = value;
            }
        }

        public (float pageWidth, float pageHeight) Dimensions
        {
            get => dimensions;
            set
            {
                if (value.pageWidth <= 0)
                {
                    throw new Exception($"Width for PDF page must be > 0");
                }
                if (value.pageHeight <= 0)
                {
                    throw new Exception($"Height for PDF page must be > 0");
                }
                dimensions = value;
            }
        }

        public PDFMetadata(string destination, float horizontalMargin, float verticalMargin, float pageWidth, float pageHeight)
        {
            Destination = destination;
            Margins = (horizontalMargin, verticalMargin);
            Dimensions = (pageWidth, pageHeight);
        }
    }
}
