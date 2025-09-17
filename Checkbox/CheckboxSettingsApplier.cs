using EscapeProject.Text;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;

namespace EscapeProject.Checkbox
{
    internal class CheckboxSettingsApplier
    {
        private readonly CheckboxSettings checkboxSettings;

        internal CheckboxSettingsApplier(CheckboxSettings checkboxSettings)
        {
            this.checkboxSettings = checkboxSettings;
        }

        internal void Apply(PdfDocument pdf, PdfAcroForm form, Canvas layoutCanvas, float x, float y, int pageNum)
        {
            Rectangle rect = new Rectangle(x, y, checkboxSettings.Size, checkboxSettings.Size);
            PdfButtonFormField checkbox = PdfFormField.CreateCheckBox(
                pdf,
                rect,
                Guid.NewGuid().ToString(),
                checkboxSettings.DefaultValue == CheckboxState.ON ? "On" : "Off",
                PdfFormField.TYPE_CHECK
            );

            checkbox.SetPage(pageNum);
            form.AddField(checkbox);

            if (checkboxSettings.Text != null)
            {
                float fontSize = checkboxSettings.Text.FontSize;
                float checkboxSize = checkboxSettings.Size;

                PdfFont font = checkboxSettings.Text.Font;
                float ascent = font.GetAscent("A", fontSize);
                float descent = font.GetDescent("g", fontSize);
                float textHeight = ascent - descent;

                // Move the label slightly down to visually center it with checkbox
                float textCenter = y + (checkboxSize / 2) - (textHeight / 2) + (descent * 2);

                new TextSettingsApplier(checkboxSettings.Text)
                    .Apply(layoutCanvas, x + checkboxSettings.Size + 5, textCenter);
            }
        }
    }
}
