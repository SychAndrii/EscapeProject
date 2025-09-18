using iText.IO.Font;
using iText.Kernel.Font;

namespace EscapeProject.Text
{
    internal class TextSettings
    {
        private float fontSize = 12;

        internal string? Text
        {
            get;
            set;
        }
        internal float FontSize
        {
            get => fontSize;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException($"{nameof(FontSize)} cannot be less <= 0");
                }
                fontSize = value;
            }
        }
        internal float TextWidth => Font.GetWidth(Text, fontSize);
        internal PdfFont Font { get; set; } = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
        internal TextWeight FontWeight { get; set; } = TextWeight.NORMAL;
        internal TextStyle FontStyle { get; set; } = TextStyle.NORMAL;
    }
}
