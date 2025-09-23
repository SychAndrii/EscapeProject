using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI.Settings.GenerateTaskPlan
{
    public abstract class GenerateTaskPlanSettings : CommandSettings
    {
        [CommandOption("-t|--tasks <PATH>")]
        [Description("Path to the tasks JSON file")]
        public required string TasksFilePath
        {
            get; init;
        }

        public abstract string DirectoryPath
        {
            get; init;
        }

        public override ValidationResult Validate()
        {
            // --- Validate tasks file ---
            if (string.IsNullOrWhiteSpace(TasksFilePath))
            {
                return ValidationResult.Error("You must provide a tasks JSON file.");
            }

            if (!File.Exists(TasksFilePath))
            {
                return ValidationResult.Error($"File not found: {TasksFilePath}");
            }

            if (Path.GetExtension(TasksFilePath).ToLowerInvariant() != ".json")
            {
                return ValidationResult.Error("Tasks file must have a .json extension.");
            }

            // --- Validate directory path (syntax only) ---
            char[] invalidPathChars = Path.GetInvalidPathChars();
            return DirectoryPath.IndexOfAny(invalidPathChars) >= 0
                ? ValidationResult.Error($"Invalid characters in directory path: {DirectoryPath}")
                : ValidationResult.Success();
        }
    }
}
