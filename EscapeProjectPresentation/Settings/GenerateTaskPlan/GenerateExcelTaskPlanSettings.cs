using System.ComponentModel;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Settings.GenerateTaskPlan
{
    public class GenerateExcelTaskPlanSettings : GenerateTaskPlanSettings
    {
        [CommandOption("-d|--directory <DIR>")]
        [Description("Output directory for generated Excel files")]
        [DefaultValue("./TaskPlans")]
        public override string DirectoryPath { get; init; } = "./TaskPlans";
    }
}
