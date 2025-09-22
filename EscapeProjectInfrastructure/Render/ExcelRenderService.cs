using BaseDomain;
using EscapeProjectApplication.Services;
using EscapeProjectDomain;
using UIApplication.Excel;
using UIDomain.Select;
using UIDomain.Text;

namespace EscapeProjectInfrastructure.Render
{
    public class ExcelRenderService : RenderService
    {
        private readonly ExcelServiceFactory excelServiceFactory;

        public ExcelRenderService(ExcelServiceFactory excelServiceFactory)
        {
            this.excelServiceFactory = excelServiceFactory;
        }

        public void RenderTaskPlan(List<TaskGroupAggregate> taskGroups)
        {
            ExcelMetadataBuilder excelMetadataBuilder = new ExcelMetadataBuilder()
                .WithDestination("file.xlsx")
                .WithColumns([
                    GetColumn("Task"), GetColumn("Status"),
                    GetColumn("Duration"), GetColumn("Time Range")
                ]);
            ExcelService excelService = excelServiceFactory
                .Create(excelMetadataBuilder);

            foreach (TaskGroupAggregate taskGroup in taskGroups)
            {
                NormalizedString taskGroupName = taskGroup.Id;
                ISet<TaskEntity> tasksForGroup = taskGroup.Tasks;

                excelService.CreateNewWorksheet(taskGroupName);
                excelService.GoToWorksheet(taskGroupName);

                foreach (TaskEntity task in tasksForGroup)
                {
                    TextSettingsBuilder taskNameBuilder = new TextSettingsBuilder(task.Name);
                    taskNameBuilder
                        .WithFontWeight(TextWeight.BOLD);
                    excelService.RenderText(taskNameBuilder);

                    excelService.CurrentPos = (excelService.CurrentPos.row, "Status");

                    var notDoneOptionBuilder = new TextSettingsBuilder("Not done");
                    var doneOptionBuilder = new TextSettingsBuilder("Done");

                    SelectSettingsBuilder statusSelectBuilder = new SelectSettingsBuilder()
                                                    .AddOptions([notDoneOptionBuilder, doneOptionBuilder])
                                                    .WithSelectedIndex(0);
                    excelService.RenderSelect(statusSelectBuilder);

                    excelService.CurrentPos = (excelService.CurrentPos.row, "Duration");
                    var durationText = task.Duration();
                    if (durationText == null)
                    {
                        durationText = "Unknown";
                    }

                    var durationTextBuilder = new TextSettingsBuilder(durationText);
                    excelService.RenderText(durationTextBuilder);

                    excelService.CurrentPos = (excelService.CurrentPos.row, "Time Range");
                    var rangeText = task.Range();
                    if (rangeText == null)
                    {
                        rangeText = "Unknown";
                    }

                    var rangeTextBuilder = new TextSettingsBuilder(rangeText)
                                                .WithFontStyle(TextStyle.ITALIC);
                    excelService.RenderText(rangeTextBuilder);

                    excelService.CurrentPos = (excelService.CurrentPos.row + 1, "Task");
                }
            }

            excelService.RemoveWorksheet(ExcelService.DEFAULT_WORKSHEET);
            excelService.Close();
        }

        private TextSettingsBuilder GetColumn(string value)
        {
            var columnBuilder = new TextSettingsBuilder(value)
                .WithFontWeight(TextWeight.BOLD)
                .WithFontSize(14)
                .WithFontStyle(TextStyle.ITALIC);
            return columnBuilder;
        }
    }
}
