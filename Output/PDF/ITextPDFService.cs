using EscapeProjectApplication.Output.PDF;
using EscapeProjectApplication.UIElements.Checkbox;
using EscapeProjectApplication.UIElements.Text;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;


namespace EscapeProjectInfrastructure.Output.PDF
{
    public class ITextPDFService : PDFService
    {
        private Document document;
        private PdfPage currentPage;
        public ITextPDFService(float marginX, float marginY, float pageWidth, float pageHeight) : base(marginX, marginY, pageWidth, pageHeight)
        {
        }

        public override void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder)
        {
            var settings = settingsBuilder.Build();
            PdfDocument pdf = document.GetPdfDocument();
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);

            Rectangle rect = new Rectangle(CurrentPos.x, CurrentPos.y, settings.Size, settings.Size);
            PdfButtonFormField checkbox = PdfFormField.CreateCheckBox(
                pdf,
                rect,
                Guid.NewGuid().ToString(),
                settings.DefaultValue == CheckboxState.ON ? "On" : "Off",
                PdfFormField.TYPE_CHECK
            );

            checkbox.SetPage(pdf.GetPageNumber(currentPage));
            form.AddField(checkbox);

            if (settings.TextSettingsBuilder != null)
            {
                TextSettings textSettings = settings.TextSettingsBuilder.Build();
                float fontSize = textSettings.FontSize;
                float checkboxSize = settings.Size;

                PdfFont font = GetPdfFont(textSettings.Font);
                float ascent = font.GetAscent("A", fontSize);
                float descent = font.GetDescent("g", fontSize);
                float textHeight = ascent - descent;

                // Move the label slightly down to visually center it with checkbox
                float textCenter = CurrentPos.y + (checkboxSize / 2) - (textHeight / 2) + (descent * 2);

                (float x, float y, int pageNumber) checkboxPos = CurrentPos;
                CurrentPos = (CurrentPos.x + settings.Size + 5, textCenter, CurrentPos.pageNumber);
                RenderText(settings.TextSettingsBuilder);
                CurrentPos = checkboxPos;
            }
        }

        public override void RenderText(TextSettingsBuilder settingsBuilder)
        {
            var settings = settingsBuilder.Build();
            if (!string.IsNullOrWhiteSpace(settings.Text))
            {
                PdfDocument pdf = document.GetPdfDocument();
                Canvas layoutCanvas = new Canvas(new PdfCanvas(currentPage), pdf, currentPage.GetPageSize());

                PdfFont font = GetPdfFont(settings.Font);
                float textWidth = font.GetWidth(settings.Text, settings.FontSize);

                Paragraph par = new Paragraph(settings.Text)
                    .SetFont(font)
                    .SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.FILL_STROKE)
                    .SetStrokeWidth(settings.FontWeight == TextWeight.BOLD ? .5f : 0f)
                    .SetStrokeColor(DeviceGray.BLACK)
                    .SetFontSize(settings.FontSize)
                    .SetFixedPosition(CurrentPos.x, CurrentPos.y, textWidth + 10);

                if (settings.FontStyle == TextStyle.ITALIC)
                {
                    par.SetItalic();
                }

                layoutCanvas.Add(par);
            }
        }

        public override void Setup(string fileDestination)
        {
            PdfWriter pdfWriter = new PdfWriter(fileDestination);
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);
            pdfDocument.SetDefaultPageSize(new PageSize(Dimensions.pageWidth, Dimensions.pageHeight));
            Document document = new Document(pdfDocument);
            document.SetMargins(Margins.vertical, Margins.horizontal, Margins.vertical, Margins.vertical);
            this.document = document;
        }

        public override int CreateNewPage()
        {
            PdfDocument pdf = document.GetPdfDocument();
            PdfPage newPage = pdf.AddNewPage();
            return pdf.GetPageNumber(newPage);
        }

        private PdfFont GetPdfFont(TextFont font)
        {
            return font == TextFont.HELVETICA
                ? PdfFontFactory.CreateFont(FontConstants.HELVETICA)
                : throw new Exception($"{font} does not have corresponding font in ITextUIText");
            ;
        }
    }
}
