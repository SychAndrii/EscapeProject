using EscapeProjectApplication.UseCases;
using EscapeProjectPresentationCLI.Configuration;
using EscapeProjectPresentationCLI.Settings.GenerateTaskPlan;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Commands.GenerateTaskPlan
{
    public abstract class GenerateTaskPlanCommand<T>
        : AsyncCommand<T>
        where T : GenerateTaskPlanSettings
    {
        protected readonly GenerateTaskPlanUseCaseFactory useCaseFactory;

        protected GenerateTaskPlanCommand(GenerateTaskPlanUseCaseFactory factory)
        {
            useCaseFactory = factory;
        }

        public override abstract Task<int> ExecuteAsync(CommandContext context, T settings);

        protected async Task<int> ExecuteTestCaseAsync(CommandContext context, GenerateTaskPlanSettings settings)
        {
            var configService = new SpectreConfigurationService(settings);
            var useCase = useCaseFactory.Create(configService);
            await useCase.Execute();
            return 0;
        }
    }
}
