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
using UIApplication.PDF;
using UIDomain.Checkbox;
using UIDomain.Text;

namespace UIInfrastructure.PDF
{
    public class ITextPDFService : PDFService
    {
#nullable disable
        private Document document;
        private PdfPage currentPage;
#nullable enable

        public ITextPDFService(PDFMetadataBuilder metadataBuilder)
            : base(metadataBuilder)
        {
            if (document == null || currentPage == null)
            {
                throw new Exception("Document and PdfPage objects were not set by the end of ITextPDFService creation");
            }
        }

        /* Render methods */
        public override void RenderCheckbox(CheckboxSettingsBuilder settingsBuilder)
        {
            if (document == null || currentPage == null)
            {
                throw new InvalidOperationException("Document is not initialized. Call Setup() first.");
            }

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

                float textCenter = CurrentPos.y + (checkboxSize / 2) - (textHeight / 2) + (descent * 2);

                (float x, float y) checkboxPos = CurrentPos;
                CurrentPos = (CurrentPos.x + settings.Size + 5, textCenter);
                RenderText(settings.TextSettingsBuilder);
                CurrentPos = checkboxPos;
            }
        }

        public override void RenderText(TextSettingsBuilder settingsBuilder)
        {
            if (document == null || currentPage == null)
            {
                throw new InvalidOperationException("Document is not initialized. Call Setup() first.");
            }

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

        /* IText helper methods */
        private PdfFont GetPdfFont(TextFont font)
        {
            return font == TextFont.HELVETICA
                ? PdfFontFactory.CreateFont(FontConstants.HELVETICA)
                : throw new Exception($"{font} does not have corresponding font in ITextUIText");
        }

        /* PDF document state manipulation methods */
        protected override void OnMetadataSet()
        {
            PdfWriter pdfWriter = new PdfWriter(PDFMetadata.Destination);
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);
            pdfDocument.SetDefaultPageSize(new PageSize(PDFMetadata.Dimensions.pageWidth, PDFMetadata.Dimensions.pageHeight));

            Document doc = new Document(pdfDocument);
            doc.SetMargins(PDFMetadata.Margins.vertical, PDFMetadata.Margins.horizontal, PDFMetadata.Margins.vertical, PDFMetadata.Margins.vertical);
            document = doc;
        }

        protected override void OnCurrentPageNumberChanged()
        {
            PdfDocument pdf = document.GetPdfDocument();
            currentPage = pdf.GetPage(CurrentPageNumber);
        }

        protected override void OnNewPageCreated()
        {
            PdfDocument pdf = document.GetPdfDocument();
            pdf.AddNewPage();
        }

        public override void Close()
        {
            document?.Close();
            document = null;
            currentPage = null;
        }
    }
}
