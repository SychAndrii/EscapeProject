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

        private static int GetNumberOfPages(List<TaskGroupEntity> taskGroups, PageSize singlePageSize, float lineHeight)
        {
            int taskGroupsAmount = taskGroups.Count;
            float pageContentHeight = singlePageSize.GetHeight() - (MARGIN_Y * 2);
            float totalContentHeight = lineHeight * 2 * taskGroupsAmount;

            foreach (var taskGroup in taskGroups)
            {
                TaskEntity[] tasksForGroup = taskGroup.tasks;
                totalContentHeight += lineHeight * tasksForGroup.Length;
            }

            int totalPages = (int)Math.Ceiling(totalContentHeight / pageContentHeight);
            return totalPages;
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

                float neededSpace = (lineHeight * 2) + (tasksForGroup.Count() * lineHeight);

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
                        var checkboxTextBuilder = new TextSettingsBuilder(task.name);
                        var checkboxBuilder = new CheckboxSettingsBuilder();
                        checkboxBuilder
                            .WithText(checkboxTextBuilder);
                        pageBuilder.AddCheckbox(MARGIN_X, currentY, checkboxBuilder);
                        currentY -= lineHeight;
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
