using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class SwapTagsPlugin : PluginWindowTemplate
    {
        private Plugin.MetaDataType sourceTagId;
        private Plugin.MetaDataType destinationTagId;
        private string[] files = new string[0];

        public SwapTagsPlugin()
        {
            InitializeComponent();
        }

        public SwapTagsPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillList(sourceTagList.Items);
            sourceTagList.Text = Plugin.SavedSettings.swapTagsSourceTagName;

            Plugin.FillList(destinationTagList.Items);
            destinationTagList.Text = Plugin.SavedSettings.swapTagsDestinationTagName;

            smartOperationCheckBox.Checked = Plugin.SavedSettings.smartOperation;
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            sourceTagId = Plugin.GetTagId(sourceTagList.Text);
            destinationTagId = Plugin.GetTagId(destinationTagList.Text);

            if (MbApiInterface.Library_QueryFiles("domain=SelectedFiles"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(TagToolsPlugin.msgNoFilesSelected);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void swapTags()
        {
            string currentFile;
            string sourceTagValue;
            string destinationTagValue;
            Plugin.SwappedTags swappedTags;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.swapTagsCommandSbText, false, fileCounter, files.Length, currentFile);

                sourceTagValue = TagToolsPlugin.getFileTag(currentFile, sourceTagId);
                destinationTagValue = TagToolsPlugin.getFileTag(currentFile, destinationTagId);

                swappedTags = TagToolsPlugin.swapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId, smartOperationCheckBox.Checked);

                if (sourceTagId != destinationTagId)
                    TagToolsPlugin.setFileTag(currentFile, destinationTagId, swappedTags.newDestinationTagValue);

                TagToolsPlugin.setFileTag(currentFile, sourceTagId, swappedTags.newSourceTagValue);
                TagToolsPlugin.commitTagsToFile(currentFile);
            }

            TagToolsPlugin.refreshPanels(true);

            TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.swapTagsCommandSbText, false, files.Length - 1, files.Length, null, true);
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.swapTagsSourceTagName = sourceTagList.Text;
            Plugin.SavedSettings.swapTagsDestinationTagName = destinationTagList.Text;
            Plugin.SavedSettings.smartOperation = smartOperationCheckBox.Checked;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagList.Text == destinationTagList.Text)
                if(!smartOperationCheckBox.Checked || !(Plugin.GetTagId(sourceTagList.Text) == Plugin.ArtistArtistsId || Plugin.GetTagId(sourceTagList.Text) == Plugin.ComposerComposersId))
                {
                    MessageBox.Show(TagToolsPlugin.msgSwapTagsSourceAndDestinationTagsAreTheSame);
                    return;
                }

            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(swapTags, (Button)sender, TagToolsPlugin.emptyButton, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonOK, " ");
            dirtyErrorProvider.SetError(buttonOK, String.Empty);
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
        }
    }
}
