using System;

namespace MusicBeePlugin
{
    public partial class TimePickerForm : PluginWindowTemplate
    {
        public DateTime dateTime;

        public TimePickerForm()
        {
            InitializeComponent();
        }

        public TimePickerForm(Plugin plugin, DateTime datetime)
        {
            InitializeComponent();

            PluginRef = plugin;
            dateTime = datetime;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();
            dateTimePicker.Value = dateTime;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            dateTime = dateTimePicker.Value;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
