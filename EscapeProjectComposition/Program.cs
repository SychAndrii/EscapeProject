using EscapeProjectApplication.Services;
using EscapeProjectApplication.UseCases;
using EscapeProjectDomain;
using EscapeProjectInfrastructure.Render;
using EscapeProjectInfrastructure.Task;
using EscapeProjectPresentationCLI;
using UIApplication.Excel;
using UIApplication.PDF;
using UIInfrastructure.Excel;
using UIInfrastructure.PDF;

namespace EscapeProjectComposition
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            string baseDir = AppContext.BaseDirectory;
            string tasksJSONPath = Path.Combine(baseDir, "Task", "tasks.json");
            TaskGroupRepository taskGroupRepository = new JSONTaskGroupRepository(tasksJSONPath);

            PDFServiceFactory pdfServiceFactory = new ITextPDFServiceFactory();
            RenderService renderServicePDF = new PDFRenderService(pdfServiceFactory);
            GenerateTaskPlanUseCase useCasePDF = new GenerateTaskPlanUseCase(taskGroupRepository, renderServicePDF);
            TasksController tasksControllerPDF = new TasksController(useCasePDF);
            await tasksControllerPDF.GenerateTaskPlan();

            ExcelServiceFactory excelServiceFactory = new ClosedXMLExcelServiceFactory();
            RenderService renderServiceExcel = new ExcelRenderService(excelServiceFactory);
            GenerateTaskPlanUseCase useCaseExcel = new GenerateTaskPlanUseCase(taskGroupRepository, renderServiceExcel);
            TasksController tasksControllerExcel = new TasksController(useCaseExcel);
            await tasksControllerExcel.GenerateTaskPlan();
        }
    }
}
