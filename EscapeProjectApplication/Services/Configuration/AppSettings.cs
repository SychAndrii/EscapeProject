namespace EscapeProjectApplication.Services.Configuration
{
    public class AppSettings
    {
        public required string BaseDirectoryAbsolutePath
        {
            get; set;
        }
        public required string TasksFilePath
        {
            get; set;
        }
        public required string TaskPlansDirectoryPath
        {
            get; set;
        }
    }
}
