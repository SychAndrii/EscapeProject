using EscapeProjectApplication.Services;
using EscapeProjectApplication.UseCases;
using EscapeProjectDomain;
using EscapeProjectInfrastructure.Render;
using EscapeProjectInfrastructure.Task;
using EscapeProjectPresentationCLI;
using UIApplication.PDF;
using UIInfrastructure.PDF;

namespace EscapeProjectComposition
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            PDFServiceFactory pdfServiceFactory = new ITextPDFServiceFactory();
            TaskGroupRepository taskGroupRepository = new JSONTaskGroupRepository("Task/tasks.json");
            RenderService renderService = new PDFRenderService(pdfServiceFactory);

            GenerateTaskPlanPDFUseCase useCase = new GenerateTaskPlanPDFUseCase(taskGroupRepository, renderService);
            TasksController tasksController = new TasksController(useCase);
            await tasksController.GenerateTaskPlanPDF();
        }
    }
}
