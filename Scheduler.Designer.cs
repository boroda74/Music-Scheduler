namespace MusicBeePlugin
{
    partial class SchedulerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchedulerForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Play = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.WakeUp = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StartDateCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTimeCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Playlist = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EndDateCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTimeCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxDurationCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TurnOff = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.previewTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Play,
            this.WakeUp,
            this.StartDateCheckbox,
            this.StartDate,
            this.StartTimeCheckbox,
            this.StartTime,
            this.Playlist,
            this.EndDateCheckbox,
            this.EndDate,
            this.EndTimeCheckbox,
            this.EndTime,
            this.MaxDurationCheckbox,
            this.Duration,
            this.TurnOff,
            this.Order});
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            this.previewTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellDoubleClick);
            this.previewTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellValueChanged);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            this.previewTable.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.previewTable_SortCompare);
            this.previewTable.Sorted += new System.EventHandler(this.previewTable_Sorted);
            // 
            // buttonDown
            // 
            resources.ApplyResources(this.buttonDown, "buttonDown");
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.FillWeight = 15.79214F;
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn2.FalseValue = "False";
            this.dataGridViewCheckBoxColumn2.FillWeight = 15.85654F;
            resources.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn2.TrueValue = "True";
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
            // WakeUp
            // 
            this.WakeUp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.WakeUp.FalseValue = "False";
            this.WakeUp.FillWeight = 15.85654F;
            resources.ApplyResources(this.WakeUp, "WakeUp");
            this.WakeUp.Name = "WakeUp";
            this.WakeUp.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.WakeUp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.WakeUp.TrueValue = "True";
            // 
            // StartDateCheckbox
            // 
            this.StartDateCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StartDateCheckbox.FalseValue = "False";
            resources.ApplyResources(this.StartDateCheckbox, "StartDateCheckbox");
            this.StartDateCheckbox.Name = "StartDateCheckbox";
            this.StartDateCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StartDateCheckbox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StartDateCheckbox.TrueValue = "True";
            // 
            // StartDate
            // 
            this.StartDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.StartDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.StartDate.FillWeight = 93.35762F;
            resources.ApplyResources(this.StartDate, "StartDate");
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            this.StartDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // StartTimeCheckbox
            // 
            this.StartTimeCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StartTimeCheckbox.FalseValue = "False";
            resources.ApplyResources(this.StartTimeCheckbox, "StartTimeCheckbox");
            this.StartTimeCheckbox.Name = "StartTimeCheckbox";
            this.StartTimeCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StartTimeCheckbox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StartTimeCheckbox.TrueValue = "True";
            // 
            // StartTime
            // 
            this.StartTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.StartTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.StartTime.FillWeight = 75.82898F;
            resources.ApplyResources(this.StartTime, "StartTime");
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Playlist
            // 
            this.Playlist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Playlist.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.Playlist.FillWeight = 2.133126F;
            resources.ApplyResources(this.Playlist, "Playlist");
            this.Playlist.Name = "Playlist";
            this.Playlist.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EndDateCheckbox
            // 
            this.EndDateCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EndDateCheckbox.FalseValue = "False";
            resources.ApplyResources(this.EndDateCheckbox, "EndDateCheckbox");
            this.EndDateCheckbox.Name = "EndDateCheckbox";
            this.EndDateCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EndDateCheckbox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EndDateCheckbox.TrueValue = "True";
            // 
            // EndDate
            // 
            this.EndDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.EndDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.EndDate.FillWeight = 221.3507F;
            resources.ApplyResources(this.EndDate, "EndDate");
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            this.EndDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // EndTimeCheckbox
            // 
            this.EndTimeCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EndTimeCheckbox.FalseValue = "False";
            resources.ApplyResources(this.EndTimeCheckbox, "EndTimeCheckbox");
            this.EndTimeCheckbox.Name = "EndTimeCheckbox";
            this.EndTimeCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EndTimeCheckbox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EndTimeCheckbox.TrueValue = "True";
            // 
            // EndTime
            // 
            this.EndTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.EndTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.EndTime.FillWeight = 185.8332F;
            resources.ApplyResources(this.EndTime, "EndTime");
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            this.EndTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // MaxDurationCheckbox
            // 
            this.MaxDurationCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MaxDurationCheckbox.FalseValue = "False";
            resources.ApplyResources(this.MaxDurationCheckbox, "MaxDurationCheckbox");
            this.MaxDurationCheckbox.Name = "MaxDurationCheckbox";
            this.MaxDurationCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MaxDurationCheckbox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MaxDurationCheckbox.TrueValue = "True";
            // 
            // Duration
            // 
            this.Duration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.Duration, "Duration");
            this.Duration.Name = "Duration";
            this.Duration.ReadOnly = true;
            this.Duration.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // TurnOff
            // 
            this.TurnOff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TurnOff.FalseValue = "False";
            resources.ApplyResources(this.TurnOff, "TurnOff");
            this.TurnOff.Name = "TurnOff";
            this.TurnOff.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TurnOff.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TurnOff.TrueValue = "True";
            // 
            // Order
            // 
            resources.ApplyResources(this.Order, "Order");
            this.Order.Name = "Order";
            // 
            // SchedulerForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "SchedulerForm";
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Play;
        private System.Windows.Forms.DataGridViewCheckBoxColumn WakeUp;
        private System.Windows.Forms.DataGridViewCheckBoxColumn StartDateCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn StartTimeCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewComboBoxColumn Playlist;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EndDateCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EndTimeCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MaxDurationCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TurnOff;
        private System.Windows.Forms.DataGridViewTextBoxColumn Order;
    }
}