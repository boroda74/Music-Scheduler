namespace MusicBeePlugin
{
    partial class LibraryReportsPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryReportsPlugin));
            this.sourceTagList = new System.Windows.Forms.CheckedListBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.destinationTagList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.conditionList = new System.Windows.Forms.ComboBox();
            this.conditionFieldList = new System.Windows.Forms.ComboBox();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonUncheckAll = new System.Windows.Forms.Button();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comparedFieldList = new System.Windows.Forms.ComboBox();
            this.presetsBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.functionComboBox = new System.Windows.Forms.ComboBox();
            this.totalsCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.parameter2ComboBox = new System.Windows.Forms.ComboBox();
            this.resizeArtworkCheckBox = new System.Windows.Forms.CheckBox();
            this.xArworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.yArworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xArworkSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yArworkSizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.CheckOnClick = true;
            this.dirtyErrorProvider.SetError(this.sourceTagList, resources.GetString("sourceTagList.Error"));
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceTagList, ((int)(resources.GetObject("sourceTagList.IconPadding"))));
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceTagList_ItemCheck);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.dirtyErrorProvider.SetError(this.buttonSave, resources.GetString("buttonSave.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSave, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSave.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSave, ((int)(resources.GetObject("buttonSave.IconPadding"))));
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // destinationTagList
            // 
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.destinationTagList, resources.GetString("destinationTagList.Error"));
            this.destinationTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.destinationTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("destinationTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.destinationTagList, ((int)(resources.GetObject("destinationTagList.IconPadding"))));
            this.destinationTagList.Name = "destinationTagList";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.dirtyErrorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // conditionList
            // 
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.conditionList, resources.GetString("conditionList.Error"));
            this.conditionList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionList, ((int)(resources.GetObject("conditionList.IconPadding"))));
            this.conditionList.Name = "conditionList";
            // 
            // conditionFieldList
            // 
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.conditionFieldList, resources.GetString("conditionFieldList.Error"));
            this.conditionFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionFieldList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionFieldList, ((int)(resources.GetObject("conditionFieldList.IconPadding"))));
            this.conditionFieldList.Name = "conditionFieldList";
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.dirtyErrorProvider.SetError(this.conditionCheckBox, resources.GetString("conditionCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBox, ((int)(resources.GetObject("conditionCheckBox.IconPadding"))));
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.checkBoxCondition_CheckedChanged);
            // 
            // buttonUncheckAll
            // 
            resources.ApplyResources(this.buttonUncheckAll, "buttonUncheckAll");
            this.dirtyErrorProvider.SetError(this.buttonUncheckAll, resources.GetString("buttonUncheckAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUncheckAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUncheckAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUncheckAll, ((int)(resources.GetObject("buttonUncheckAll.IconPadding"))));
            this.buttonUncheckAll.Name = "buttonUncheckAll";
            this.buttonUncheckAll.UseVisualStyleBackColor = true;
            this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
            // 
            // buttonCheckAll
            // 
            resources.ApplyResources(this.buttonCheckAll, "buttonCheckAll");
            this.dirtyErrorProvider.SetError(this.buttonCheckAll, resources.GetString("buttonCheckAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCheckAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCheckAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCheckAll, ((int)(resources.GetObject("buttonCheckAll.IconPadding"))));
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewList_DataError);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonCancel, resources.GetString("buttonCancel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCancel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCancel, ((int)(resources.GetObject("buttonCancel.IconPadding"))));
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // comparedFieldList
            // 
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.comparedFieldList, resources.GetString("comparedFieldList.Error"));
            this.comparedFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.comparedFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comparedFieldList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.comparedFieldList, ((int)(resources.GetObject("comparedFieldList.IconPadding"))));
            this.comparedFieldList.Name = "comparedFieldList";
            // 
            // presetsBox
            // 
            resources.ApplyResources(this.presetsBox, "presetsBox");
            this.presetsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetsBox.DropDownWidth = 1000;
            this.dirtyErrorProvider.SetError(this.presetsBox, resources.GetString("presetsBox.Error"));
            this.presetsBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.presetsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetsBox, ((int)(resources.GetObject("presetsBox.IconPadding"))));
            this.presetsBox.Name = "presetsBox";
            this.presetsBox.SelectedValueChanged += new System.EventHandler(this.presetsBox_SelectedValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.dirtyErrorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.dirtyErrorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // fieldComboBox
            // 
            resources.ApplyResources(this.fieldComboBox, "fieldComboBox");
            this.fieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fieldComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.fieldComboBox, resources.GetString("fieldComboBox.Error"));
            this.fieldComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.fieldComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fieldComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.fieldComboBox, ((int)(resources.GetObject("fieldComboBox.IconPadding"))));
            this.fieldComboBox.Name = "fieldComboBox";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.dirtyErrorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // functionComboBox
            // 
            resources.ApplyResources(this.functionComboBox, "functionComboBox");
            this.functionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.functionComboBox, resources.GetString("functionComboBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.functionComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.functionComboBox, ((int)(resources.GetObject("functionComboBox.IconPadding"))));
            this.functionComboBox.Name = "functionComboBox";
            this.functionComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // totalsCheckBox
            // 
            resources.ApplyResources(this.totalsCheckBox, "totalsCheckBox");
            this.dirtyErrorProvider.SetError(this.totalsCheckBox, resources.GetString("totalsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.totalsCheckBox, ((int)(resources.GetObject("totalsCheckBox.IconPadding"))));
            this.totalsCheckBox.Name = "totalsCheckBox";
            this.totalsCheckBox.UseVisualStyleBackColor = true;
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.dirtyErrorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // parameter2ComboBox
            // 
            resources.ApplyResources(this.parameter2ComboBox, "parameter2ComboBox");
            this.parameter2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.parameter2ComboBox, resources.GetString("parameter2ComboBox.Error"));
            this.parameter2ComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2ComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2ComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameter2ComboBox, ((int)(resources.GetObject("parameter2ComboBox.IconPadding"))));
            this.parameter2ComboBox.Name = "parameter2ComboBox";
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // resizeArtworkCheckBox
            // 
            resources.ApplyResources(this.resizeArtworkCheckBox, "resizeArtworkCheckBox");
            this.dirtyErrorProvider.SetError(this.resizeArtworkCheckBox, resources.GetString("resizeArtworkCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.resizeArtworkCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resizeArtworkCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.resizeArtworkCheckBox, ((int)(resources.GetObject("resizeArtworkCheckBox.IconPadding"))));
            this.resizeArtworkCheckBox.Name = "resizeArtworkCheckBox";
            this.resizeArtworkCheckBox.UseVisualStyleBackColor = true;
            this.resizeArtworkCheckBox.CheckedChanged += new System.EventHandler(this.resizeArtworkCheckBox_CheckedChanged);
            // 
            // xArworkSizeUpDown
            // 
            resources.ApplyResources(this.xArworkSizeUpDown, "xArworkSizeUpDown");
            this.dirtyErrorProvider.SetError(this.xArworkSizeUpDown, resources.GetString("xArworkSizeUpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.xArworkSizeUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xArworkSizeUpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.xArworkSizeUpDown, ((int)(resources.GetObject("xArworkSizeUpDown.IconPadding"))));
            this.xArworkSizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.xArworkSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.xArworkSizeUpDown.Name = "xArworkSizeUpDown";
            this.xArworkSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.xArworkSizeUpDown.ValueChanged += new System.EventHandler(this.xArworkSizeUpDown_ValueChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.dirtyErrorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // yArworkSizeUpDown
            // 
            resources.ApplyResources(this.yArworkSizeUpDown, "yArworkSizeUpDown");
            this.dirtyErrorProvider.SetError(this.yArworkSizeUpDown, resources.GetString("yArworkSizeUpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.yArworkSizeUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("yArworkSizeUpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.yArworkSizeUpDown, ((int)(resources.GetObject("yArworkSizeUpDown.IconPadding"))));
            this.yArworkSizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.yArworkSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.yArworkSizeUpDown.Name = "yArworkSizeUpDown";
            this.yArworkSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.yArworkSizeUpDown.ValueChanged += new System.EventHandler(this.yArworkSizeUpDown_ValueChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.dirtyErrorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // LibraryReportsPlugin
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.yArworkSizeUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.xArworkSizeUpDown);
            this.Controls.Add(this.resizeArtworkCheckBox);
            this.Controls.Add(this.parameter2ComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.totalsCheckBox);
            this.Controls.Add(this.functionComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fieldComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.presetsBox);
            this.Controls.Add(this.comparedFieldList);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.destinationTagList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.conditionList);
            this.Controls.Add(this.conditionFieldList);
            this.Controls.Add(this.conditionCheckBox);
            this.Controls.Add(this.buttonUncheckAll);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.buttonCheckAll);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Name = "LibraryReportsPlugin";
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xArworkSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yArworkSizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.CheckedListBox sourceTagList;
        private System.Windows.Forms.Button buttonUncheckAll;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.ComboBox conditionFieldList;
        private System.Windows.Forms.ComboBox conditionList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ComboBox comparedFieldList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox presetsBox;
        private System.Windows.Forms.ComboBox fieldComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox functionComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox totalsCheckBox;
        private System.Windows.Forms.ComboBox parameter2ComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown yArworkSizeUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown xArworkSizeUpDown;
        private System.Windows.Forms.CheckBox resizeArtworkCheckBox;
    }
}