using EscapeProjectApplication.Task;

namespace EscapeProjectPresentationCLI
{
    public class TasksController
    {
        private readonly GenerateTaskPlanPDFUseCase taskPlanUseCase;

        public TasksController(GenerateTaskPlanPDFUseCase taskPlanUseCase)
        {
            this.taskPlanUseCase = taskPlanUseCase;
        }

        public async Task GenerateTaskPlanPDF()
        {
            await taskPlanUseCase.GeneratePlanPDF();
        }
    }
}
