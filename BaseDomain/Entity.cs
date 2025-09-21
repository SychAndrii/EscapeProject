namespace BaseDomain
{
    public abstract class Entity<T> where T : notnull
    {
        private int? _requestedHashCode;

        public abstract T Id
        {
            get;
        }

        public bool IsTransient()
        {
            return EqualityComparer<T>.Default.Equals(Id, default);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = (Entity<T>)obj;

            return !IsTransient() && !other.IsTransient() && EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 31;
                }
                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(Entity<T>? left, Entity<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<T>? left, Entity<T>? right)
        {
            return !(left == right);
        }
    }
}
