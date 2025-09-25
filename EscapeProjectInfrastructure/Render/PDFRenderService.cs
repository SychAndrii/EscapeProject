using BaseDomain;
using EscapeProjectApplication.Services.Configuration;
using EscapeProjectDomain;
using UIApplication.PDF;
using UIDomain.Checkbox;
using UIDomain.Text;

namespace EscapeProjectInfrastructure.Render
{
    public class PDFRenderService : TaskRenderService
    {
        private readonly PDFServiceFactory pdfServiceFactory;
        private readonly ConfigurationService configService;
        private const float LINE_HEIGHT = 15;
        private const float SUBTASK_PADDING = 25;

        public PDFRenderService(PDFServiceFactory pdfServiceFactory, ConfigurationService configService)
        {
            this.pdfServiceFactory = pdfServiceFactory;
            this.configService = configService;
        }

        public override void RenderTaskPlan(List<TaskGroupAggregate> taskGroups)
        {
            // Ensure the TaskPlans directory exists
            string taskPlansDir = configService.Settings.TaskPlansDirectoryPath;
            Directory.CreateDirectory(taskPlansDir);

            // Now safely combine into a full destination path
            string destinationPath = Path.Combine(taskPlansDir, "taskPlan.pdf");
            float maxTaskGroupHeight = taskGroups.Select(t => GetTaskGroupHeight(t)).Max();

            PDFMetadataBuilder pdfMetadataBuilder = new PDFMetadataBuilder()
                .WithDestination(destinationPath)
                .WithDimensions(764, maxTaskGroupHeight)
                .WithMargins(25, 50);
            PDFService pdfService = pdfServiceFactory
                .Create(pdfMetadataBuilder);

            bool passedFirstPage = false;

            foreach (TaskGroupAggregate taskGroup in taskGroups)
            {
                NormalizedString taskGroupName = taskGroup.Id;
                ISet<TaskEntity> tasksForGroup = taskGroup.Tasks;

                if (!passedFirstPage)
                {
                    passedFirstPage = true;
                }
                else
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

                // Start queue with all top-level tasks at depth 0
                Queue<(TaskEntity Task, int Depth)> queue = new();
                foreach (var task in tasksForGroup)
                {
                    queue.Enqueue((task, 0));
                }

                float initialX = pdfService.CurrentPos.x;

                Stack<(TaskEntity Task, int Depth)> stack = new();
                for (int i = tasksForGroup.Count - 1; i >= 0; i--)
                {
                    stack.Push((tasksForGroup.ElementAt(i), 0));
                }

                while (stack.Count > 0)
                {
                    var (currentTask, depth) = stack.Pop();

                    pdfService.CurrentPos = (initialX + (depth * SUBTASK_PADDING), pdfService.CurrentPos.y);

                    RenderTask(currentTask, pdfService);

                    if (currentTask.Subtasks != null)
                    {
                        for (int i = currentTask.Subtasks.Count - 1; i >= 0; i--)
                        {
                            stack.Push((currentTask.Subtasks[i], depth + 1));
                        }
                    }

                    pdfService.CurrentPos = (initialX, pdfService.CurrentPos.y);
                }

                pdfService.CurrentPos = (initialX, pdfService.CurrentPos.y + (LINE_HEIGHT * 2));
            }

            pdfService.Close();
        }

        private void RenderTask(TaskEntity task, PDFService pdfService, int indentation = 0)
        {
            var checkboxText = task.Name;
            var durationText = GetFormattedDuration(task);

            if (durationText != null)
            {
                checkboxText += $" ({durationText})";
            }

            // Add the main line (task name + duration)
            var checkboxTextBuilder = new TextSettingsBuilder(checkboxText);
            var checkboxBuilder = new CheckboxSettingsBuilder()
                .WithText(checkboxTextBuilder);
            pdfService.RenderCheckbox(checkboxBuilder);

            // Add the date/time line underneath (smaller font)
            var rangeText = GetFormattedRange(task);
            if (rangeText != null)
            {
                pdfService.CurrentPos = (pdfService.CurrentPos.x, pdfService.CurrentPos.y + LINE_HEIGHT);

                var timeTextBuilder = new TextSettingsBuilder(rangeText)
                    .WithFontSize(10)
                    .WithFontStyle(TextStyle.ITALIC);
                pdfService.RenderText(timeTextBuilder);
            }
            pdfService.CurrentPos = (pdfService.CurrentPos.x, pdfService.CurrentPos.y + (LINE_HEIGHT * 1.5f));
        }

        private float GetTaskGroupHeight(TaskGroupAggregate aggregate)
        {
            float height = 0;

            // header (title + spacing after)
            height += LINE_HEIGHT * 2;

            foreach (var task in aggregate.Tasks)
            {
                height += GetTaskRenderHeight(task);
            }

            // spacing after group
            height += LINE_HEIGHT;

            return height;
        }

        private float GetTaskRenderHeight(TaskEntity task)
        {
            float height = 0;

            // checkbox line
            height += LINE_HEIGHT;

            // date range line + spacing
            if (GetFormattedRange(task) != null)
            {
                height += LINE_HEIGHT;   // range line
                height += LINE_HEIGHT * 1.5f; // spacing after
            }
            else
            {
                height += LINE_HEIGHT * 1.5f; // spacing if no range
            }

            // subtasks
            if (task.Subtasks != null)
            {
                foreach (var sub in task.Subtasks)
                {
                    height += GetTaskRenderHeight(sub);
                }
            }

            return height;
        }
    }
}
