﻿namespace MusicBeePlugin
{
    partial class AdvancedSearchAndReplacePlugin
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSearchAndReplacePlugin));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.buttonImportNew = new System.Windows.Forms.Button();
            this.buttonImportAll = new System.Windows.Forms.Button();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.buttonDownloadNew = new System.Windows.Forms.Button();
            this.buttonDownloadAll = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.clearSearchButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.customText4Label = new System.Windows.Forms.Label();
            this.customText2Label = new System.Windows.Forms.Label();
            this.customText4Box = new System.Windows.Forms.TextBox();
            this.customText2Box = new System.Windows.Forms.TextBox();
            this.customText3Label = new System.Windows.Forms.Label();
            this.labelTag6 = new System.Windows.Forms.Label();
            this.customText3Box = new System.Windows.Forms.TextBox();
            this.parameterTag6List = new System.Windows.Forms.ComboBox();
            this.labelTag5 = new System.Windows.Forms.Label();
            this.parameterTag5List = new System.Windows.Forms.ComboBox();
            this.labelTag4 = new System.Windows.Forms.Label();
            this.parameterTag4List = new System.Windows.Forms.ComboBox();
            this.customTextLabel = new System.Windows.Forms.Label();
            this.customTextBox = new System.Windows.Forms.TextBox();
            this.labelTag3 = new System.Windows.Forms.Label();
            this.parameterTag3List = new System.Windows.Forms.ComboBox();
            this.labelTag2 = new System.Windows.Forms.Label();
            this.parameterTag2List = new System.Windows.Forms.ComboBox();
            this.labelTag = new System.Windows.Forms.Label();
            this.parameterTagList = new System.Windows.Forms.ComboBox();
            this.playlistComboBox = new System.Windows.Forms.ComboBox();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.presetList = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();
            //MusicBee
            this.searchTextBox = (System.Windows.Forms.TextBox)MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customTextBox = (System.Windows.Forms.TextBox)MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText2Box = (System.Windows.Forms.TextBox)MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText3Box = (System.Windows.Forms.TextBox)MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText4Box = (System.Windows.Forms.TextBox)MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 1500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // buttonDeleteAll
            // 
            resources.ApplyResources(this.buttonDeleteAll, "buttonDeleteAll");
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.toolTip1.SetToolTip(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.ToolTip"));
            this.buttonDeleteAll.UseVisualStyleBackColor = true;
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // buttonImportNew
            // 
            resources.ApplyResources(this.buttonImportNew, "buttonImportNew");
            this.buttonImportNew.Name = "buttonImportNew";
            this.toolTip1.SetToolTip(this.buttonImportNew, resources.GetString("buttonImportNew.ToolTip"));
            this.buttonImportNew.UseVisualStyleBackColor = true;
            this.buttonImportNew.Click += new System.EventHandler(this.buttonImportNew_Click);
            // 
            // buttonImportAll
            // 
            resources.ApplyResources(this.buttonImportAll, "buttonImportAll");
            this.buttonImportAll.Name = "buttonImportAll";
            this.toolTip1.SetToolTip(this.buttonImportAll, resources.GetString("buttonImportAll.ToolTip"));
            this.buttonImportAll.UseVisualStyleBackColor = true;
            this.buttonImportAll.Click += new System.EventHandler(this.buttonImportAll_Click);
            // 
            // buttonSubmit
            // 
            resources.ApplyResources(this.buttonSubmit, "buttonSubmit");
            this.buttonSubmit.Name = "buttonSubmit";
            this.toolTip1.SetToolTip(this.buttonSubmit, resources.GetString("buttonSubmit.ToolTip"));
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // buttonDownloadNew
            // 
            resources.ApplyResources(this.buttonDownloadNew, "buttonDownloadNew");
            this.buttonDownloadNew.Name = "buttonDownloadNew";
            this.toolTip1.SetToolTip(this.buttonDownloadNew, resources.GetString("buttonDownloadNew.ToolTip"));
            this.buttonDownloadNew.UseVisualStyleBackColor = true;
            this.buttonDownloadNew.Click += new System.EventHandler(this.buttonDownloadNew_Click);
            // 
            // buttonDownloadAll
            // 
            resources.ApplyResources(this.buttonDownloadAll, "buttonDownloadAll");
            this.buttonDownloadAll.Name = "buttonDownloadAll";
            this.toolTip1.SetToolTip(this.buttonDownloadAll, resources.GetString("buttonDownloadAll.ToolTip"));
            this.buttonDownloadAll.UseVisualStyleBackColor = true;
            this.buttonDownloadAll.Click += new System.EventHandler(this.buttonDownloadAll_Click);
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Track,
            this.OriginalTag1,
            this.NewTag1,
            this.OriginalTag2,
            this.NewTag2,
            this.OriginalTag3,
            this.NewTag3,
            this.OriginalTag4,
            this.NewTag4,
            this.OriginalTag5,
            this.NewTag5});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // File
            // 
            this.File.FillWeight = 1F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.FillWeight = 75F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // OriginalTag1
            // 
            this.OriginalTag1.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag1, "OriginalTag1");
            this.OriginalTag1.Name = "OriginalTag1";
            // 
            // NewTag1
            // 
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag1.DefaultCellStyle = dataGridViewCellStyle1;
            this.NewTag1.FillWeight = 25F;
            resources.ApplyResources(this.NewTag1, "NewTag1");
            this.NewTag1.Name = "NewTag1";
            // 
            // OriginalTag2
            // 
            this.OriginalTag2.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag2, "OriginalTag2");
            this.OriginalTag2.Name = "OriginalTag2";
            // 
            // NewTag2
            // 
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag2.DefaultCellStyle = dataGridViewCellStyle2;
            this.NewTag2.FillWeight = 25F;
            resources.ApplyResources(this.NewTag2, "NewTag2");
            this.NewTag2.Name = "NewTag2";
            // 
            // OriginalTag3
            // 
            this.OriginalTag3.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag3, "OriginalTag3");
            this.OriginalTag3.Name = "OriginalTag3";
            // 
            // NewTag3
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag3.DefaultCellStyle = dataGridViewCellStyle3;
            this.NewTag3.FillWeight = 25F;
            resources.ApplyResources(this.NewTag3, "NewTag3");
            this.NewTag3.Name = "NewTag3";
            // 
            // OriginalTag4
            // 
            this.OriginalTag4.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag4, "OriginalTag4");
            this.OriginalTag4.Name = "OriginalTag4";
            // 
            // NewTag4
            // 
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag4.DefaultCellStyle = dataGridViewCellStyle4;
            this.NewTag4.FillWeight = 25F;
            resources.ApplyResources(this.NewTag4, "NewTag4");
            this.NewTag4.Name = "NewTag4";
            // 
            // OriginalTag5
            // 
            this.OriginalTag5.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag5, "OriginalTag5");
            this.OriginalTag5.Name = "OriginalTag5";
            // 
            // NewTag5
            // 
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag5.DefaultCellStyle = dataGridViewCellStyle5;
            this.NewTag5.FillWeight = 25F;
            resources.ApplyResources(this.NewTag5, "NewTag5");
            this.NewTag5.Name = "NewTag5";
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCopy
            // 
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCreate
            // 
            resources.ApplyResources(this.buttonCreate, "buttonCreate");
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonEdit
            // 
            resources.ApplyResources(this.buttonEdit, "buttonEdit");
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.clearSearchButton);
            this.panel1.Controls.Add(this.searchTextBox);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.customText4Label);
            this.panel1.Controls.Add(this.customText2Label);
            this.panel1.Controls.Add(this.customText4Box);
            this.panel1.Controls.Add(this.customText2Box);
            this.panel1.Controls.Add(this.customText3Label);
            this.panel1.Controls.Add(this.labelTag6);
            this.panel1.Controls.Add(this.customText3Box);
            this.panel1.Controls.Add(this.parameterTag6List);
            this.panel1.Controls.Add(this.labelTag5);
            this.panel1.Controls.Add(this.parameterTag5List);
            this.panel1.Controls.Add(this.labelTag4);
            this.panel1.Controls.Add(this.parameterTag4List);
            this.panel1.Controls.Add(this.customTextLabel);
            this.panel1.Controls.Add(this.customTextBox);
            this.panel1.Controls.Add(this.labelTag3);
            this.panel1.Controls.Add(this.parameterTag3List);
            this.panel1.Controls.Add(this.labelTag2);
            this.panel1.Controls.Add(this.parameterTag2List);
            this.panel1.Controls.Add(this.labelTag);
            this.panel1.Controls.Add(this.parameterTagList);
            this.panel1.Controls.Add(this.playlistComboBox);
            this.panel1.Controls.Add(this.conditionCheckBox);
            this.panel1.Controls.Add(this.presetList);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Name = "panel1";
            // 
            // clearSearchButton
            // 
            resources.ApplyResources(this.clearSearchButton, "clearSearchButton");
            this.clearSearchButton.Image = global::MusicBeePlugin.Properties.Resources.Xion20;
            this.clearSearchButton.Name = "clearSearchButton";
            this.clearSearchButton.UseVisualStyleBackColor = true;
            this.clearSearchButton.Click += new System.EventHandler(this.clearSearchButton_Click);
            // 
            // searchTextBox
            // 
            resources.ApplyResources(this.searchTextBox, "searchTextBox");
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MusicBeePlugin.Properties.Resources.Search;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.descriptionBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // descriptionBox
            // 
            resources.ApplyResources(this.descriptionBox, "descriptionBox");
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            // 
            // customText4Label
            // 
            resources.ApplyResources(this.customText4Label, "customText4Label");
            this.customText4Label.Name = "customText4Label";
            // 
            // customText2Label
            // 
            resources.ApplyResources(this.customText2Label, "customText2Label");
            this.customText2Label.Name = "customText2Label";
            // 
            // customText4Box
            // 
            resources.ApplyResources(this.customText4Box, "customText4Box");
            this.customText4Box.Name = "customText4Box";
            this.customText4Box.TextChanged += new System.EventHandler(this.customText4Box_TextChanged);
            // 
            // customText2Box
            // 
            resources.ApplyResources(this.customText2Box, "customText2Box");
            this.customText2Box.Name = "customText2Box";
            this.customText2Box.TextChanged += new System.EventHandler(this.customText2Box_TextChanged);
            // 
            // customText3Label
            // 
            resources.ApplyResources(this.customText3Label, "customText3Label");
            this.customText3Label.Name = "customText3Label";
            // 
            // labelTag6
            // 
            resources.ApplyResources(this.labelTag6, "labelTag6");
            this.labelTag6.Name = "labelTag6";
            // 
            // customText3Box
            // 
            resources.ApplyResources(this.customText3Box, "customText3Box");
            this.customText3Box.Name = "customText3Box";
            this.customText3Box.TextChanged += new System.EventHandler(this.customText3Box_TextChanged);
            // 
            // parameterTag6List
            // 
            resources.ApplyResources(this.parameterTag6List, "parameterTag6List");
            this.parameterTag6List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag6List.DropDownWidth = 250;
            this.parameterTag6List.FormattingEnabled = true;
            this.parameterTag6List.Name = "parameterTag6List";
            this.parameterTag6List.SelectedIndexChanged += new System.EventHandler(this.parameterTag6List_SelectedIndexChanged);
            // 
            // labelTag5
            // 
            resources.ApplyResources(this.labelTag5, "labelTag5");
            this.labelTag5.Name = "labelTag5";
            // 
            // parameterTag5List
            // 
            resources.ApplyResources(this.parameterTag5List, "parameterTag5List");
            this.parameterTag5List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag5List.DropDownWidth = 250;
            this.parameterTag5List.FormattingEnabled = true;
            this.parameterTag5List.Name = "parameterTag5List";
            this.parameterTag5List.SelectedIndexChanged += new System.EventHandler(this.parameterTag5List_SelectedIndexChanged);
            // 
            // labelTag4
            // 
            resources.ApplyResources(this.labelTag4, "labelTag4");
            this.labelTag4.Name = "labelTag4";
            // 
            // parameterTag4List
            // 
            resources.ApplyResources(this.parameterTag4List, "parameterTag4List");
            this.parameterTag4List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag4List.DropDownWidth = 250;
            this.parameterTag4List.FormattingEnabled = true;
            this.parameterTag4List.Name = "parameterTag4List";
            this.parameterTag4List.SelectedIndexChanged += new System.EventHandler(this.parameterTag4List_SelectedIndexChanged);
            // 
            // customTextLabel
            // 
            resources.ApplyResources(this.customTextLabel, "customTextLabel");
            this.customTextLabel.Name = "customTextLabel";
            // 
            // customTextBox
            // 
            resources.ApplyResources(this.customTextBox, "customTextBox");
            this.customTextBox.Name = "customTextBox";
            this.customTextBox.TextChanged += new System.EventHandler(this.customText_TextChanged);
            // 
            // labelTag3
            // 
            resources.ApplyResources(this.labelTag3, "labelTag3");
            this.labelTag3.Name = "labelTag3";
            // 
            // parameterTag3List
            // 
            resources.ApplyResources(this.parameterTag3List, "parameterTag3List");
            this.parameterTag3List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag3List.DropDownWidth = 250;
            this.parameterTag3List.FormattingEnabled = true;
            this.parameterTag3List.Name = "parameterTag3List";
            this.parameterTag3List.SelectedIndexChanged += new System.EventHandler(this.parameterTag3_SelectedIndexChanged);
            // 
            // labelTag2
            // 
            resources.ApplyResources(this.labelTag2, "labelTag2");
            this.labelTag2.Name = "labelTag2";
            // 
            // parameterTag2List
            // 
            resources.ApplyResources(this.parameterTag2List, "parameterTag2List");
            this.parameterTag2List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag2List.DropDownWidth = 250;
            this.parameterTag2List.FormattingEnabled = true;
            this.parameterTag2List.Name = "parameterTag2List";
            this.parameterTag2List.SelectedIndexChanged += new System.EventHandler(this.parameterTag2_SelectedIndexChanged);
            // 
            // labelTag
            // 
            resources.ApplyResources(this.labelTag, "labelTag");
            this.labelTag.Name = "labelTag";
            // 
            // parameterTagList
            // 
            resources.ApplyResources(this.parameterTagList, "parameterTagList");
            this.parameterTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTagList.DropDownWidth = 250;
            this.parameterTagList.FormattingEnabled = true;
            this.parameterTagList.Name = "parameterTagList";
            this.parameterTagList.SelectedIndexChanged += new System.EventHandler(this.parameterTag_SelectedIndexChanged);
            // 
            // playlistComboBox
            // 
            resources.ApplyResources(this.playlistComboBox, "playlistComboBox");
            this.playlistComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playlistComboBox.DropDownWidth = 250;
            this.playlistComboBox.FormattingEnabled = true;
            this.playlistComboBox.Name = "playlistComboBox";
            this.playlistComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistComboBox_SelectedIndexChanged);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // presetList
            // 
            resources.ApplyResources(this.presetList, "presetList");
            this.presetList.Name = "presetList";
            this.presetList.SelectedIndexChanged += new System.EventHandler(this.presetList_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.buttonDeleteAll);
            this.panel2.Controls.Add(this.previewTable);
            this.panel2.Controls.Add(this.buttonSave);
            this.panel2.Controls.Add(this.buttonPreview);
            this.panel2.Controls.Add(this.buttonImportNew);
            this.panel2.Controls.Add(this.buttonImportAll);
            this.panel2.Controls.Add(this.buttonCopy);
            this.panel2.Controls.Add(this.buttonSubmit);
            this.panel2.Controls.Add(this.buttonDownloadNew);
            this.panel2.Controls.Add(this.buttonDownloadAll);
            this.panel2.Controls.Add(this.buttonOK);
            this.panel2.Controls.Add(this.buttonCreate);
            this.panel2.Controls.Add(this.buttonEdit);
            this.panel2.Controls.Add(this.buttonDelete);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Name = "panel2";
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // AdvancedSearchAndReplacePlugin
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AdvancedSearchAndReplacePlugin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdvancedSearchAndReplacePlugin_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.Button buttonImportNew;
        private System.Windows.Forms.Button buttonImportAll;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.Button buttonDownloadNew;
        private System.Windows.Forms.Button buttonDownloadAll;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag2;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag3;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag4;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag4;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag5;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag5;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox descriptionBox;
        private System.Windows.Forms.Label customText4Label;
        private System.Windows.Forms.Label customText2Label;
        private System.Windows.Forms.TextBox customText4Box;
        private System.Windows.Forms.TextBox customText2Box;
        private System.Windows.Forms.Label customText3Label;
        private System.Windows.Forms.Label labelTag6;
        private System.Windows.Forms.TextBox customText3Box;
        private System.Windows.Forms.ComboBox parameterTag6List;
        private System.Windows.Forms.Label labelTag5;
        private System.Windows.Forms.ComboBox parameterTag5List;
        private System.Windows.Forms.Label labelTag4;
        private System.Windows.Forms.ComboBox parameterTag4List;
        private System.Windows.Forms.Label customTextLabel;
        private System.Windows.Forms.TextBox customTextBox;
        private System.Windows.Forms.Label labelTag3;
        private System.Windows.Forms.ComboBox parameterTag3List;
        private System.Windows.Forms.Label labelTag2;
        private System.Windows.Forms.ComboBox parameterTag2List;
        private System.Windows.Forms.Label labelTag;
        private System.Windows.Forms.ComboBox parameterTagList;
        private System.Windows.Forms.ComboBox playlistComboBox;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.CheckedListBox presetList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button clearSearchButton;
        private System.Windows.Forms.TextBox searchTextBox;



    }
}