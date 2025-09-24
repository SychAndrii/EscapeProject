using System.ComponentModel;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Settings.GenerateTaskPlan
{
    public class GeneratePDFTaskPlanSettings : GenerateTaskPlanSettings
    {
        [CommandOption("-d|--directory <DIR>")]
        [Description("Output directory for generated PDF files")]
        [DefaultValue("./TaskPlans")]
        public override string DirectoryPath { get; init; } = "./TaskPlans";
    }
}
