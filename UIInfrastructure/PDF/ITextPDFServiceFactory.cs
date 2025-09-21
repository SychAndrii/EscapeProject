using UIApplication.PDF;

namespace UIInfrastructure.PDF
{
    public class ITextPDFServiceFactory : PDFServiceFactory
    {
        public override PDFService Create(PDFMetadataBuilder metadataBuilder)
        {
            return new ITextPDFService(metadataBuilder);
        }
    }
}
