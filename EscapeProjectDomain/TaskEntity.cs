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
            get => from;
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
            get => until;
            set
            {
                if (From != null && value != null && value <= From)
                {
                    throw new InvalidConstraintException($"{nameof(Until)} must not be less or equal than From.");
                }
                until = value;
            }
        }

        public TimeSpan? Duration
        {
            get
            {
                DateTime? effectiveFrom = From ?? Subtasks?.Where(s => s.From != null).Min(s => s.From);
                DateTime? effectiveUntil = Until ?? Subtasks?.Where(s => s.Until != null).Max(s => s.Until);

                return effectiveFrom != null && effectiveUntil != null
                    ? effectiveUntil.Value - effectiveFrom.Value
                    : null;
            }
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return From;
            yield return Until;
            yield return Subtasks;
        }
    }
}
