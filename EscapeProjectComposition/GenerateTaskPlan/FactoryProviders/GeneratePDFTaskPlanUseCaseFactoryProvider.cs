using EscapeProjectApplication.UseCases;
using EscapeProjectComposition.GenerateTaskPlan.Factories;
using EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan;

namespace EscapeProjectComposition.GenerateTaskPlan.FactoryProviders
{
    internal class GeneratePDFTaskPlanUseCaseFactoryProvider : IGeneratePDFTaskPlanUseCaseFactoryProvider
    {
        private readonly GeneratePDFTaskPlanUseCaseFactory useCaseFactory;

        public GeneratePDFTaskPlanUseCaseFactoryProvider(GeneratePDFTaskPlanUseCaseFactory useCaseFactory)
        {
            this.useCaseFactory = useCaseFactory;
        }

        public GenerateTaskPlanUseCaseFactory GetFactory()
        {
            return useCaseFactory;
        }
    }
}
