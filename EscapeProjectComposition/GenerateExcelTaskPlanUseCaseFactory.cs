using EscapeProjectApplication.Services.Configuration;
using EscapeProjectApplication.UseCases;
using EscapeProjectInfrastructure.Render;
using EscapeProjectInfrastructure.Task;
using UIInfrastructure.Excel;

namespace EscapeProjectComposition
{
    public class GenerateExcelTaskPlanUseCaseFactory : GenerateTaskPlanUseCaseFactory
    {
        public GenerateTaskPlanUseCase Create(ConfigurationService configService)
        {
            var repo = new JSONTaskGroupRepository(configService);
            var renderService = new ExcelRenderService(new ClosedXMLExcelServiceFactory(), configService);
            return new GenerateTaskPlanUseCase(repo, renderService);
        }
    }
}
