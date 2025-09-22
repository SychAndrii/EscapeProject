using Microsoft.Extensions.Configuration;

namespace EscapeProjectInfrastructure.Configuration
{
    public class JSONConfigurationService : ConfigurationService
    {
        public JSONConfigurationService(string jsonConfigPath)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(jsonConfigPath, optional: true, reloadOnChange: true)
                .Build();

            var raw = config.Get<AppSettings>() ?? throw new Exception("Config failed to load");

            // Normalize to absolute paths
            string baseDir = Path.GetDirectoryName(Path.GetFullPath(jsonConfigPath))!;

            Settings = new AppSettings
            {
                TasksFilePath = Path.GetFullPath(Path.Combine(baseDir, raw.TasksFilePath)),
                TaskPlansDirectoryPath = Path.GetFullPath(Path.Combine(baseDir, raw.TaskPlansDirectoryPath))
            };
        }

        public AppSettings Settings
        {
            get;
        }
    }
}
