using BaseDomain;
using EscapeProjectDomain;

namespace EscapeProjectInfrastructure.Task
{
    public static class TaskEntityMapper
    {
        internal static TaskEntity ToDomain(TaskEntityDto dto) =>
            new TaskEntity(
                new NormalizedString(dto.Name),
                dto.From,
                dto.Until,
                dto.Subtasks?.Select(ToDomain)
            );

        internal static TaskEntityDto ToDto(TaskEntity entity) =>
            new TaskEntityDto
            {
                Name = entity.Name.ToString(),
                From = entity.From,
                Until = entity.Until,
                Subtasks = entity.Subtasks?.Select(ToDto).ToList()
            };
    }
}
