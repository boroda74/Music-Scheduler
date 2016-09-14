using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public partial class SettingsPlugin : PluginWindowTemplate
    {
        protected Plugin.PluginInfo about;

        protected void setMenuPlacementRadioButtons(int pos)
        {
            switch (pos)
            {
                case 1:
                    menuPlacement1RadioButton.Checked = true;
                    break;
                case 2:
                    menuPlacement2RadioButton.Checked = true;
                    break;
                default:
                    menuPlacement3RadioButton.Checked = true;
                    break;
            }
        }

        protected int getMenuPlacementRadioButtons()
        {
            if (menuPlacement1RadioButton.Checked) return 1;
            else if (menuPlacement2RadioButton.Checked) return 2;
            else return 3;
        }

        protected void setCloseShowWindowsRadioButtons(int pos)
        {
            switch (pos)
            {
                case 1:
                    closeHiddenCommandWindowsRadioButton.Checked = true;
                    break;
                default:
                    showHiddenCommandWindowsRadioButton.Checked = true;
                    break;
            }
        }

        protected int getCloseShowWindowsRadioButtons()
        {
            if (closeHiddenCommandWindowsRadioButton.Checked) return 1;
            else return 2;
        }

        public SettingsPlugin()
        {
            InitializeComponent();
        }

        public SettingsPlugin(Plugin TagToolsPluginParam, Plugin.PluginInfo aboutParam)
        {
            InitializeComponent();

            TagToolsPlugin = TagToolsPluginParam;
            about = aboutParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text += about.VersionMajor + "." + about.VersionMinor + "." + about.Revision;

            setMenuPlacementRadioButtons(Plugin.SavedSettings.menuPlacement);
            contextMenuCheckBox.Checked = Plugin.SavedSettings.contextMenu;

            showCopyTagCheckBox.Checked = !Plugin.SavedSettings.dontShowCopyTag;
            showSwapTagsCheckBox.Checked = !Plugin.SavedSettings.dontShowSwapTags;
            showChangeCaseCheckBox.Checked = !Plugin.SavedSettings.dontShowChangeCase;
            showRencodeTagCheckBox.Checked = !Plugin.SavedSettings.dontShowRencodeTag;
            showLibraryReportsCheckBox.Checked = !Plugin.SavedSettings.dontShowLibraryReports;
            showAutorateCheckBox.Checked = !Plugin.SavedSettings.dontShowAutorate;
            showASRCheckBox.Checked = !Plugin.SavedSettings.dontShowASR;
            showCARCheckBox.Checked = !Plugin.SavedSettings.dontShowCAR;
            showShowHiddenWindowsCheckBox.Checked = !Plugin.SavedSettings.dontShowShowHiddenWindows;

            showBackupRestoreCheckBox.Checked = !Plugin.SavedSettings.dontShowBackupRestore;

            useSkinColorsCheckBox.Checked = Plugin.SavedSettings.useSkinColors;
            setCloseShowWindowsRadioButtons(Plugin.SavedSettings.closeShowHiddenWindows);

            playCompletedSoundCheckBox.Checked = !Plugin.SavedSettings.dontPlayCompletedSound;
            playStartedSoundCheckBox.Checked = Plugin.SavedSettings.playStartedSound;
            playStoppedSoundCheckBox.Checked = Plugin.SavedSettings.playCanceledSound;

            unitKBox.Text = Plugin.SavedSettings.unitK;
            unitMBox.Text = Plugin.SavedSettings.unitM;
            unitGBox.Text = Plugin.SavedSettings.unitG;

            menuPlacement1RadioButton.Text = Regex.Replace(menuPlacement1RadioButton.Text, "^(.*?)/[^\']*(.*)", "$1/" + TagToolsPlugin.pluginName + "$2");
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.menuPlacement = getMenuPlacementRadioButtons();
            Plugin.SavedSettings.contextMenu = contextMenuCheckBox.Checked;

            Plugin.SavedSettings.dontShowCopyTag = !showCopyTagCheckBox.Checked;
            Plugin.SavedSettings.dontShowSwapTags = !showSwapTagsCheckBox.Checked;
            Plugin.SavedSettings.dontShowChangeCase = !showChangeCaseCheckBox.Checked;
            Plugin.SavedSettings.dontShowRencodeTag = !showRencodeTagCheckBox.Checked;
            Plugin.SavedSettings.dontShowLibraryReports = !showLibraryReportsCheckBox.Checked;
            Plugin.SavedSettings.dontShowAutorate = !showAutorateCheckBox.Checked;
            Plugin.SavedSettings.dontShowASR = !showASRCheckBox.Checked;
            Plugin.SavedSettings.dontShowCAR = !showCARCheckBox.Checked;
            Plugin.SavedSettings.dontShowShowHiddenWindows = !showShowHiddenWindowsCheckBox.Checked;

            Plugin.SavedSettings.dontShowBackupRestore = !showBackupRestoreCheckBox.Checked;

            Plugin.SavedSettings.useSkinColors = useSkinColorsCheckBox.Checked;
            Plugin.SavedSettings.closeShowHiddenWindows = getCloseShowWindowsRadioButtons();

            Plugin.SavedSettings.dontPlayCompletedSound = !playCompletedSoundCheckBox.Checked;
            Plugin.SavedSettings.playStartedSound = playStartedSoundCheckBox.Checked;
            Plugin.SavedSettings.playCanceledSound = playStoppedSoundCheckBox.Checked;

            Plugin.SavedSettings.unitK = unitKBox.Text;
            Plugin.SavedSettings.unitM = unitMBox.Text;
            Plugin.SavedSettings.unitG = unitGBox.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
