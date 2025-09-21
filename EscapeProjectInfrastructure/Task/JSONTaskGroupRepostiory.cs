using System.Text.Json;
using BaseDomain;
using EscapeProject.Task;
using EscapeProjectDomain;

namespace EscapeProjectInfrastructure.Task
{
    public class JSONTaskGroupRepository : TaskGroupRepository
    {
        private readonly string filePath;

        public JSONTaskGroupRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public IUnitOfWork UnitOfWork
        {
            get;
        }

        public async Task<List<TaskGroupAggregate>> GetTaskGroups()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            string json = await File.ReadAllTextAsync(filePath);

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
