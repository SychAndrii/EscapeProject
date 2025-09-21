using System.Reflection;

namespace BaseDomain;

/// <summary>
/// Base class for domain enumerations (a "smart enum").
/// Provides name-based identity and value equality.
///
/// WARNING: Do not add any additional instance fields in subclasses unless
/// they are explicitly accounted for in equality (via GetEqualityComponents).
/// Otherwise, you'll silently break value semantics.
/// </summary>
public abstract class Enumeration : ValueObject, IComparable
{
    public NormalizedString Name
    {
        get;
    }

    protected Enumeration(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Enumeration name cannot be null or empty.");
        }

        Name = new NormalizedString(name);
    }

    public override string ToString() => Name.ToString();

    public int CompareTo(object? other)
    {
        return other is not Enumeration otherEnum
            ? 1
            : string.Compare(Name.ToString(), otherEnum.Name.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => (T)f.GetValue(null)!);
    }

    public static T FromString<T>(string name) where T : Enumeration
    {
        var normalized = new NormalizedString(name);
        return GetAll<T>().FirstOrDefault(x => x.Name.Equals(normalized))
               ?? throw new ArgumentException($"Invalid {typeof(T).Name}: {name}");
    }
}
