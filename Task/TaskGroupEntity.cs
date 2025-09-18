namespace EscapeProject.Task
{
    public class TaskGroupEntity
    {
        public string name { get; set; } = string.Empty;
        public TaskEntity[] tasks { get; set; } = [];
    }
}
