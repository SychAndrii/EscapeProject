using System.Runtime.InteropServices;
using EscapeProjectComposition;
using EscapeProjectPresentationCLI.Commands.GenerateTaskPlan;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var registrar = new TypeRegistrar(new ServiceCollection());
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
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
