using System;

namespace MusicBeePlugin
{
    public partial class DayPickerForm : PluginWindowTemplate
    {
        public byte days;

        public DayPickerForm()
        {
            InitializeComponent();
        }

        public DayPickerForm(Plugin plugin, byte pDays)
        {
            InitializeComponent();

            PluginRef = plugin;
            days = pDays;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            if (ScheduledPlaylist.FirstDayOfTheWeek != 0)
            {
                suCheckBox.Visible = false;
                su2CheckBox.Visible = true;
            }

            suCheckBox.Checked = su2CheckBox.Checked = (days & ScheduledPlaylist.SundayBit) == ScheduledPlaylist.SundayBit;
            moCheckBox.Checked = (days & ScheduledPlaylist.MondayBit) == ScheduledPlaylist.MondayBit;
            tuCheckBox.Checked = (days & ScheduledPlaylist.TuesdayBit) == ScheduledPlaylist.TuesdayBit;
            weCheckBox.Checked = (days & ScheduledPlaylist.WednesdayBit) == ScheduledPlaylist.WednesdayBit;
            thCheckBox.Checked = (days & ScheduledPlaylist.ThursdayBit) == ScheduledPlaylist.ThursdayBit;
            frCheckBox.Checked = (days & ScheduledPlaylist.FridayBit) == ScheduledPlaylist.FridayBit;
            saCheckBox.Checked = (days & ScheduledPlaylist.SaturdayBit) == ScheduledPlaylist.SaturdayBit;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            days = 0x00;

            if (ScheduledPlaylist.FirstDayOfTheWeek == 0)
            {
                days |= (byte)(suCheckBox.Checked ? ScheduledPlaylist.SundayBit : 0x00);
            }
            else
            {
                days |= (byte)(su2CheckBox.Checked ? ScheduledPlaylist.SundayBit : 0x00);
            }

            days |= (byte)(moCheckBox.Checked ? ScheduledPlaylist.MondayBit : 0x00);
            days |= (byte)(tuCheckBox.Checked ? ScheduledPlaylist.TuesdayBit : 0x00);
            days |= (byte)(weCheckBox.Checked ? ScheduledPlaylist.WednesdayBit : 0x00);
            days |= (byte)(thCheckBox.Checked ? ScheduledPlaylist.ThursdayBit : 0x00);
            days |= (byte)(frCheckBox.Checked ? ScheduledPlaylist.FridayBit : 0x00);
            days |= (byte)(saCheckBox.Checked ? ScheduledPlaylist.SaturdayBit : 0x00);

            Close();
        }
    }
}
