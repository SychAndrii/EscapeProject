using EscapeProjectApplication.UIElements.Text;

namespace EscapeProjectApplication.UIElements.Checkbox;

public class CheckboxSettings
{
    private float size = 15;

    public float Size
    {
        get => size;
        set
        {
            if (value < 0)
            {
                throw new InvalidDataException($"{nameof(Size)} cannot be <= 0");
            }

            size = value;
        }
    }

    public CheckboxState DefaultValue { get; set; } = CheckboxState.OFF;
    public TextSettingsBuilder? TextSettingsBuilder
    {
        get; set;
    }
}
