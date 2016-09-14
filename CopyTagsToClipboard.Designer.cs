namespace MusicBeePlugin
{
    partial class CopyTagsToClipboardPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyTagsToClipboardPlugin));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sourceTagList = new System.Windows.Forms.CheckedListBox();
            this.checkedSourceTagList = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1.SuspendLayout();
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
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.sourceTagList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkedSourceTagList, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.FormattingEnabled = true;
            this.sourceTagList.MultiColumn = true;
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Sorted = true;
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // checkedSourceTagList
            // 
            resources.ApplyResources(this.checkedSourceTagList, "checkedSourceTagList");
            this.checkedSourceTagList.FormattingEnabled = true;
            this.checkedSourceTagList.MultiColumn = true;
            this.checkedSourceTagList.Name = "checkedSourceTagList";
            this.checkedSourceTagList.Sorted = true;
            this.checkedSourceTagList.SelectedIndexChanged += new System.EventHandler(this.checkedSourceTagList_SelectedIndexChanged);
            // 
            // CopyTagsToClipboardPlugin
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyTagsToClipboardPlugin";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox sourceTagList;
        private System.Windows.Forms.CheckedListBox checkedSourceTagList;
    }
}