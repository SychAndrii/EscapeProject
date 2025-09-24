using System.Runtime.InteropServices;
using EscapeProjectComposition;
using EscapeProjectComposition.GenerateTaskPlan.Factories;
using EscapeProjectComposition.GenerateTaskPlan.FactoryProviders;
using EscapeProjectPresentationCLI.Commands.GenerateTaskPlan;
using EscapeProjectPresentationCLI.UseCaseFactoryProviders.GenerateTaskPlan;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddTransient<GenerateExcelTaskPlanUseCaseFactory>();
        services.AddTransient<GeneratePDFTaskPlanUseCaseFactory>();

        // Register providers
        services.AddTransient<IGenerateExcelTaskPlanUseCaseFactoryProvider, GenerateExcelTaskPlanUseCaseFactoryProvider>();
        services.AddTransient<IGeneratePDFTaskPlanUseCaseFactoryProvider, GeneratePDFTaskPlanUseCaseFactoryProvider>();

        // Register commands
        services.AddTransient<GenerateExcelTaskPlanCommand>();
        services.AddTransient<GeneratePDFTaskPlanCommand>();

        // Wrap in Spectre registrar
        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.PropagateExceptions();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                config.SetApplicationName("escape-run.bat");
            }
            else
            {
                config.SetApplicationName("escape-run.sh");
            }

            config.AddCommand<GenerateExcelTaskPlanCommand>("generate-excel")
                  .WithExample(new[] { "generate-excel", "-h" });

            config.AddCommand<GeneratePDFTaskPlanCommand>("generate-pdf")
                  .WithExample(new[] { "generate-pdf", "-h" });
        });

        return await app.RunAsync(args);
    }
}
