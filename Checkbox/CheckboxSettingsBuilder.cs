using EscapeProject.Checkbox;
using EscapeProject.Text;

namespace EscapeProjectDomain.Checkbox
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

        internal CheckboxSettings Build()
        {
            CheckboxSettings cs = new CheckboxSettings
            {
                DefaultValue = defaultValue
            };
            if (size != 0)
            {
                cs.Size = size;
            }
            if (textSettingsBuilder != null)
            {
                cs.Text = textSettingsBuilder.Build();
            }
            return cs;
        }
    }
}
