namespace MusicBeePlugin
{
    partial class DayPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DayPickerForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.suCheckBox = new System.Windows.Forms.CheckBox();
            this.moCheckBox = new System.Windows.Forms.CheckBox();
            this.tuCheckBox = new System.Windows.Forms.CheckBox();
            this.weCheckBox = new System.Windows.Forms.CheckBox();
            this.thCheckBox = new System.Windows.Forms.CheckBox();
            this.frCheckBox = new System.Windows.Forms.CheckBox();
            this.saCheckBox = new System.Windows.Forms.CheckBox();
            this.su2CheckBox = new System.Windows.Forms.CheckBox();
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
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // suCheckBox
            // 
            resources.ApplyResources(this.suCheckBox, "suCheckBox");
            this.suCheckBox.Name = "suCheckBox";
            this.suCheckBox.UseVisualStyleBackColor = true;
            // 
            // moCheckBox
            // 
            resources.ApplyResources(this.moCheckBox, "moCheckBox");
            this.moCheckBox.Name = "moCheckBox";
            this.moCheckBox.UseVisualStyleBackColor = true;
            // 
            // tuCheckBox
            // 
            resources.ApplyResources(this.tuCheckBox, "tuCheckBox");
            this.tuCheckBox.Name = "tuCheckBox";
            this.tuCheckBox.UseVisualStyleBackColor = true;
            // 
            // weCheckBox
            // 
            resources.ApplyResources(this.weCheckBox, "weCheckBox");
            this.weCheckBox.Name = "weCheckBox";
            this.weCheckBox.UseVisualStyleBackColor = true;
            // 
            // thCheckBox
            // 
            resources.ApplyResources(this.thCheckBox, "thCheckBox");
            this.thCheckBox.Name = "thCheckBox";
            this.thCheckBox.UseVisualStyleBackColor = true;
            // 
            // frCheckBox
            // 
            resources.ApplyResources(this.frCheckBox, "frCheckBox");
            this.frCheckBox.Name = "frCheckBox";
            this.frCheckBox.UseVisualStyleBackColor = true;
            // 
            // saCheckBox
            // 
            resources.ApplyResources(this.saCheckBox, "saCheckBox");
            this.saCheckBox.Name = "saCheckBox";
            this.saCheckBox.UseVisualStyleBackColor = true;
            // 
            // su2CheckBox
            // 
            resources.ApplyResources(this.su2CheckBox, "su2CheckBox");
            this.su2CheckBox.Name = "su2CheckBox";
            this.su2CheckBox.UseVisualStyleBackColor = true;
            // 
            // DayPickerForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.ControlBox = false;
            this.Controls.Add(this.su2CheckBox);
            this.Controls.Add(this.saCheckBox);
            this.Controls.Add(this.frCheckBox);
            this.Controls.Add(this.thCheckBox);
            this.Controls.Add(this.weCheckBox);
            this.Controls.Add(this.tuCheckBox);
            this.Controls.Add(this.moCheckBox);
            this.Controls.Add(this.suCheckBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DayPickerForm";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox suCheckBox;
        private System.Windows.Forms.CheckBox moCheckBox;
        private System.Windows.Forms.CheckBox tuCheckBox;
        private System.Windows.Forms.CheckBox weCheckBox;
        private System.Windows.Forms.CheckBox thCheckBox;
        private System.Windows.Forms.CheckBox frCheckBox;
        private System.Windows.Forms.CheckBox saCheckBox;
        private System.Windows.Forms.CheckBox su2CheckBox;
    }
}