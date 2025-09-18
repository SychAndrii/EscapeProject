using System.Text.Json;

namespace EscapeProject.Task
{
    public class TaskRepository
    {
        public List<TaskGroupEntity> GetTaskGroups()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var raw = JsonSerializer.Deserialize<Dictionary<string, TaskEntity[]>>(
                File.ReadAllText("Task/tasks.json"),
                options
            );

            return raw == null
                ? []
                : raw.Select(kvp => new TaskGroupEntity
                {
                    name = kvp.Key,
                    tasks = kvp.Value
                }).ToList();
        }
    }
}
