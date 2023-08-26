using MusicBeePlugin.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class TasksForm : PluginWindowTemplate
    {
        public TasksForm()
        {
            InitializeComponent();
        }

        public TasksForm(Plugin pluginParam)
        {
            InitializeComponent();

            PluginRef = pluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            string[] taskFiles = System.IO.Directory.GetFiles(Plugin.TasksFolder, "*." + Plugin.TaskExtention);

            for (int i = 0; i < taskFiles.Length; i++)
            {
                ScheduledTask task = ScheduledTask.Read(taskFiles[i]);
                addPreviewTableRow(task, true);
            }
            previewTable.Sort(previewTable.Columns[2], System.ComponentModel.ListSortDirection.Ascending);


            if (previewTable.Rows.Count == 0)
            {
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
            }

            if (previewTable.Rows.Count < 2)
            {
                buttonUp.Enabled = false;
                buttonDown.Enabled = false;
            }

            if (Plugin.SavedSettings.shutdownMethod == 0)
            {
                radioButtonOn.Checked = true;
            }
            else if (Plugin.SavedSettings.shutdownMethod == 1)
            {
                radioButtonSleep.Checked = true;
            }
            else if (Plugin.SavedSettings.shutdownMethod == 2)
            {
                radioButtonHibernate.Checked = true;
            }
            else //if (Plugin.SavedSettings.shutdownMethod == 3)
            {
                radioButtonShutdown.Checked = true;
            }

            closeMbCheckBox.Checked = Plugin.SavedSettings.closeMb;
            checkBoxWakeupPCOnAutostart.Checked = !Plugin.SavedSettings.dontWakeupPCOnAutostart;
            preventPcSleepCheckBox.Checked = !Plugin.SavedSettings.dontPreventPcSleep;
            preventMonitorSleepCheckBox.Checked = !Plugin.SavedSettings.dontPreventMonitorSleep;

            skinColorsCheckBox.Checked = Plugin.SavedSettings.useSkinColors;
            stopAfterCurrentCheckBox.Checked = Plugin.SavedSettings.stopAfterCurrent;

            preventPcSleepCheckBox_CheckedChanged(null, null);

            string toolTipText = previewTable.Columns[0].HeaderCell.ToolTipText;
            System.Resources.ResourceManager resourceManager = Resources.ResourceManager;
            previewTable.Columns[0].HeaderCell = new DataGridBitmapHeaderCell((System.Drawing.Bitmap)resourceManager.GetObject("DontSkip"), toolTipText);
            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        private string generateTaskPostfix(ScheduledTask task, int rowIndex)
        {
            string oldPostfix = null;
            string postfix = "";

            loop:
            while (oldPostfix != postfix)
            {
                oldPostfix = postfix;

                for (int i = 0; i < previewTable.RowCount; i++)
                {
                    if (task.taskName + postfix == (string)previewTable.Rows[i].Cells[1].Value && i != rowIndex)
                    {
                        postfix += "*";
                        goto loop;
                    }
                }
            }

            return postfix;
        }

        private void addPreviewTableRow(ScheduledTask task, bool orderByTaskInternalNumber)
        {
            previewTable.Rows.Add();

            previewTable.Rows[previewTable.RowCount - 1].Cells[0].Tag = task;

            if (orderByTaskInternalNumber)
                previewTable.Rows[previewTable.RowCount - 1].Cells[2].Value = task.taskOrder.ToString("D8");
            else
                previewTable.Rows[previewTable.RowCount - 1].Cells[2].Value = (previewTable.RowCount - 1).ToString("D8");

            previewTable.Rows[previewTable.RowCount - 1].Cells[0].Value = task.playTask ? "True" : "False";

            task.taskName += generateTaskPostfix(task, previewTable.RowCount - 1);
            previewTable.Rows[previewTable.RowCount - 1].Cells[1].Value = task.taskName;
        }

        private void saveSettings()
        {
            if (radioButtonOn.Checked)
            {
                Plugin.SavedSettings.shutdownMethod = 0;
            }
            else if (radioButtonSleep.Checked)
            {
                Plugin.SavedSettings.shutdownMethod = 1;
            }
            else if (radioButtonHibernate.Checked)
            {
                Plugin.SavedSettings.shutdownMethod = 2;
            }
            else //if (radioButtonShutdown.Checked)
            {
                Plugin.SavedSettings.shutdownMethod = 3;
            }

            Plugin.SavedSettings.closeMb = closeMbCheckBox.Checked;
            Plugin.SavedSettings.dontWakeupPCOnAutostart = !checkBoxWakeupPCOnAutostart.Checked;
            Plugin.SavedSettings.dontPreventPcSleep = !preventPcSleepCheckBox.Checked;
            Plugin.SavedSettings.dontPreventMonitorSleep = !preventMonitorSleepCheckBox.Checked;

            Plugin.SavedSettings.useSkinColors = skinColorsCheckBox.Checked;
            Plugin.SavedSettings.stopAfterCurrent = stopAfterCurrentCheckBox.Checked;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            saveSettings();
            PluginRef.SaveSettings();

            buttonStopAll_Click(null, null);

            var taskFiles = System.IO.Directory.GetFiles(Plugin.TasksFolder, "*." + Plugin.TaskExtention);
            foreach (var taskFile in taskFiles)
            {
                System.IO.File.Delete(taskFile);
            }


            for (int i = 0; i < previewTable.RowCount; i++)
            {
                var task = (ScheduledTask)previewTable.Rows[i].Cells[0].Tag;

                task.taskOrder = i;
                task.playTask = (string)previewTable.Rows[i].Cells[0].Value == "True" ? true : false;
                task.taskName = (string)previewTable.Rows[i].Cells[1].Value;

                task.scheduleTask(false);

                task.save();
            }


            Process proc = new Process();
            proc.StartInfo.FileName = Plugin.SavedSettings.setupWindowsSchedulerTasksPath;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonApply_Click(null, null);
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            addPreviewTableRow(new ScheduledTask(), false);
            previewTable.Rows[previewTable.RowCount - 1].Selected = true;


            buttonDelete.Enabled = true;
            buttonEdit.Enabled = true;

            if (previewTable.Rows.Count > 1)
            {
                buttonUp.Enabled = true;
                buttonDown.Enabled = true;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = previewTable.CurrentRow.Index;
            if (index >= 0)
            {
                if (index == previewTable.Rows.Count - 1)
                    previewTable.Rows[index].Selected = false;

                previewTable.Rows.RemoveAt(index);


                for (int i = index; i < previewTable.RowCount; i++)
                    ((ScheduledTask)previewTable.Rows[i].Cells[0].Tag).taskOrder--;


                if (previewTable.Rows.Count == 0)
                {
                    buttonDelete.Enabled = false;
                    buttonEdit.Enabled = false;
                }

                if (previewTable.Rows.Count < 2)
                {
                    buttonUp.Enabled = false;
                    buttonDown.Enabled = false;
                }
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = previewTable.CurrentRow.Index;

            if (index > 0)
            {
                previewTable.Rows[index].Cells[2].Value = (index - 1).ToString("D8");
                previewTable.Rows[index - 1].Cells[2].Value = index.ToString("D8");
                previewTable.Sort(previewTable.Columns[2], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = previewTable.CurrentRow.Index;

            if (index < previewTable.Rows.Count - 1)
            {
                previewTable.Rows[index].Cells[2].Value = (index + 1).ToString("D8");
                previewTable.Rows[index + 1].Cells[2].Value = index.ToString("D8");
                previewTable.Sort(previewTable.Columns[2], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void previewTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                bool taskNameExists = false;
                string taskName = (string)previewTable.Rows[e.RowIndex].Cells[1].Value;

                for (int i = 0; i < previewTable.RowCount; i++)
                {
                    if (i != e.RowIndex)
                    {
                        if ((string)previewTable.Rows[i].Cells[1].Value == taskName)
                        {
                            taskNameExists = true;
                            break;
                        }
                    }
                }

                if (taskNameExists)
                {
                    MessageBox.Show(Plugin.MsgTaskWithThisNameAlreadyExists.Replace("%%taskName%%", taskName));

                    var task = (ScheduledTask)previewTable.Rows[e.RowIndex].Cells[0].Tag;
                    previewTable.Rows[e.RowIndex].Cells[1].Value = task.taskName + generateTaskPostfix(task, e.RowIndex);
                }
                else
                {
                    ((ScheduledTask)previewTable.Rows[e.RowIndex].Cells[0].Tag).taskName = (string)previewTable.Rows[e.RowIndex].Cells[1].Value;
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (previewTable.SelectedRows.Count == 0)
                return;

            SchedulerForm form = new SchedulerForm(PluginRef, (ScheduledTask)previewTable.SelectedRows[0].Cells[0].Tag);
            form.display(true);

            previewTable.SelectedRows[0].Cells[0].Tag = form.scheduledTask;
        }

        private void previewTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonEdit_Click(null, null);
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            if (e.ColumnIndex == 0)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[0].Value == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";
                }
                else
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    for (int i = 0; i < previewTable.Rows.Count; i++)
                    {
                        if (i != e.RowIndex)
                        {
                            previewTable.Rows[i].Cells[0].Value = "False";
                        }
                    }
                }
            }
        }

        private void previewTable_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < previewTable.RowCount; i++)
                previewTable.Rows[i].Cells[2].Value = i.ToString("D8");
        }

        public static void StopAll(ScheduledTask task)
        {
            ScheduledTask.TaskStopped(task);
            Plugin.MbApiInterface.Player_Stop();
        }

        private void buttonStopAll_Click(object sender, EventArgs e)
        {
            if (Plugin.CurrentScheduledPlaylist != null)
                StopAll(Plugin.CurrentScheduledPlaylist.getTask());
            else
                StopAll(null);
        }

        private void preventPcSleepCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!preventPcSleepCheckBox.Checked)
                preventMonitorSleepCheckBox.Checked = false;

            preventMonitorSleepCheckBox.Enabled = preventPcSleepCheckBox.Checked;
        }
    }
}
