using System.Text.Json;

namespace EscapeProject.Task
{
    public class TaskRepository
    {
        public List<TaskGroupEntity> GetTaskGroups()
        {
            Dictionary<string, string[]> raw = JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText("Task/tasks.json"));

            List<TaskGroupEntity> taskGroups = [];

            foreach (var kvp in raw)
            {
                taskGroups.Add(new TaskGroupEntity
                {
                    name = kvp.Key,
                    tasks = kvp.Value.Select(taskName => new TaskEntity { name = taskName }).ToArray()
                });
            }

            return taskGroups;
        }
    }
}
