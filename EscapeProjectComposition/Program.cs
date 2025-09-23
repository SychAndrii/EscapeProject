using EscapeProjectComposition;
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

        services.AddTransient<IGenerateExcelTaskPlanUseCaseFactoryProvider, GenerateExcelTaskPlanUseCaseFactoryProvider>();
        services.AddTransient<IGeneratePDFTaskPlanUseCaseFactoryProvider, GeneratePDFTaskPlanUseCaseFactoryProvider>();

        services.AddTransient<GenerateExcelTaskPlanCommand>();
        services.AddTransient<GeneratePDFTaskPlanCommand>();

        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.AddCommand<GenerateExcelTaskPlanCommand>("generate-excel")
                .WithDescription("Generate a task plan in Excel format");

            config.AddCommand<GeneratePDFTaskPlanCommand>("generate-pdf")
                .WithDescription("Generate a task plan in PDF format");
        });

        return await app.RunAsync(args);
    }
}
