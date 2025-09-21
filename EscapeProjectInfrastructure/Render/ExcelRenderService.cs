using BaseDomain;
using EscapeProjectApplication.Services;
using EscapeProjectDomain;
using UIApplication.Excel;
using UIDomain.Checkbox;
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
                .WithColumns(["Task", "Status", "Duration", "Time Range"]);
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
                    TextSettingsBuilder headerBuilder = new TextSettingsBuilder(task.Name);
                    headerBuilder
                        .WithFontWeight(TextWeight.BOLD)
                        .WithFontSize(20);
                    excelService.RenderText(headerBuilder);

                    excelService.CurrentPos = (excelService.CurrentPos.row, "Status");
                    var checkboxBuilder = new CheckboxSettingsBuilder();
                    excelService.RenderCheckbox(checkboxBuilder);

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
                                                .WithFontSize(10)
                                                .WithFontStyle(TextStyle.ITALIC);
                    excelService.RenderText(rangeTextBuilder);

                    excelService.CurrentPos = (excelService.CurrentPos.row + 1, "Task");
                }
            }

            excelService.RemoveWorksheet(ExcelService.DEFAULT_WORKSHEET);
            excelService.Close();
        }
    }
}
