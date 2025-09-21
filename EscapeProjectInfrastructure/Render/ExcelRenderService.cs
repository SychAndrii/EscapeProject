namespace EscapeProjectInfrastructure.Render
{
    /*
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

                if (!CurrentPageCanFitTaskGroup(taskGroup, pdfService))
                {
                    pdfService.CreateNewPage();
                    pdfService.GoToPage(pdfService.TotalPages);
                }

                TextSettingsBuilder headerBuilder = new TextSettingsBuilder(taskGroupName);
                headerBuilder
                    .WithFontWeight(TextWeight.BOLD)
                    .WithFontSize(20);
                pdfService.RenderText(headerBuilder);

                pdfService.CurrentPos = (pdfService.CurrentPos.x, pdfService.CurrentPos.y + (LINE_HEIGHT * 1.5f));

                foreach (TaskEntity task in tasksForGroup)
                {
                    var checkboxText = task.Name;
                    var durationText = task.Duration();

                    if (durationText != null)
                    {
                        checkboxText += $" ({durationText})";
                    }

                    // Add the main line (task name + duration)
                    var checkboxTextBuilder = new TextSettingsBuilder(checkboxText);
                    var checkboxBuilder = new CheckboxSettingsBuilder()
                        .WithText(checkboxTextBuilder);
                    pdfService.RenderCheckbox(checkboxBuilder);

                    pdfService.CurrentPos = (pdfService.CurrentPos.x, pdfService.CurrentPos.y + LINE_HEIGHT);

                    // Add the date/time line underneath (smaller font)
                    var rangeText = task.Range();
                    if (rangeText != null)
                    {
                        var timeTextBuilder = new TextSettingsBuilder(rangeText)
                            .WithFontSize(10)
                            .WithFontStyle(TextStyle.ITALIC);
                        pdfService.RenderText(timeTextBuilder);
                    }
                    pdfService.CurrentPos = (pdfService.CurrentPos.x, pdfService.CurrentPos.y + (LINE_HEIGHT * 1.5f));
                }
                pdfService.CurrentPos = (pdfService.CurrentPos.x, pdfService.CurrentPos.y + LINE_HEIGHT);
            }

            pdfService.Close();
        }

        private bool CurrentPageCanFitTaskGroup(TaskGroupAggregate aggregate, PDFService pdfService)
        {
            float height = 0;

            // header
            height += LINE_HEIGHT * 2;  // header line and space after

            // tasks
            foreach (var task in aggregate.Tasks)
            {
                height += LINE_HEIGHT * 2; // checkbox line, date line
                height += LINE_HEIGHT / 2; // spacing after task
            }

            // spacing after group
            height += LINE_HEIGHT;

            return pdfService.CurrentPos.y + height <= pdfService.PDFMetadata.Dimensions.pageHeight;
        }
    }
    */
}
