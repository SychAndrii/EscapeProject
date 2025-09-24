using EscapeProjectPresentationCLI.Settings.GenerateTaskPlan;
using EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Commands.GenerateTaskPlan
{
    public class GeneratePDFTaskPlanCommand : GenerateTaskPlanCommand<GeneratePDFTaskPlanSettings>
    {
        public GeneratePDFTaskPlanCommand(IGeneratePDFTaskPlanUseCaseFactoryProvider factoryProvider) : base(factoryProvider.GetFactory())
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, GeneratePDFTaskPlanSettings settings)
        {
            return await ExecuteTestCaseAsync(context, settings);
        }
    }
}
