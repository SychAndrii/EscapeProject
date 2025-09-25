using System.Text.Json;
using EscapeProject.Task;
using EscapeProjectApplication.Services.Configuration;
using EscapeProjectDomain;

namespace EscapeProjectInfrastructure.Task
{
    public class JSONTaskGroupRepository : TaskGroupRepository
    {
        private readonly ConfigurationService configService;

        public JSONTaskGroupRepository(ConfigurationService configService)
        {
            this.configService = configService;
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

            var dtoGroups = JsonSerializer.Deserialize<Dictionary<string, List<TaskEntityDto>>>(json, options);

            if (dtoGroups == null)
            {
                throw new InvalidDataException("Failed to parse JSON task groups.");
            }

            var result = new List<TaskGroupAggregate>();
            foreach (var (key, taskDtos) in dtoGroups)
            {
                var tasks = taskDtos.Select(TaskEntityMapper.ToDomain).ToList();
                result.Add(new TaskGroupAggregate(key, tasks));
            }

            return result;
        }
    }
}
