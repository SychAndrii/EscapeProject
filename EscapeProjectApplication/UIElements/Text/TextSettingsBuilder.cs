namespace EscapeProjectApplication.UIElements.Text
{
    public class TextSettingsBuilder
    {
        private string text;
        private float fontSize;
        private TextFont font;
        private TextWeight fontWeight;
        private TextStyle fontStyle;

        public TextSettingsBuilder(string text)
        {
            this.text = text;
        }

        public TextSettingsBuilder WithText(string text)
        {
            this.text = text ?? string.Empty;
            return this;
        }

        public TextSettingsBuilder WithFont(TextFont font)
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

        public TextSettingsBuilder WithFontStyle(TextStyle fontStyle)
        {
            this.fontStyle = fontStyle;
            return this;
        }

        public TextSettings Build()
        {
            TextSettings ts = new TextSettings
            {
                Text = text,
                FontWeight = fontWeight,
                FontStyle = fontStyle,
                Font = font
            };
            if (fontSize != 0f)
            {
                ts.FontSize = fontSize;
            }
            return ts;
        }
    }
}
