using EscapeProjectApplication.UseCases;

namespace EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan
{
    public interface IGenerateExcelTaskPlanUseCaseFactoryProvider
    {
        GenerateTaskPlanUseCaseFactory GetFactory();
    }
}
