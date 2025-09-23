using EscapeProjectApplication.Services.Configuration;
using EscapeProjectApplication.UseCases;
using EscapeProjectInfrastructure.Render;
using EscapeProjectInfrastructure.Task;
using UIInfrastructure.PDF;

namespace EscapeProjectComposition.GenerateTaskPlan
{
    public class GeneratePDFTaskPlanUseCaseFactory : GenerateTaskPlanUseCaseFactory
    {
        public GenerateTaskPlanUseCase Create(ConfigurationService configService)
        {
            var repo = new JSONTaskGroupRepository(configService);
            var renderService = new PDFRenderService(new ITextPDFServiceFactory(), configService);
            return new GenerateTaskPlanUseCase(repo, renderService);
        }
    }
}
