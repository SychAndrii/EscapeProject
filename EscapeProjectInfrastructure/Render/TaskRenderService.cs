using System.Text;
using EscapeProjectApplication.Services;
using EscapeProjectDomain;

namespace EscapeProjectInfrastructure.Render
{
    public abstract class TaskRenderService : RenderService
    {
        protected string? GetFormattedDuration(TaskEntity task)
        {
            if (task.Duration != null)
            {
                var duration = task.Duration.Value;
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

                return sb.ToString().TrimEnd();
            }
            return null;
        }
        public abstract void RenderTaskPlan(List<TaskGroupAggregate> taskGroups);
    }
}
