using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class CopyTagsToClipboardPlugin : PluginWindowTemplate
    {
        public CopyTagsToClipboardPlugin()
        {
            InitializeComponent();
        }

        public CopyTagsToClipboardPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillList(sourceTagList.Items, true, true, false);
            Plugin.FillListWithProps(sourceTagList.Items);

            if (Plugin.SavedSettings.copyTagsSourceTagIds == null)
            {
                for (int i = 0; i < sourceTagList.Items.Count; i++)
                    if ((string)sourceTagList.Items[i] != "Sort Artist" && (string)sourceTagList.Items[i] != "Sort Album Artist")
                        if (!Plugin.PropNamesIds.ContainsKey((string)sourceTagList.Items[i]))
                            sourceTagList.SetItemChecked(i, true);
            }
            else
            {
                for (int i = 0; i < Plugin.SavedSettings.copyTagsSourceTagIds.Length; i++)
                {
                    for (int j = 0; j < sourceTagList.Items.Count; j++)
                    {
                        string tagName = Plugin.GetTagName((Plugin.MetaDataType)Plugin.SavedSettings.copyTagsSourceTagIds[i]);

                        if ((string)sourceTagList.Items[j] == tagName)
                        {
                            sourceTagList.SetItemChecked(j, true);
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < sourceTagList.Items.Count; )
                if (sourceTagList.GetItemChecked(i))
                    sourceTagList.SelectedIndex = i;
                else
                    i++;
        }

        private bool copyTagsToClipboard()
        {
            string[] files = new string[0];

            if (MbApiInterface.Library_QueryFiles("domain=SelectedFiles"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];


            if (files.Length == 0)
            {
                MessageBox.Show(TagToolsPlugin.msgNoFilesSelected);
                return false;
            }


            string clipboardText = "";
            foreach (string file in files)
            {
                string tags = "";
                for (int i = 0; i < Plugin.SavedSettings.copyTagsSourceTagIds.Length; i++)
                    tags += (TagToolsPlugin.getFileTag(file, (Plugin.MetaDataType)Plugin.SavedSettings.copyTagsSourceTagIds[i])).Replace("\u0000", "\uFFFF").Replace("\u000D", "\u0007").Replace("\u000A", "\u0008") + "\t";

                tags = tags.Remove(tags.Length - 1);

                clipboardText += tags + "\n";
            }

            clipboardText = clipboardText.Remove(clipboardText.Length - 1);

            if (clipboardText == "")
                clipboardText = "\u0000";


            System.Threading.Thread thread = new System.Threading.Thread(() => Clipboard.SetText(clipboardText));
            thread.SetApartmentState(System.Threading.ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();


            return true;
        }

        private void saveSettings()
        {
            int displayedArtistOffset = 0;
            int displayedComposerOffset = 0;
            List<int> checkedIds = new List<int>();

            for (int i = 0; i < checkedSourceTagList.Items.Count; i++)
            {
                int id = (int)Plugin.GetTagId((string)checkedSourceTagList.Items[i]);

                checkedIds.Add(id);

                if ((string)checkedSourceTagList.Items[i] == Plugin.DisplayedArtistName) //Displayed artist should be copied to clipboard the first or second
                {
                    if (displayedArtistOffset == 0)
                        displayedComposerOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedArtistOffset];
                    checkedIds[displayedArtistOffset] = id;
                }
                else if ((string)checkedSourceTagList.Items[i] == Plugin.DisplayedComposerName) //Displayed composer should be copied to clipboard the first or second
                {
                    if (displayedComposerOffset == 0)
                        displayedArtistOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedComposerOffset];
                    checkedIds[displayedComposerOffset] = id;
                }
            }

            Plugin.SavedSettings.copyTagsSourceTagIds = new int[checkedIds.Count];
            checkedIds.CopyTo(Plugin.SavedSettings.copyTagsSourceTagIds, 0);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            copyTagsToClipboard();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedSourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedSourceTagList.SelectedIndex != -1)
            {
                sourceTagList.Items.Add(checkedSourceTagList.Items[checkedSourceTagList.SelectedIndex]);
                checkedSourceTagList.Items.RemoveAt(checkedSourceTagList.SelectedIndex);
            }

            if (checkedSourceTagList.Items.Count == 0)
                buttonOK.Enabled = false;
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceTagList.SelectedIndex != -1)
            {
                checkedSourceTagList.Items.Add(sourceTagList.Items[sourceTagList.SelectedIndex]);

                for (int i = 0; i < checkedSourceTagList.Items.Count; i++)
                    checkedSourceTagList.SetItemChecked(i, true);

                sourceTagList.Items.RemoveAt(sourceTagList.SelectedIndex);
            }

            if (checkedSourceTagList.Items.Count > 0)
                buttonOK.Enabled = true;
        }
    }
}
