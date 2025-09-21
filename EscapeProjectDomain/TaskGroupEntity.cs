using System.Data;
using BaseDomain;

namespace EscapeProjectDomain
{
    public class TaskGroupAggregate : AggregateRoot<NormalizedString>
    {
        private readonly NormalizedString id;
        public TaskGroupAggregate(string id, IEnumerable<TaskEntity> tasks)
        {
            this.id = id;
            HashSet<TaskEntity> setOfTasks = tasks.ToHashSet();
            if (!setOfTasks.Any())
            {
                throw new InvalidConstraintException($"Task group ({id}) must have at least one task.");
            }
            Tasks = setOfTasks;
        }

        public ISet<TaskEntity> Tasks
        {
            get; set;
        }

        public override NormalizedString Id => id;
    }
}
