using EscapeProjectApplication.UIElements.Checkbox;
using EscapeProjectApplication.UIElements.Text;

namespace EscapeProjectApplication.Output.PDF
{
    public abstract class PDFService
    {
        private const string DEST = "file.pdf";
        private (float pageWidth, float pageHeight, int pageAmount) dimensions;
        private (float x, float y, int pageNumber) currentPos;
        private (float horizontal, float vertical) margins;

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

        public (float pageWidth, float pageHeight, int pageAmount) Dimensions
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
                if (value.pageAmount <= 0)
                {
                    throw new Exception($"Number of pages for PDF page must be > 0");
                }
                dimensions = value;
            }
        }

        public (float x, float y, int pageNumber) CurrentPos
        {
            get => currentPos;
            set
            {
                if (value.x < 0 || value.x > dimensions.pageWidth)
                {
                    throw new Exception("Page X coordinate in PDF document is out of bounds");
                }
                if (value.y < 0 || value.y > dimensions.pageHeight)
                {
                    throw new Exception("Page Y coordinate in PDF document is out of bounds");
                }
                if (value.pageNumber <= 0 || value.pageNumber > Dimensions.pageAmount)
                {
                    throw new Exception("Page number coordinate in PDF document is out of bounds");
                }
                currentPos = value;
            }
        }

        protected PDFService(float marginX, float marginY, float pageWidth, float pageHeight)
        {
            Margins = (marginX, marginY);
            Dimensions = (pageWidth, pageHeight, 1);
            CurrentPos = (0, 0, 1);
            Setup(DEST);
        }

        public abstract void RenderText(TextSettingsBuilder settingsBuilder);
        public abstract void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder);
        public abstract int CreateNewPage(bool updateDimensions = true);
        public abstract void Setup(string fileDestination);
        public abstract void Close();
    }
}
