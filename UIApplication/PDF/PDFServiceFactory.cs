namespace UIApplication.PDF
{
    public abstract class PDFServiceFactory
    {
        public abstract PDFService Create(PDFMetadataBuilder metadataBuilder);
    }
}
