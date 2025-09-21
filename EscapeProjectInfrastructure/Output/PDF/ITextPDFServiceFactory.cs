using EscapeProjectApplication.Output.PDF;

namespace EscapeProjectInfrastructure.Output.PDF
{
    public class ITextPDFServiceFactory : PDFServiceFactory
    {
        public override PDFService Create(PDFMetadataBuilder metadataBuilder)
        {
            return new ITextPDFService(metadataBuilder);
        }
    }
}
