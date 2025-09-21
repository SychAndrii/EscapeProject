using BaseDomain;

namespace EscapeProjectDomain
{
    public interface TaskGroupRepository : Repository
    {
        Task<List<TaskGroupAggregate>> GetTaskGroups();
    }
}
