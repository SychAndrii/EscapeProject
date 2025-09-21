using System.Data;
using System.Text;
using BaseDomain;

namespace EscapeProjectDomain
{
    public class TaskEntity : ValueObject
    {
        private DateTime? from;
        private DateTime? until;

        public TaskEntity(NormalizedString name, DateTime? from, DateTime? until)
        {
            Name = name;
            From = from;
            Until = until;
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

        public NormalizedString? Duration()
        {
            if (From != null && Until != null)
            {
                var duration = Until.Value - From.Value;
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

        public NormalizedString? Range()
        {
            if (From != null && Until != null)
            {
                return $"{From.Value:yyyy-MM-dd HH:mm} – {Until.Value:yyyy-MM-dd HH:mm}";
            }
            else if (From != null)
            {
                return $"{From.Value:yyyy-MM-dd HH:mm} – ?";
            }
            else if (Until != null)
            {
                return $"? – {Until.Value:yyyy-MM-dd HH:mm}";
            }
            return null;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return From;
            yield return Until;
        }
    }
}
