using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace EscapeProjectPresentationCLI
{
    public class GeneratePDFTaskPlanCommand
    {
        public class Settings : CommandSettings
        {
            [Description("Path to the repository where to save the PDF file")]
            [CommandOption("-d|--directory")]
            [DefaultValue("./TaskPlans")]
            public string DirectoryPath { get; init; } = "./TaskPlans";

            [Description("Path to the file which contains tasks in JSON format")]
            [CommandOption("-t|--tasks")]
            public required string TasksFilePath
            {
                get; init;
            }

            public override ValidationResult Validate()
            {
                // --- Path syntax checks ---
                char[] invalidFileChars = Path.GetInvalidFileNameChars();
                char[] invalidPathChars = Path.GetInvalidPathChars();

                if (string.IsNullOrWhiteSpace(TasksFilePath))
                {
                    return ValidationResult.Error("The tasks file path must be provided.");
                }

                if (TasksFilePath.IndexOfAny(invalidPathChars) >= 0 ||
                    Path.GetFileName(TasksFilePath).IndexOfAny(invalidFileChars) >= 0)
                {
                    return ValidationResult.Error($"Invalid characters in tasks file path: {TasksFilePath}");
                }

                if (DirectoryPath.IndexOfAny(invalidPathChars) >= 0)
                {
                    return ValidationResult.Error($"Invalid characters in directory path: {DirectoryPath}");
                }

                // --- Existence checks ---
                return !File.Exists(TasksFilePath)
                    ? ValidationResult.Error($"The specified tasks file does not exist: {TasksFilePath}")
                    : ValidationResult.Success();
            }
        }
    }
}
