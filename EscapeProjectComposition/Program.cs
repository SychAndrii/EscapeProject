using EscapeProjectPresentationCLI.Commands.GenerateTaskPlan;
using Spectre.Console.Cli;

namespace EscapeProjectComposition
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<GenerateExcelTaskPlanCommand>("generate-excel")
                    .WithDescription("Generate a task plan in Excel format")
                    .WithExample(["generate-excel", "-t", "tasks.json"]);

                // Register PDF generator
                config.AddCommand<GeneratePDFTaskPlanCommand>("generate-pdf")
                    .WithDescription("Generate a task plan in PDF format")
                    .WithExample(["generate-pdf", "-t", "tasks.json"]);
            });
            return await app.RunAsync(args);
        }
    }
}
