namespace EscapeProjectInfrastructure.Task
{
    internal class TaskEntityDto
    {
        public string Name { get; set; } = "";
        public DateTime? From
        {
            get; set;
        }
        public DateTime? Until
        {
            get; set;
        }
        public List<TaskEntityDto>? Subtasks
        {
            get; set;
        }
    }
}
