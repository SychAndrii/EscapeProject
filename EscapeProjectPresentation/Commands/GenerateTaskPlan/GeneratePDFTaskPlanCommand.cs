using EscapeProjectApplication.UseCases;
using EscapeProjectPresentationCLI.Settings.GenerateTaskPlan;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Commands.GenerateTaskPlan
{
    public class GeneratePDFTaskPlanCommand : GenerateTaskPlanCommand<GeneratePDFTaskPlanSettings>
    {
        public GeneratePDFTaskPlanCommand(GenerateTaskPlanUseCaseFactory factory) : base(factory)
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, GeneratePDFTaskPlanSettings settings)
        {
            return await ExecuteTestCaseAsync(context, settings);
        }
    }
}
