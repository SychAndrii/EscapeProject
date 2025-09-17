using EscapeProject.Checkbox;
using EscapeProject.Text;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;

namespace EscapeProject
{
    public class PageContentBuilder
    {
        private readonly PdfPage page;
        private readonly int pageNumber;
        private readonly Canvas layoutCanvas;
        private readonly PdfAcroForm form;
        public PageContentBuilder(Document document, int pageNumber)
        {
            var pdfDocument = document.GetPdfDocument();
            page = pdfDocument.GetPage(pageNumber);
            this.pageNumber = pageNumber;
            layoutCanvas = new Canvas(new PdfCanvas(page), pdfDocument, page.GetPageSize());
            form = PdfAcroForm.GetAcroForm(pdfDocument, true);
            form.SetGenerateAppearance(true);
        }

        public PageContentBuilder AddText(float x, float y, TextSettingsBuilder textBuilder)
        {
            var settings = textBuilder.Build();
            new TextSettingsApplier(textBuilder.Build()).Apply(layoutCanvas, x, y);
            return this;
        }

        public PageContentBuilder AddCheckbox(float x, float y, CheckboxSettingsBuilder checkboxBuilder)
        {
            var pdf = page.GetDocument();
            new CheckboxSettingsApplier(checkboxBuilder.Build()).Apply(pdf, form, layoutCanvas, x, y, pageNumber);
            return this;
        }
    }
}
