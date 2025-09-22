using System.Text.Json;
using BaseDomain;
using EscapeProject.Task;
using EscapeProjectDomain;
using EscapeProjectInfrastructure.Configuration;

namespace EscapeProjectInfrastructure.Task
{
    public class JSONTaskGroupRepository : TaskGroupRepository
    {
        private readonly ConfigurationService configService;

        public JSONTaskGroupRepository(ConfigurationService configService)
        {
            this.configService = configService;
        }

        public IUnitOfWork UnitOfWork
        {
            get;
        }

        public async Task<List<TaskGroupAggregate>> GetTaskGroups()
        {
            string jsonFile = configService.Settings.TasksFilePath;
            if (!File.Exists(jsonFile))
            {
                throw new FileNotFoundException($"File not found: {jsonFile}");
            }

            string json = await File.ReadAllTextAsync(jsonFile);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new StrictDateTimeConverter(), new NormalizedStringJsonConverter() }
            };

            // Deserialize to Dictionary<string, List<TaskEntity>>
            Dictionary<string, List<TaskEntity>>? rawGroups =
                JsonSerializer.Deserialize<Dictionary<string, List<TaskEntity>>>(json, options);

            if (rawGroups == null)
            {
                throw new InvalidDataException("Failed to parse JSON task groups.");
            }

            // Convert to List<TaskGroupAggregate>
            var result = new List<TaskGroupAggregate>();
            foreach (var (key, taskList) in rawGroups)
            {
                result.Add(new TaskGroupAggregate(key, taskList));
            }

            return result;
        }
    }
}
