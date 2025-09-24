using EscapeProjectApplication.UseCases;

namespace EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan
{
    public interface IGeneratePDFTaskPlanUseCaseFactoryProvider
    {
        GenerateTaskPlanUseCaseFactory GetFactory();
    }
}
