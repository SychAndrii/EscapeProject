
using System.Text.RegularExpressions;

namespace BaseDomain
{
    public class NormalizedString : ValueObject
    {
        private readonly string _value;
        public NormalizedString(string str)
        {
            _value = Regex.Replace(str.Trim(), @"\s+", " ");
            if (_value.Length == 0)
            {
                throw new ArgumentException("String must not be empty after normalizing it");
            }
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }

        public int Length => _value.Length;
        public override string ToString() => _value;

        public static implicit operator string(NormalizedString val) => val?._value ?? string.Empty;
        public static implicit operator NormalizedString(string str) => new(str);
    }
}
