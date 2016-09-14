using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class MultiplePastingQuestion : PluginWindowTemplate
    {
        private int _fileTagsLength = 0;
        private int _filesLength = 0;
        public bool PasteAnyway = false;

        public MultiplePastingQuestion()
        {
            InitializeComponent();
        }

        public MultiplePastingQuestion(Plugin tagToolsPluginParam, int fileTagsLength, int filesLength)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
            _fileTagsLength = fileTagsLength;
            _filesLength = filesLength;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            label1.Text = TagToolsPlugin.msgNumberOfTracksInClipboard + _fileTagsLength + TagToolsPlugin.msgDoesntCorrespondToNumberOfCopiedTagsC + _filesLength;
            label1.Text += TagToolsPlugin.msgMessageEndC + TagToolsPlugin.msgDoYouWantToPasteAnyway;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PasteAnyway = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
