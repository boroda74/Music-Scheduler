using System;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class AutoBackupSettingsPlugin : PluginWindowTemplate
    {
        private decimal initialAutobackupInterval;
        private string initialAutobackupDirectory;
        
        public AutoBackupSettingsPlugin()
        {
            InitializeComponent();
        }

        public AutoBackupSettingsPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            initialAutobackupInterval = Plugin.SavedSettings.autobackupInterval;
            initialAutobackupDirectory = Plugin.SavedSettings.autobackupDirectory;

            autobackupFolderTextBox.Text = Plugin.SavedSettings.autobackupDirectory;
            autobackupPrefixTextBox.Text = Plugin.SavedSettings.autobackupPrefix;

            autobackupNumericUpDown.Value = Plugin.SavedSettings.autobackupInterval;
            numberOfDaysNumericUpDown.Value = Plugin.SavedSettings.autodeleteKeepNumberOfDays;
            numberOfFilesNumericUpDown.Value = Plugin.SavedSettings.autodeleteKeepNumberOfFiles;

            if (Plugin.SavedSettings.autobackupInterval != 0)
                autobackupCheckBox.Checked = true;

            if (Plugin.SavedSettings.autodeleteKeepNumberOfDays != 0)
                autodeleteOldCheckBox.Checked = true;

            if (Plugin.SavedSettings.autodeleteKeepNumberOfFiles != 0)
                autodeleteManyCheckBox.Checked = true;
        }

        private void autobackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autobackupCheckBox.Checked)
                autobackupNumericUpDown.Enabled = true;
            else
                autobackupNumericUpDown.Enabled = false;
        }

        private void autodeleteOldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autodeleteOldCheckBox.Checked)
                numberOfDaysNumericUpDown.Enabled = true;
            else
                numberOfDaysNumericUpDown.Enabled = false;
        }

        private void autodeleteManyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autodeleteManyCheckBox.Checked)
                numberOfFilesNumericUpDown.Enabled = true;
            else
                numberOfFilesNumericUpDown.Enabled = false;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = autobackupFolderTextBox.Text;

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            autobackupFolderTextBox.Text = dialog.SelectedPath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Plugin.SavedSettings.autobackupDirectory = autobackupFolderTextBox.Text;
            Plugin.SavedSettings.autobackupPrefix = autobackupPrefixTextBox.Text;

            if (autobackupCheckBox.Checked)
                Plugin.SavedSettings.autobackupInterval = autobackupNumericUpDown.Value;
            else
                Plugin.SavedSettings.autobackupInterval = 0;

            if (autodeleteOldCheckBox.Checked)
                Plugin.SavedSettings.autodeleteKeepNumberOfDays = numberOfDaysNumericUpDown.Value;
            else
                Plugin.SavedSettings.autodeleteKeepNumberOfDays = 0;

            if (autodeleteManyCheckBox.Checked)
                Plugin.SavedSettings.autodeleteKeepNumberOfFiles = numberOfFilesNumericUpDown.Value;
            else
                Plugin.SavedSettings.autodeleteKeepNumberOfFiles = 0;


            TagToolsPlugin.periodicAutobackupTimer.Dispose();
            TagToolsPlugin.periodicAutobackupTimer = null;

            if (initialAutobackupDirectory != Plugin.SavedSettings.autobackupDirectory)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(TagToolsPlugin.sbMovingBackupsToNewFolder);

                lock (TagToolsPlugin.autobackupLocker)
                {
                    System.IO.Directory.Move(initialAutobackupDirectory, Plugin.SavedSettings.autobackupDirectory);
                }

                MbApiInterface.MB_SetBackgroundTaskMessage("");
            }

            if (initialAutobackupInterval != Plugin.SavedSettings.autobackupInterval && Plugin.SavedSettings.autobackupInterval != 0)
            {
                TagToolsPlugin.periodicAutobackupTimer = new System.Threading.Timer(TagToolsPlugin.periodicAutobackup, null, (int)Plugin.SavedSettings.autobackupInterval * 1000 * 60, (int)Plugin.SavedSettings.autobackupInterval * 1000 * 60);
            }


            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
