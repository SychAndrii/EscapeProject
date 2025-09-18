using System.Text;
using EscapeProject.Checkbox;
using EscapeProject.Task;
using EscapeProject.Text;
using iText.Kernel.Geom;
using iText.Layout;

namespace EscapeProject
{
    internal class Program
    {
        private const string DEST = "file.pdf";
        private const float MARGIN_Y = 40;
        private const float MARGIN_X = 25;
        public Program()
        {
        }

        private static int GetNumberOfPages(List<TaskGroupEntity> taskGroups, PageSize pageSize, float lineHeight)
        {
            float pageContentHeight = pageSize.GetHeight() - (MARGIN_Y * 2);
            float remainingHeight = pageContentHeight;
            int pages = 1;

            foreach (var group in taskGroups)
            {
                float groupHeight = CalculateGroupHeight(group, lineHeight);

                if (groupHeight > remainingHeight)
                {
                    pages++;
                    remainingHeight = pageContentHeight;
                }

                remainingHeight -= groupHeight;
            }

            return pages;
        }

        private static void GeneratePDF(List<TaskGroupEntity>.Enumerator taskGroups, DocumentIterator document, float lineHeight, float yStart)
        {
            bool hasTask = taskGroups.MoveNext();
            float currentY = yStart;
            while (document.CanWrite() && hasTask)
            {
                var taskGroup = taskGroups.Current;
                var taskGroupName = taskGroup.name;
                var tasksForGroup = taskGroup.tasks;

                float neededSpace = CalculateGroupHeight(taskGroup, lineHeight);

                if (document.HasVerticalSpace(neededSpace))
                {
                    document.ReserveVerticalSpace(neededSpace);
                    PageContentBuilder pageBuilder = document.GetCurrentPage();

                    TextSettingsBuilder headerBuilder = new TextSettingsBuilder(taskGroupName);
                    headerBuilder
                        .WithFontWeight(TextWeight.BOLD)
                        .WithFontSize(20);
                    pageBuilder.AddText(MARGIN_X, currentY, headerBuilder);
                    currentY -= lineHeight;

                    foreach (TaskEntity task in tasksForGroup)
                    {
                        var checkboxText = task.name;

                        // Duration inline with task name
                        if (task.from != null && task.until != null)
                        {
                            var duration = task.until.Value - task.from.Value;
                            var sb = new StringBuilder();

                            if (duration.Days > 0)
                            {
                                sb.Append($"{duration.Days}d ");
                            }

                            if (duration.Hours > 0)
                            {
                                sb.Append($"{duration.Hours}h ");
                            }

                            if (duration.Minutes > 0)
                            {
                                sb.Append($"{duration.Minutes}m ");
                            }

                            if (duration.Seconds > 0)
                            {
                                sb.Append($"{duration.Seconds}s ");
                            }

                            checkboxText += $" ({sb.ToString().TrimEnd()})";
                        }

                        // Add the main line (task name + duration)
                        var checkboxTextBuilder = new TextSettingsBuilder(checkboxText);
                        var checkboxBuilder = new CheckboxSettingsBuilder()
                            .WithText(checkboxTextBuilder);
                        pageBuilder.AddCheckbox(MARGIN_X, currentY, checkboxBuilder);
                        currentY -= lineHeight;

                        // Add the date/time line underneath (smaller font)
                        if (task.from != null || task.until != null)
                        {
                            string rangeText = task.from != null && task.until != null
                                ? $"{task.from.Value:yyyy-MM-dd HH:mm} – {task.until.Value:yyyy-MM-dd HH:mm}"
                                : task.from != null ? $"{task.from.Value:yyyy-MM-dd HH:mm} – ?" : $"? – {task.until.Value:yyyy-MM-dd HH:mm}";

                            var timeTextBuilder = new TextSettingsBuilder(rangeText)
                                .WithFontSize(10)
                                .WithFontStyle(TextStyle.ITALIC);
                            pageBuilder.AddText(MARGIN_X + 30, currentY, timeTextBuilder); // indent
                            currentY -= lineHeight;
                        }

                        currentY -= lineHeight / 2; // extra spacing between tasks
                    }
                    currentY -= lineHeight;
                    hasTask = taskGroups.MoveNext();
                }
                else
                {
                    document.GoToNextPage();
                    currentY = yStart;
                }
            }
        }

        private static float CalculateGroupHeight(TaskGroupEntity group, float lineHeight)
        {
            float height = 0;

            // header
            height += lineHeight;  // header line
            height += lineHeight;  // spacing after header

            // tasks
            foreach (var task in group.tasks)
            {
                height += lineHeight; // checkbox line

                if (task.from != null || task.until != null)
                {
                    height += lineHeight; // date line
                }

                height += lineHeight / 2; // spacing after task
            }

            // spacing after group
            height += lineHeight;

            return height;
        }

        private static void Main(string[] args)
        {
            PageSize pageSize = PageSize.A4;
            float lineHeight = 25;

            var tasksRepository = new TaskRepository();
            List<TaskGroupEntity> taskGroups = tasksRepository.GetTaskGroups();

            int totalPages = GetNumberOfPages(taskGroups, pageSize, lineHeight);

            Document document = new DocumentBuilder(DEST)
                                      .SetPageSize(pageSize)
                                      .SetMarginX(MARGIN_X)
                                      .SetMarginY(MARGIN_Y)
                                      .AddPages(totalPages)
                                      .BuildDocument();

            using var taskGroupsIterator = taskGroups.GetEnumerator();
            DocumentIterator documentIterator = new DocumentIterator(document);
            GeneratePDF(taskGroupsIterator, documentIterator, lineHeight, pageSize.GetHeight() - MARGIN_Y);

            document.Close();
        }
    }
}
