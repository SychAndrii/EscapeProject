using EscapeProjectApplication.Services.Configuration;
using Microsoft.Extensions.Configuration;

namespace EscapeProjectInfrastructure.Configuration
{
    public class JSONConfigurationService : ConfigurationService
    {
        public JSONConfigurationService()
        {
            string basePath = ReadBaseDirectoryFromEnvVariable();

            // Build full path to the config file
            string jsonConfigPath = Path.Combine(basePath, "tasks.json");

            var config = new ConfigurationBuilder()
                .AddJsonFile(jsonConfigPath, optional: true, reloadOnChange: true)
                .Build();

            var raw = config.Get<AppSettings>() ?? throw new Exception("Config failed to load");

            // Normalize to absolute paths
            string baseDir = Path.GetDirectoryName(Path.GetFullPath(jsonConfigPath))!;

            Settings = new AppSettings
            {
                BaseDirectoryAbsolutePath = baseDir,
                TasksFilePath = Path.GetFullPath(Path.Combine(baseDir, raw.TasksFilePath)),
                TaskPlansDirectoryPath = Path.GetFullPath(Path.Combine(baseDir, raw.TaskPlansDirectoryPath))
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
