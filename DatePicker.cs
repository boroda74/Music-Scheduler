using System;

namespace MusicBeePlugin
{
    public partial class DatePickerForm : PluginWindowTemplate
    {
        public DateTime dateTime;

        public DatePickerForm()
        {
            InitializeComponent();
        }

        public DatePickerForm(Plugin plugin, DateTime datetime)
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
    }
}
