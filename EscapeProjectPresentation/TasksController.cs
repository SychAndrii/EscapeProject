using EscapeProjectApplication.UseCases;

namespace EscapeProjectPresentationCLI
{
    public class TasksController
    {
        private readonly GenerateTaskPlanUseCase taskPlanUseCase;

        public TasksController(GenerateTaskPlanUseCase taskPlanUseCase)
        {
            this.taskPlanUseCase = taskPlanUseCase;
        }

        public async Task GenerateTaskPlan()
        {
            await taskPlanUseCase.Execute();
        }
    }
}
