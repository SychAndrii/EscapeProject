using UIApplication.Excel;

namespace UIInfrastructure.Excel
{
    public class OpenXMLExcelServiceFactory : ExcelServiceFactory
    {
        public override ExcelService Create(ExcelMetadataBuilder metadataBuilder)
        {
            return new OpenXMLExcelService(metadataBuilder);
        }
    }
}
