using UIDomain.Checkbox;
using UIDomain.Text;

namespace UIApplication.PDF
{
    public abstract class PDFService
    {
        /* PDF document state */
        public PDFMetadata PDFMetadata
        {
            get;
        }

        private int currentPageNumber;
        public int CurrentPageNumber
        {
            get => currentPageNumber;
            private set
            {
                if (value <= 0 || value > TotalPages)
                {
                    throw new Exception($"Current page number is out of range of PDF pages");
                }
                currentPageNumber = value;
            }
        }

        private int totalPages;
        public int TotalPages
        {
            get => totalPages;
            private set
            {
                if (value <= 0)
                {
                    throw new Exception($"Total page amount of PDF cannot be <= 0");
                }
                totalPages = value;
            }
        }

        private (float x, float y) currentPos;

        public (float x, float y) CurrentPos
        {
            get => currentPos;
            set
            {
                if (value.x < 0 || value.x > PDFMetadata.Dimensions.pageWidth)
                {
                    throw new Exception("Page X coordinate in PDF document is out of bounds");
                }
                if (value.y < 0 || value.y > PDFMetadata.Dimensions.pageHeight)
                {
                    throw new Exception("Page Y coordinate in PDF document is out of bounds");
                }
                currentPos = value;
            }
        }

        protected PDFService(PDFMetadataBuilder metadataBuilder)
        {
            PDFMetadata = metadataBuilder.Build();
            OnMetadataSet();
            CreateNewPage();
            GoToPage(totalPages);
        }

        /* Render methods */
        public abstract void RenderText(TextSettingsBuilder settingsBuilder);
        public abstract void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder);

        /* PDF document state manipulation methods */
        public void GoToPage(int pageNumber)
        {
            CurrentPageNumber = pageNumber;
            CurrentPos = (0, 0);
            OnCurrentPageNumberChanged();
        }
        public void CreateNewPage()
        {
            TotalPages++;
            OnNewPageCreated();
        }

        protected abstract void OnCurrentPageNumberChanged();
        protected abstract void OnNewPageCreated();
        protected abstract void OnMetadataSet();
        public abstract void Close();
    }
}
