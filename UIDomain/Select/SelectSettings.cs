using UIDomain.Text;

namespace UIDomain.Select
{
    public class SelectSettings
    {
        private List<TextSettingsBuilder> options;
        private int selectedOptionIndex = 0;

        public IReadOnlyList<TextSettingsBuilder> Options
        {
            get => options;
            internal set
            {
                if (value.Count <= 1)
                {
                    throw new InvalidDataException("There should be at least 2 options for the select element");
                }
            }
        }

        public int SelectedOptionIndex
        {
            get => selectedOptionIndex;
            internal set
            {
                if (value < 0 || value >= options.Count)
                {
                    throw new InvalidDataException("Selected option index is out of bounds");
                }
                selectedOptionIndex = value;
            }
        }

        public SelectSettings(List<TextSettingsBuilder> options, int selectedOptionIndex)
        {
            this.options = options;
            SelectedOptionIndex = selectedOptionIndex;
        }
    }
}
