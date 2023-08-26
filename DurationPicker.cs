using System;

namespace MusicBeePlugin
{
    public partial class DurationPickerForm : PluginWindowTemplate
    {
        public DateTime duration;

        public DurationPickerForm()
        {
            InitializeComponent();
        }

        public DurationPickerForm(Plugin plugin, DateTime pDuration)
        {
            InitializeComponent();

            PluginRef = plugin;
            duration = pDuration;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();
            dateTimePicker.Value = duration;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            duration = dateTimePicker.Value;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
