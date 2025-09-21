namespace EscapeProjectApplication.Output.PDF
{
    public abstract class PDFServiceFactory
    {
        protected float marginX;
        protected float marginY;
        protected float pageWidth;
        protected float pageHeight;

        public PDFServiceFactory WithMargins(float horizontalMargin, float verticalMargin)
        {
            marginX = horizontalMargin;
            marginY = verticalMargin;
            return this;
        }

        public PDFServiceFactory WithPageSize(float width, float height)
        {
            pageWidth = width;
            pageHeight = height;
            return this;
        }

        public abstract PDFService Build();
    }
}
