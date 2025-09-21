using UIDomain.Text;

namespace UIDomain.Checkbox
{
    public class CheckboxSettingsBuilder
    {
        private float size;

        private CheckboxState defaultValue;
        private TextSettingsBuilder? textSettingsBuilder;

        public CheckboxSettingsBuilder WithText(TextSettingsBuilder textBuilder)
        {
            textSettingsBuilder = textBuilder;
            return this;
        }


        public CheckboxSettingsBuilder WithDefaultChecked(bool isChecked)
        {
            defaultValue = isChecked ? CheckboxState.ON : CheckboxState.OFF;
            return this;
        }

        public CheckboxSettingsBuilder WithSize(float size)
        {
            this.size = size;
            return this;
        }

        public CheckboxSettings Build()
        {
            CheckboxSettings cs = new CheckboxSettings
            {
                DefaultValue = defaultValue
            };
            if (size != 0)
            {
                cs.Size = size;
            }
            cs.TextSettingsBuilder = textSettingsBuilder;
            return cs;
        }
    }
}
