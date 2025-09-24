using EscapeProjectPresentationCLI.Settings.GenerateTaskPlan;
using EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Commands.GenerateTaskPlan
{
    public class GenerateExcelTaskPlanCommand : GenerateTaskPlanCommand<GenerateExcelTaskPlanSettings>
    {
        public GenerateExcelTaskPlanCommand(IGenerateExcelTaskPlanUseCaseFactoryProvider factoryProvider) : base(factoryProvider.GetFactory())
        {
        }

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateExcelTaskPlanSettings settings)
        {
            return await ExecuteTestCaseAsync(context, settings);
        }
    }
}
