using iText.Kernel.Font;

namespace EscapeProject.Text
{
    public class TextSettingsBuilder
    {
        private string text;
        private float fontSize;
        private PdfFont? font;
        private TextWeight fontWeight;

        public TextSettingsBuilder(string text)
        {
            this.text = text;
        }

        public TextSettingsBuilder WithText(string text)
        {
            this.text = text ?? string.Empty;
            return this;
        }

        public TextSettingsBuilder WithFont(PdfFont font)
        {
            this.font = font;
            return this;
        }

        public TextSettingsBuilder WithFontSize(float fontSize)
        {
            this.fontSize = fontSize;
            return this;
        }

        public TextSettingsBuilder WithFontWeight(TextWeight fontWeight)
        {
            this.fontWeight = fontWeight;
            return this;
        }

        internal TextSettings Build()
        {
            TextSettings ts = new TextSettings
            {
                Text = text,
                FontWeight = fontWeight
            };
            if (fontSize != 0f)
            {
                ts.FontSize = fontSize;
            }
            if (font != null)
            {
                ts.Font = font;
            }
            return ts;
        }
    }
}
