using EscapeProjectApplication.Output.PDF;
using EscapeProjectApplication.Task;
using EscapeProjectDomain;
using EscapeProjectInfrastructure.Output.PDF;
using EscapeProjectInfrastructure.Task;
using EscapeProjectPresentationCLI;

namespace EscapeProjectComposition
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            PDFServiceFactory pdfServiceFactory = new ITextPDFServiceFactory();
            TaskGroupRepository taskGroupRepository = new JSONTaskGroupRepository("Task/tasks.json");

            GenerateTaskPlanPDFUseCase useCase = new GenerateTaskPlanPDFUseCase(taskGroupRepository, pdfServiceFactory);
            TasksController tasksController = new TasksController(useCase);
            await tasksController.GenerateTaskPlanPDF();
        }
    }
}
