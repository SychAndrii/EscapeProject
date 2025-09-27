using System.Collections.Immutable;
using System.Data;
using BaseDomain;

namespace EscapeProjectDomain
{
    public class TaskEntity : ValueObject
    {
        private DateTime? from;
        private DateTime? until;

        public TaskEntity(NormalizedString name, DateTime? from, DateTime? until, IEnumerable<TaskEntity>? subtasks = null)
        {
            Name = name;
            From = from;
            Until = until;

            if (subtasks != null)
            {
                if (subtasks.Count() == 0)
                {
                    throw new InvalidDataException("If subtasks are specified, there has to be at least one subtask");
                }
                Subtasks = subtasks.ToImmutableList();
            }
        }

        public ImmutableList<TaskEntity>? Subtasks
        {
            get;
        }

        public NormalizedString Name
        {
            get; set;
        }

        public DateTime? From
        {
            get
            {
                DateTime? currentFrom = from;
                if (currentFrom != null)
                {
                    return currentFrom;
                }

                if (Subtasks != null)
                {
                    DateTime? currentUntil = Until;
                    DateTime maxUntil = currentUntil ?? DateTime.MaxValue;

                    var tasksWithKnownFrom = Subtasks.Where(t => t.From != null && t.From < maxUntil);
                    if (tasksWithKnownFrom.Count() == 0)
                    {
                        return null;
                    }

                    var minFromInSubtasks = tasksWithKnownFrom.Min(t => t.From);
                    return minFromInSubtasks;
                }

                return null;
            }
            set
            {
                if (Until != null && value != null && value >= Until)
                {
                    throw new InvalidConstraintException($"{nameof(From)} must not be bigger or equal than Until.");
                }
                from = value;
            }
        }

        public DateTime? Until
        {
            get
            {
                DateTime? currentUntil = until;
                if (currentUntil != null)
                {
                    return currentUntil;
                }

                if (Subtasks != null)
                {
                    DateTime? currentFrom = From;
                    DateTime minFrom = currentFrom ?? DateTime.MinValue;

                    var tasksWithKnownUntil = Subtasks.Where(t => t.Until != null && t.Until > minFrom);
                    if (tasksWithKnownUntil.Count() == 0)
                    {
                        return null;
                    }

                    var maxUntilInSubtasks = tasksWithKnownUntil.Max(t => t.Until);
                    return maxUntilInSubtasks;
                }

                return null;
            }
            set
            {
                if (From != null && value != null && value <= From)
                {
                    throw new InvalidConstraintException($"{nameof(Until)} must not be less or equal than From.");
                }
                until = value;
            }
        }

        public TimeSpan? Duration => Until.HasValue && From.HasValue ? Until.Value - From.Value : null;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return From;
            yield return Until;
            yield return Subtasks;
        }
    }
}
