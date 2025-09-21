using EscapeProjectApplication.Output.PDF;

namespace EscapeProjectInfrastructure.Output.PDF
{
    public class ITextPDFServiceFactory : PDFServiceFactory
    {
        public override PDFService Build()
        {
            return new ITextPDFService(marginX, marginY, pageWidth, pageHeight);
        }
    }
}
