using EscapeProject.Text;
using EscapeProjectDomain.Checkbox;

namespace EscapeProject.Checkbox
{
    internal class CheckboxSettings
    {
        private float size = 15;

        internal float Size
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

        internal CheckboxState DefaultValue { get; set; } = CheckboxState.OFF;
        internal TextSettings? Text
        {
            get; set;
        }
    }
}
