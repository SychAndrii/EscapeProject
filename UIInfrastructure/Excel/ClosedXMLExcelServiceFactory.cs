using UIApplication.Excel;

namespace UIInfrastructure.Excel
{
    public class ClosedXMLExcelServiceFactory : ExcelServiceFactory
    {
        public override ExcelService Create(ExcelMetadataBuilder metadataBuilder)
        {
            return new ClosedXMLExcelService(metadataBuilder);
        }
    }
}
