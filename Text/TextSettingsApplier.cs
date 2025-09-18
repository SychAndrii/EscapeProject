using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace EscapeProject.Text
{
    internal class TextSettingsApplier
    {
        private readonly TextSettings textSettings;

        internal TextSettingsApplier(TextSettings textSettings)
        {
            this.textSettings = textSettings;
        }

        internal void Apply(Canvas layoutCanvas, float x, float y)
        {
            if (!string.IsNullOrWhiteSpace(textSettings.Text))
            {
                Paragraph par = new Paragraph(textSettings.Text)
                    .SetFont(textSettings.Font)
                    .SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.FILL_STROKE)
                    .SetStrokeWidth(textSettings.FontWeight == TextWeight.BOLD ? .5f : 0f)
                    .SetStrokeColor(DeviceGray.BLACK)
                    .SetFontSize(textSettings.FontSize)
                    .SetFixedPosition(x, y, textSettings.TextWidth + 10);

                if (textSettings.FontStyle == TextStyle.ITALIC)
                {
                    par.SetItalic();
                }

                layoutCanvas.Add(par);
            }
        }
    }
}
