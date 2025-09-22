using EscapeProjectApplication.Services;
using EscapeProjectApplication.UseCases;
using EscapeProjectDomain;
using EscapeProjectInfrastructure.Configuration;
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
            string configPath = ReadConfigPathFromEnvVariable();

            var configService = new JSONConfigurationService(configPath);
            TaskGroupRepository taskGroupRepository = new JSONTaskGroupRepository(configService);

            await GeneratePDF(taskGroupRepository, configService);
            await GenerateExcel(taskGroupRepository, configService);
        }

        private static string ReadConfigPathFromEnvVariable()
        {
            string? configPath = Environment.GetEnvironmentVariable("EscapeProjectAbsoluteConfigPath");
            if (string.IsNullOrEmpty(configPath))
            {
                Console.Error.WriteLine("Environment variable 'EscapeProjectAbsoluteConfigPath' is not set.");
                Environment.Exit(1);
            }
            return configPath;
        }

        private async static Task GeneratePDF(TaskGroupRepository repository, ConfigurationService configService)
        {
            PDFServiceFactory pdfServiceFactory = new ITextPDFServiceFactory();
            RenderService renderServicePDF = new PDFRenderService(pdfServiceFactory, configService);
            GenerateTaskPlanUseCase useCasePDF = new GenerateTaskPlanUseCase(repository, renderServicePDF);
            TasksController tasksControllerPDF = new TasksController(useCasePDF);
            await tasksControllerPDF.GenerateTaskPlan();
        }

        private async static Task GenerateExcel(TaskGroupRepository repository, ConfigurationService configService)
        {
            ExcelServiceFactory excelServiceFactory = new ClosedXMLExcelServiceFactory();
            RenderService renderServiceExcel = new ExcelRenderService(excelServiceFactory, configService);
            GenerateTaskPlanUseCase useCaseExcel = new GenerateTaskPlanUseCase(repository, renderServiceExcel);
            TasksController tasksControllerExcel = new TasksController(useCaseExcel);
            await tasksControllerExcel.GenerateTaskPlan();
        }
    }
}
