using EscapeProjectDomain;

namespace EscapeProjectApplication.Services
{
    public interface RenderService
    {
        void RenderTaskPlan(List<TaskGroupAggregate> taskGroups);
    }
}
