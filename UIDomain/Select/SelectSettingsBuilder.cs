using UIDomain.Text;

namespace UIDomain.Select
{
    public class SelectSettingsBuilder
    {
        private readonly List<TextSettingsBuilder> options = [];
        private int selectedOptionIndex = 0;

        /// <summary>
        /// Adds a single option to the select element.
        /// </summary>
        public SelectSettingsBuilder AddOption(TextSettingsBuilder option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }
            options.Add(option);
            return this;
        }

        /// <summary>
        /// Adds multiple options to the select element.
        /// </summary>
        public SelectSettingsBuilder AddOptions(IEnumerable<TextSettingsBuilder> opts)
        {
            if (opts == null)
            {
                throw new ArgumentNullException(nameof(opts));
            }
            options.AddRange(opts);
            return this;
        }

        /// <summary>
        /// Sets the index of the default selected option.
        /// </summary>
        public SelectSettingsBuilder WithSelectedIndex(int index)
        {
            selectedOptionIndex = index;
            return this;
        }

        /// <summary>
        /// Builds the SelectSettings object, validating inputs.
        /// </summary>
        public SelectSettings Build()
        {
            return options.Count <= 1
                ? throw new InvalidDataException("There should be at least 2 options for the select element")
                : selectedOptionIndex < 0 || selectedOptionIndex >= options.Count
                ? throw new InvalidDataException("Selected option index is out of bounds")
                : new SelectSettings(options, selectedOptionIndex);
        }
    }
}
