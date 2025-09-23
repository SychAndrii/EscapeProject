using EscapeProjectApplication.UseCases;
using EscapeProjectPresentationCLI.Settings.GenerateTaskPlan;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Commands.GenerateTaskPlan
{
    public class GenerateExcelTaskPlanCommand : GenerateTaskPlanCommand<GenerateExcelTaskPlanSettings>
    {
        public GenerateExcelTaskPlanCommand(GenerateTaskPlanUseCaseFactory factory) : base(factory)
        {
        }

        public override async Task<int> ExecuteAsync(CommandContext context, GenerateExcelTaskPlanSettings settings)
        {
            return await ExecuteTestCaseAsync(context, settings);
        }
    }
}
