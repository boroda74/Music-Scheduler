namespace MusicBeePlugin
{
    partial class TasksForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TasksForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.Play = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.checkBoxWakeupPCOnAutostart = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonStopAll = new System.Windows.Forms.Button();
            this.stopAfterCurrentCheckBox = new System.Windows.Forms.CheckBox();
            this.skinColorsCheckBox = new System.Windows.Forms.CheckBox();
            this.closeMbCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.preventMonitorSleepCheckBox = new System.Windows.Forms.CheckBox();
            this.preventPcSleepCheckBox = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButtonOn = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonShutdown = new System.Windows.Forms.RadioButton();
            this.radioButtonHibernate = new System.Windows.Forms.RadioButton();
            this.radioButtonSleep = new System.Windows.Forms.RadioButton();
            this.buttonApply = new System.Windows.Forms.Button();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Info;
            this.previewTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Play,
            this.Task,
            this.Order});
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            this.previewTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellDoubleClick);
            this.previewTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellEndEdit);
            this.previewTable.Sorted += new System.EventHandler(this.previewTable_Sorted);
            // 
            // Play
            // 
            this.Play.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Play.FalseValue = "False";
            this.Play.FillWeight = 15.79214F;
            resources.ApplyResources(this.Play, "Play");
            this.Play.Name = "Play";
            this.Play.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Play.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Play.TrueValue = "True";
            // 
            // Task
            // 
            this.Task.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Task.FillWeight = 2.133126F;
            resources.ApplyResources(this.Task, "Task");
            this.Task.Name = "Task";
            this.Task.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Order
            // 
            resources.ApplyResources(this.Order, "Order");
            this.Order.Name = "Order";
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.toolTip1.SetToolTip(this.buttonAdd, resources.GetString("buttonAdd.ToolTip"));
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.toolTip1.SetToolTip(this.buttonDelete, resources.GetString("buttonDelete.ToolTip"));
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUp
            // 
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.buttonUp.Name = "buttonUp";
            this.toolTip1.SetToolTip(this.buttonUp, resources.GetString("buttonUp.ToolTip"));
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            resources.ApplyResources(this.buttonDown, "buttonDown");
            this.buttonDown.Name = "buttonDown";
            this.toolTip1.SetToolTip(this.buttonDown, resources.GetString("buttonDown.ToolTip"));
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonEdit
            // 
            resources.ApplyResources(this.buttonEdit, "buttonEdit");
            this.buttonEdit.Name = "buttonEdit";
            this.toolTip1.SetToolTip(this.buttonEdit, resources.GetString("buttonEdit.ToolTip"));
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // checkBoxWakeupPCOnAutostart
            // 
            resources.ApplyResources(this.checkBoxWakeupPCOnAutostart, "checkBoxWakeupPCOnAutostart");
            this.checkBoxWakeupPCOnAutostart.Name = "checkBoxWakeupPCOnAutostart";
            this.toolTip1.SetToolTip(this.checkBoxWakeupPCOnAutostart, resources.GetString("checkBoxWakeupPCOnAutostart.ToolTip"));
            this.checkBoxWakeupPCOnAutostart.UseVisualStyleBackColor = true;
            // 
            // buttonStopAll
            // 
            resources.ApplyResources(this.buttonStopAll, "buttonStopAll");
            this.buttonStopAll.Name = "buttonStopAll";
            this.toolTip1.SetToolTip(this.buttonStopAll, resources.GetString("buttonStopAll.ToolTip"));
            this.buttonStopAll.UseVisualStyleBackColor = true;
            this.buttonStopAll.Click += new System.EventHandler(this.buttonStopAll_Click);
            // 
            // stopAfterCurrentCheckBox
            // 
            resources.ApplyResources(this.stopAfterCurrentCheckBox, "stopAfterCurrentCheckBox");
            this.stopAfterCurrentCheckBox.Name = "stopAfterCurrentCheckBox";
            this.toolTip1.SetToolTip(this.stopAfterCurrentCheckBox, resources.GetString("stopAfterCurrentCheckBox.ToolTip"));
            this.stopAfterCurrentCheckBox.UseVisualStyleBackColor = true;
            // 
            // skinColorsCheckBox
            // 
            resources.ApplyResources(this.skinColorsCheckBox, "skinColorsCheckBox");
            this.skinColorsCheckBox.Name = "skinColorsCheckBox";
            this.toolTip1.SetToolTip(this.skinColorsCheckBox, resources.GetString("skinColorsCheckBox.ToolTip"));
            this.skinColorsCheckBox.UseVisualStyleBackColor = true;
            // 
            // closeMbCheckBox
            // 
            resources.ApplyResources(this.closeMbCheckBox, "closeMbCheckBox");
            this.closeMbCheckBox.Name = "closeMbCheckBox";
            this.toolTip1.SetToolTip(this.closeMbCheckBox, resources.GetString("closeMbCheckBox.ToolTip"));
            this.closeMbCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.preventMonitorSleepCheckBox);
            this.groupBox1.Controls.Add(this.preventPcSleepCheckBox);
            this.groupBox1.Controls.Add(this.stopAfterCurrentCheckBox);
            this.groupBox1.Controls.Add(this.closeMbCheckBox);
            this.groupBox1.Controls.Add(this.skinColorsCheckBox);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.checkBoxWakeupPCOnAutostart);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // preventMonitorSleepCheckBox
            // 
            resources.ApplyResources(this.preventMonitorSleepCheckBox, "preventMonitorSleepCheckBox");
            this.preventMonitorSleepCheckBox.Name = "preventMonitorSleepCheckBox";
            this.toolTip1.SetToolTip(this.preventMonitorSleepCheckBox, resources.GetString("preventMonitorSleepCheckBox.ToolTip"));
            this.preventMonitorSleepCheckBox.UseVisualStyleBackColor = true;
            // 
            // preventPcSleepCheckBox
            // 
            resources.ApplyResources(this.preventPcSleepCheckBox, "preventPcSleepCheckBox");
            this.preventPcSleepCheckBox.Name = "preventPcSleepCheckBox";
            this.toolTip1.SetToolTip(this.preventPcSleepCheckBox, resources.GetString("preventPcSleepCheckBox.ToolTip"));
            this.preventPcSleepCheckBox.UseVisualStyleBackColor = true;
            this.preventPcSleepCheckBox.CheckedChanged += new System.EventHandler(this.preventPcSleepCheckBox_CheckedChanged);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.radioButtonOn);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.radioButtonShutdown);
            this.panel2.Controls.Add(this.radioButtonHibernate);
            this.panel2.Controls.Add(this.radioButtonSleep);
            this.panel2.Name = "panel2";
            this.toolTip1.SetToolTip(this.panel2, resources.GetString("panel2.ToolTip"));
            // 
            // radioButtonOn
            // 
            resources.ApplyResources(this.radioButtonOn, "radioButtonOn");
            this.radioButtonOn.Name = "radioButtonOn";
            this.radioButtonOn.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButtonOn, resources.GetString("radioButtonOn.ToolTip"));
            this.radioButtonOn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // radioButtonShutdown
            // 
            resources.ApplyResources(this.radioButtonShutdown, "radioButtonShutdown");
            this.radioButtonShutdown.Name = "radioButtonShutdown";
            this.radioButtonShutdown.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButtonShutdown, resources.GetString("radioButtonShutdown.ToolTip"));
            this.radioButtonShutdown.UseVisualStyleBackColor = true;
            // 
            // radioButtonHibernate
            // 
            resources.ApplyResources(this.radioButtonHibernate, "radioButtonHibernate");
            this.radioButtonHibernate.Name = "radioButtonHibernate";
            this.radioButtonHibernate.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButtonHibernate, resources.GetString("radioButtonHibernate.ToolTip"));
            this.radioButtonHibernate.UseVisualStyleBackColor = true;
            // 
            // radioButtonSleep
            // 
            resources.ApplyResources(this.radioButtonSleep, "radioButtonSleep");
            this.radioButtonSleep.Name = "radioButtonSleep";
            this.radioButtonSleep.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButtonSleep, resources.GetString("radioButtonSleep.ToolTip"));
            this.radioButtonSleep.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            resources.ApplyResources(this.buttonApply, "buttonApply");
            this.buttonApply.Name = "buttonApply";
            this.toolTip1.SetToolTip(this.buttonApply, resources.GetString("buttonApply.ToolTip"));
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.FalseValue = "False";
            this.dataGridViewCheckBoxColumn1.FillWeight = 15.79214F;
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn1.TrueValue = "True";
            // 
            // TasksForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonStopAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "TasksForm";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.CheckBox checkBoxWakeupPCOnAutostart;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonShutdown;
        private System.Windows.Forms.RadioButton radioButtonHibernate;
        private System.Windows.Forms.RadioButton radioButtonSleep;
        private System.Windows.Forms.Button buttonStopAll;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.CheckBox skinColorsCheckBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Play;
        private System.Windows.Forms.DataGridViewTextBoxColumn Task;
        private System.Windows.Forms.DataGridViewTextBoxColumn Order;
        private System.Windows.Forms.RadioButton radioButtonOn;
        private System.Windows.Forms.CheckBox closeMbCheckBox;
        private System.Windows.Forms.CheckBox stopAfterCurrentCheckBox;
        private System.Windows.Forms.CheckBox preventMonitorSleepCheckBox;
        private System.Windows.Forms.CheckBox preventPcSleepCheckBox;
    }
}