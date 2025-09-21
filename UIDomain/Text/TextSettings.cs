namespace UIDomain.Text
{
    public class TextSettings
    {
        private float fontSize = 12;

        public string? Text
        {
            get;
            set;
        }
        public float FontSize
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
        public TextFont Font { get; set; } = TextFont.HELVETICA;
        public TextWeight FontWeight { get; set; } = TextWeight.NORMAL;
        public TextStyle FontStyle { get; set; } = TextStyle.NORMAL;
    }
}
