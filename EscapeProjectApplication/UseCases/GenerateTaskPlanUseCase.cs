using EscapeProjectApplication.Services;
using EscapeProjectDomain;

namespace EscapeProjectApplication.UseCases
{
    public class GenerateTaskPlanUseCase
    {
        private readonly TaskGroupRepository taskGroupRepository;
        private readonly RenderService renderService;

        public GenerateTaskPlanUseCase(TaskGroupRepository taskGroupRepository, RenderService renderService)
        {
            this.taskGroupRepository = taskGroupRepository;
            this.renderService = renderService;
        }

        public async ValueTask Execute()
        {
            List<TaskGroupAggregate> taskGroups = await taskGroupRepository.GetTaskGroups();
            renderService.RenderTaskPlan(taskGroups);
        }
    }
}
