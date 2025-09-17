using iText.Kernel.Pdf;
using iText.Layout;

namespace EscapeProject
{
    public class DocumentIterator
    {
        private float currentY;
        private float endY;
        private int pageNumber;
        private Document document;
        public DocumentIterator(Document document)
        {
            this.document = document;
            GoToNextPage();
        }

        public void GoToNextPage()
        {
            pageNumber++;
            if (!CanWrite())
            {
                return;
            }

            var topMargin = document.GetTopMargin();
            var bottomMargin = document.GetBottomMargin();

            PdfDocument pdf = document.GetPdfDocument();
            var page = pdf.GetPage(pageNumber);
            var pageHeight = page.GetPageSize().GetHeight();

            currentY = topMargin;
            endY = pageHeight - bottomMargin;
        }

        public bool HasVerticalSpace(float height)
        {
            return currentY + height < endY;
        }

        public void ReserveVerticalSpace(float height)
        {
            currentY += height;
        }

        public bool CanWrite()
        {
            PdfDocument pdf = document.GetPdfDocument();
            return pageNumber <= pdf.GetNumberOfPages();
        }

        public PageContentBuilder GetCurrentPage()
        {
            PageContentBuilder pageContent = new PageContentBuilder(document, pageNumber);
            return pageContent;
        }
    }
}
