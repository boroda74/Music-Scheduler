using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace MusicBeePlugin
{
    public partial class CalculateAverageAlbumRatingPlugin : PluginWindowTemplate
    {
        private string[] files = new string[0];

        public CalculateAverageAlbumRatingPlugin()
        {
            InitializeComponent();
        }

        public CalculateAverageAlbumRatingPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            calculateAlbumRatingAtStartUpCheckBox.Checked = Plugin.SavedSettings.calculateAlbumRatingAtStartUp;
            calculateAlbumRatingAtTagsChangedCheckBox.Checked = Plugin.SavedSettings.calculateAlbumRatingAtTagsChanged;
            notifyWhenCalculationCompletedCheckBox.Checked = Plugin.SavedSettings.notifyWhenCalculationCompleted;
            considerUnratedCheckBox.Checked = Plugin.SavedSettings.considerUnrated;

            Plugin.FillList(trackRatingTagList.Items);
            trackRatingTagList.Text = Plugin.SavedSettings.trackRatingTagName;

            Plugin.FillList(albumRatingTagList.Items);
            albumRatingTagList.Text = Plugin.SavedSettings.albumRatingTagName;
        }

        public void calculateAlbumRating()
        {
            List<string[]> tags = new List<string[]>();
            string[] row;
            string currentFile;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.carCommandSbText, true, fileCounter, files.Length, currentFile);

                row = new string[4];

                row[0] = TagToolsPlugin.getFileTag(currentFile, Plugin.MetaDataType.AlbumArtist);
                row[1] = TagToolsPlugin.getFileTag(currentFile, Plugin.MetaDataType.Album);
                row[2] = TagToolsPlugin.getFileTag(currentFile, Plugin.GetTagId(Plugin.SavedSettings.trackRatingTagName), false, true);
                row[3] = currentFile;

                tags.Add(row);
            }

            TagToolsPlugin.setStatusbarText(TagToolsPlugin.carCommandSbText + " (" + TagToolsPlugin.sbSorting + ")");

            Plugin.TextTableComparator textTableComparator = new Plugin.TextTableComparator();
            textTableComparator.tagCounterIndex = 2;
            tags.Sort(textTableComparator);

            string currentAlbumArtsist;
            string currentAlbum;

            string prevAlbumArtsist = "";
            string prevAlbum = "";
            int prevRow = 0;

            double sumRating;
            int numberOfTracks;
            double avgRating;

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentAlbumArtsist = tags[i][0];
                currentAlbum = tags[i][1];

                if (i == 0)
                {
                    prevAlbumArtsist = currentAlbumArtsist;
                    prevAlbum = currentAlbum;
                }

                if (prevAlbumArtsist != currentAlbumArtsist || prevAlbum != currentAlbum)
                {
                    sumRating = 0;
                    numberOfTracks = 0;

                    for (int j = prevRow; j < i; j++)
                    {
                        if ("" + tags[j][2] != "" || Plugin.SavedSettings.considerUnrated)
                        {
                            sumRating += Plugin.ConvertStrings(tags[j][2]).result1f;
                            numberOfTracks++;
                        }
                    }

                    if (numberOfTracks == 0)
                        avgRating = 0;
                    else
                        avgRating = Math.Round(sumRating / 10 / numberOfTracks) * 10;

                    for (int j = prevRow; j < i; j++)
                    {
                        currentFile = tags[j][3];

                        TagToolsPlugin.setFileTag(currentFile, Plugin.GetTagId(Plugin.SavedSettings.albumRatingTagName), avgRating.ToString(), true);
                        TagToolsPlugin.commitTagsToFile(currentFile, false, true);

                        TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.carCommandSbText, false, j, tags.Count, currentFile);
                    }

                    prevAlbumArtsist = currentAlbumArtsist;
                    prevAlbum = currentAlbum;
                    prevRow = i;
                }
            }

            sumRating = 0;
            numberOfTracks = 0;

            for (int j = prevRow; j < tags.Count; j++)
            {
                if ("" + tags[j][2] != "" || Plugin.SavedSettings.considerUnrated)
                {
                    sumRating += Plugin.ConvertStrings(tags[j][2]).result1f;
                    numberOfTracks++;
                }
            }

            if (numberOfTracks == 0)
                avgRating = 0;
            else
                avgRating = Math.Round(sumRating / 10 / numberOfTracks) * 10;

            for (int j = prevRow; j < tags.Count; j++)
            {
                currentFile = tags[j][3];

                TagToolsPlugin.setFileTag(currentFile, Plugin.GetTagId(Plugin.SavedSettings.albumRatingTagName), avgRating.ToString(), true);
                TagToolsPlugin.commitTagsToFile(currentFile, false, true);

                TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.carCommandSbText, false, j, tags.Count, currentFile);
            }

            TagToolsPlugin.refreshPanels(true);

            TagToolsPlugin.setStatusbarTextForFileOperations(TagToolsPlugin.carCommandSbText, false, tags.Count - 1, tags.Count, null, true);

            if (Plugin.SavedSettings.notifyWhenCalculationCompleted) MessageBox.Show(TagToolsPlugin.msgBackgroundTaskIsCompleted);
        }

        public void calculateAlbumRatingForDisplayedTracks()
        {
            if (MbApiInterface.Library_QueryFiles("domain=DisplayedFiles"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];

            switchOperation(calculateAlbumRating, (Button)buttonOK, TagToolsPlugin.emptyButton, buttonCancel);
        }

        public void calculateAlbumRatingForAllTracks()
        {
            if (MbApiInterface.Library_QueryFiles("domain=Library"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];

            switchOperation(calculateAlbumRating, TagToolsPlugin.emptyButton, TagToolsPlugin.emptyButton, TagToolsPlugin.emptyButton);
        }

        public static void CalculateAlbumRatingForAlbum(Plugin tagToolsPluginParam, string currentFile)
        {
            string[] localFiles;

            if (Plugin.MbApiInterface.Library_QueryFiles("domain=Library"))
                localFiles = Plugin.MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                localFiles = new string[0];

            string currentAlbumArtist = tagToolsPluginParam.getFileTag(currentFile, Plugin.MetaDataType.AlbumArtist);
            string currentAlbum = tagToolsPluginParam.getFileTag(currentFile, Plugin.MetaDataType.Album);

            List<string[]> tags = new List<string[]>();
            string[] row;
            string file;

            for (int fileCounter = 0; fileCounter < localFiles.Length; fileCounter++)
            {
                file = localFiles[fileCounter];

                string albumArtist = tagToolsPluginParam.getFileTag(file, Plugin.MetaDataType.AlbumArtist);
                string album = tagToolsPluginParam.getFileTag(file, Plugin.MetaDataType.Album);
                string rating = tagToolsPluginParam.getFileTag(file, Plugin.GetTagId(Plugin.SavedSettings.trackRatingTagName), false, true);

                if (currentAlbumArtist == albumArtist && currentAlbum == album)
                {
                    row = new string[4];

                    row[0] = albumArtist;
                    row[1] = album;
                    row[2] = rating;
                    row[3] = file;

                    tags.Add(row);
                }
            }


            double sumRating;
            int numberOfTracks;
            double avgRating;

            sumRating = 0;
            numberOfTracks = 0;

            for (int j = 0; j < tags.Count; j++)
            {
                if ("" + tags[j][2] != "" || Plugin.SavedSettings.considerUnrated)
                {
                    sumRating += Plugin.ConvertStrings(tags[j][2]).result1f;
                    numberOfTracks++;
                }
            }

            if (numberOfTracks == 0)
                avgRating = 0;
            else
                avgRating = Math.Round(sumRating / 10 / numberOfTracks) * 10;

            for (int j = 0; j < tags.Count; j++)
            {
                file = tags[j][3];

                tagToolsPluginParam.setFileTag(file, Plugin.GetTagId(Plugin.SavedSettings.albumRatingTagName), avgRating.ToString(), true);
                tagToolsPluginParam.commitTagsToFile(file, false, true);
            }

            tagToolsPluginParam.refreshPanels(true);
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.calculateAlbumRatingAtStartUp = calculateAlbumRatingAtStartUpCheckBox.Checked;
            Plugin.SavedSettings.calculateAlbumRatingAtTagsChanged = calculateAlbumRatingAtTagsChangedCheckBox.Checked;
            Plugin.SavedSettings.notifyWhenCalculationCompleted = notifyWhenCalculationCompletedCheckBox.Checked;
            Plugin.SavedSettings.considerUnrated = considerUnratedCheckBox.Checked;

            Plugin.SavedSettings.trackRatingTagName = trackRatingTagList.Text;
            Plugin.SavedSettings.albumRatingTagName = albumRatingTagList.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            calculateAlbumRatingForDisplayedTracks();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveSettings();
        }
    }
}
