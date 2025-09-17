using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;

namespace EscapeProject
{
    public class DocumentBuilder
    {
        private PageSize pageSize = PageSize.A4;
        private PdfDocument pdf;
        private float marginX = 20;
        private float marginY = 20;
        private readonly string pdfFileDestination;

        public DocumentBuilder(string pdfFileDestination)
        {
            this.pdfFileDestination = pdfFileDestination;
            Reset();
        }

        public DocumentBuilder Reset()
        {
            pageSize = PageSize.A4;
            PdfWriter pdfWriter = new PdfWriter(pdfFileDestination);
            pdf = new PdfDocument(pdfWriter);
            return this;
        }

        public DocumentBuilder SetPageSize(PageSize pageSize)
        {
            this.pageSize = pageSize;
            return this;
        }

        public DocumentBuilder AddPages(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                pdf.AddNewPage(pageSize);
            }
            return this;
        }

        public DocumentBuilder SetMarginX(float marginX)
        {
            this.marginY = marginX;
            return this;
        }

        public DocumentBuilder SetMarginY(float marginY)
        {
            this.marginY = marginY;
            return this;
        }

        public Document BuildDocument()
        {
            var document = new Document(pdf);
            document.SetMargins(marginY, marginX, marginY, marginX);
            return document;
        }
    }
}
