using EscapeProjectApplication.Services.Configuration;
using EscapeProjectPresentationCLI.Settings.GenerateTaskPlan;

namespace EscapeProjectPresentationCLI.Configuration
{
    public class SpectreConfigurationService : ConfigurationService
    {
        public SpectreConfigurationService(GenerateTaskPlanSettings spectreSettings)
        {
            string basePath = ReadBaseDirectoryFromEnvVariable();

            Settings = new AppSettings
            {
                BaseDirectoryAbsolutePath = basePath,
                TasksFilePath = Path.GetFullPath(Path.Combine(basePath, spectreSettings.TasksFilePath)),
                TaskPlansDirectoryPath = Path.GetFullPath(Path.Combine(basePath, spectreSettings.DirectoryPath))
            };
        }

        public AppSettings Settings
        {
            get;
        }

        private static string ReadBaseDirectoryFromEnvVariable()
        {
            string? baseDirectoryPath = Environment.GetEnvironmentVariable("ESCAPE_PROJECT_BASE_DIRECTORY");
            if (string.IsNullOrEmpty(baseDirectoryPath))
            {
                Console.Error.WriteLine("Environment variable 'ESCAPE_PROJECT_BASE_DIRECTORY' is not set.");
                Environment.Exit(1);
            }
            return baseDirectoryPath;
        }
    }
}
