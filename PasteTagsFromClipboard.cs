using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class PasteTagsFromClipboardPlugin : PluginWindowTemplate
    {
        private List<string> destinationTagNames = new List<string>();

        public PasteTagsFromClipboardPlugin()
        {
            InitializeComponent();
        }

        public PasteTagsFromClipboardPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillList(destinationTagNames, false, false, false);
        }

        private bool pasteTagsFromClipboard()
        {
            string[] files;

            if (MbApiInterface.Library_QueryFiles("domain=SelectedFiles"))
                files = MbApiInterface.Library_QueryGetAllFiles().Split(Plugin.FilesSeparators, StringSplitOptions.RemoveEmptyEntries);
            else
                files = new string[0];


            if (files.Length == 0)
            {
                MessageBox.Show(TagToolsPlugin.msgNoFilesSelected);
                return false;
            }


            if (!Clipboard.ContainsText())
            {
                MessageBox.Show(TagToolsPlugin.msgClipboardDesntContainText);
                return false;
            }

            string[] fileTags = Clipboard.GetText().Split(new string[] { "\n" }, StringSplitOptions.None);

            bool multiplePasting = false;
            if (fileTags.Length == 1 && files.Length > 1)
            {
                MultiplePastingQuestion question = new MultiplePastingQuestion(TagToolsPlugin, fileTags.Length, files.Length);
                question.ShowDialog();

                if (question.PasteAnyway)
                    multiplePasting = true;
                else
                    return false;

            }
            else if (fileTags.Length != files.Length)
            {
                MessageBox.Show(TagToolsPlugin.msgNumberOfTracksInClipboard + fileTags.Length + TagToolsPlugin.msgDoesntCorrespondToNumberOfSelectedTracksC + files.Length + TagToolsPlugin.msgMessageEndC);
                return false;
            }


            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string[] tags = fileTags[multiplePasting ? 0 : i].Split(new string[] { "\t" }, StringSplitOptions.None);

                if (tags.Length != Plugin.SavedSettings.copyTagsSourceTagIds.Length)
                {
                    MessageBox.Show(TagToolsPlugin.msgNumberOfTagsInClipboard + tags.Length + TagToolsPlugin.msgDoesntCorrespondToNumberOfCopiedTagsC + Plugin.SavedSettings.copyTagsSourceTagIds.Length + TagToolsPlugin.msgMessageEndC);
                    return false;
                }

                for (int j = 0; j < Plugin.SavedSettings.copyTagsSourceTagIds.Length; j++)
                {
                    TagToolsPlugin.setFileTag(file, (Plugin.MetaDataType)Plugin.SavedSettings.copyTagsSourceTagIds[j], tags[j].Replace("\uFFFF", "\u0000").Replace("\u0007", "\u000D").Replace("\u0008", "\u000A"));
                }
                TagToolsPlugin.commitTagsToFile(file);
            }

            TagToolsPlugin.refreshPanels(true);

            return true;
        }

        private void PasteTagsFromClipboardPlugin_Shown(object sender, EventArgs e)
        {
            pasteTagsFromClipboard();
            Close();
        }
    }
}
