using EscapeProjectApplication.UseCases;
using EscapeProjectComposition.GenerateTaskPlan.Factories;
using EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan;

namespace EscapeProjectComposition.GenerateTaskPlan.FactoryProviders
{
    internal class GenerateExcelTaskPlanUseCaseFactoryProvider : IGenerateExcelTaskPlanUseCaseFactoryProvider
    {
        private readonly GenerateExcelTaskPlanUseCaseFactory useCaseFactory;

        public GenerateExcelTaskPlanUseCaseFactoryProvider(GenerateExcelTaskPlanUseCaseFactory useCaseFactory)
        {
            this.useCaseFactory = useCaseFactory;
        }

        public GenerateTaskPlanUseCaseFactory GetFactory()
        {
            return useCaseFactory;
        }
    }
}
