namespace UIApplication.Excel
{
    public abstract class ExcelServiceFactory
    {
        public abstract ExcelService Create(ExcelMetadataBuilder metadataBuilder);
    }
}
