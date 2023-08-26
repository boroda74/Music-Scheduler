using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using MusicBeePlugin.Properties;


namespace MusicBeePlugin
{
    public partial class SchedulerForm : PluginWindowTemplate
    {
        public ScheduledTask scheduledTask;
        private List<LibraryPlaylist> libraryPlaylists = new List<LibraryPlaylist>();

        private string name3;
        private string name5;
        private string name8;
        private string name10;
        private string name12;

        private DataGridViewCellStyle normalTextCellStyle = null;
        private DataGridViewCellStyle grayedTextCellStyle = null;

        public SchedulerForm()
        {
            InitializeComponent();
        }

        public SchedulerForm(Plugin plugin, ScheduledTask task)
        {
            InitializeComponent();

            PluginRef = plugin;
            scheduledTask = task;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            name3 = previewTable.Columns[3].HeaderText;
            name5 = previewTable.Columns[5].HeaderText;
            name8 = previewTable.Columns[8].HeaderText;
            name10 = previewTable.Columns[10].HeaderText;
            name12 = previewTable.Columns[12].HeaderText;


            libraryPlaylists.Add(new LibraryPlaylist(Plugin.CtlAutoDJ));
            ((DataGridViewComboBoxColumn)previewTable.Columns[6]).Items.Add(Plugin.CtlAutoDJ);

            if (MbApiInterface.Playlist_QueryPlaylists())
            {
                string playlist;
                while (!string.IsNullOrEmpty(playlist = MbApiInterface.Playlist_QueryGetNextPlaylist()))
                {
                    var libraryPlaylist = new LibraryPlaylist(playlist);
                    libraryPlaylists.Add(libraryPlaylist);

                    ((DataGridViewComboBoxColumn)previewTable.Columns[6]).Items.Add(libraryPlaylist.playlistSubPath);
                }
            }


            System.Resources.ResourceManager resourceManager = Resources.ResourceManager;

            string toolTipText = previewTable.Columns[0].HeaderCell.ToolTipText;
            previewTable.Columns[0].HeaderCell = new DataGridBitmapHeaderCell((System.Drawing.Bitmap)resourceManager.GetObject("DontSkip"), toolTipText);
            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            toolTipText = previewTable.Columns[1].HeaderCell.ToolTipText;
            previewTable.Columns[1].HeaderCell = new DataGridBitmapHeaderCell((System.Drawing.Bitmap)resourceManager.GetObject("Autostart"), toolTipText);
            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            toolTipText = previewTable.Columns[13].HeaderCell.ToolTipText;
            previewTable.Columns[13].HeaderCell = new DataGridBitmapHeaderCell((System.Drawing.Bitmap)resourceManager.GetObject("TurnOff"), toolTipText);
            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            foreach (var playlist in scheduledTask.playlists)
                addPreviewTableRow(playlist, true);


            Text = scheduledTask.taskName + " - " + Text;
        }

        private bool playlistSubPathExists(string playlistSubPath)
        {
            foreach (var libraryPlaylist in libraryPlaylists)
                if (libraryPlaylist.playlistSubPath == playlistSubPath)
                    return true;

            return false;
        }

        private LibraryPlaylist findLibraryPlaylistBySubPath(string playlistSubPath)
        {
            foreach (var libraryPlaylist in libraryPlaylists)
                if (libraryPlaylist.playlistSubPath == playlistSubPath)
                    return libraryPlaylist;

            return null;
        }

        private void addPreviewTableRow(ScheduledPlaylist playlist, bool orderByPlaylistInternalNumber)
        {
            previewTable.Rows.Add();

            if (orderByPlaylistInternalNumber)
                previewTable.Rows[previewTable.RowCount - 1].Cells[14].Value = playlist.playlistOrder.ToString("D8");
            else
                previewTable.Rows[previewTable.RowCount - 1].Cells[14].Value = (previewTable.RowCount - 1).ToString("D8");

            previewTable.Rows[previewTable.RowCount - 1].Cells[2].Tag = playlist.startDaysOfWeek;
            previewTable.Rows[previewTable.RowCount - 1].Cells[3].Tag = playlist.startDate;
            previewTable.Rows[previewTable.RowCount - 1].Cells[5].Tag = playlist.startTime;
            previewTable.Rows[previewTable.RowCount - 1].Cells[8].Tag = playlist.endDate;
            previewTable.Rows[previewTable.RowCount - 1].Cells[10].Tag = playlist.endTime;
            previewTable.Rows[previewTable.RowCount - 1].Cells[12].Tag = playlist.duration;


            previewTable.Rows[previewTable.RowCount - 1].Cells[0].Value = playlist.playPlaylist ? "True" : "False";
            previewTable.Rows[previewTable.RowCount - 1].Cells[1].Value = playlist.autorunMusicBee ? "True" : "False";
            previewTable.Rows[previewTable.RowCount - 1].Cells[2].Value = playlist.startDateChecked ? "True" : "False";

            if (playlist.startDateChecked)
                previewTable.Rows[previewTable.RowCount - 1].Cells[3].Value = playlist.startDate.ToString("d");
            else
                previewTable.Rows[previewTable.RowCount - 1].Cells[3].Value = ScheduledPlaylist.GetWeekDays(playlist.startDaysOfWeek);

            previewTable.Rows[previewTable.RowCount - 1].Cells[4].Value = playlist.startTimeChecked ? "True" : "False";
            previewTable.Rows[previewTable.RowCount - 1].Cells[5].Value = playlist.startTime.ToString("T");

            if (playlist.libraryPlaylist == null && ((DataGridViewComboBoxColumn)previewTable.Columns[6]).Items.Count > 0)
            {
                ((DataGridViewComboBoxCell)previewTable.Rows[previewTable.RowCount - 1].Cells[6]).Value = ((DataGridViewComboBoxColumn)previewTable.Columns[6]).Items[0];
            }
            else if (playlist.libraryPlaylist.playlistSubPath == Plugin.CtlAutoDJ)
            {
                ((DataGridViewComboBoxCell)previewTable.Rows[previewTable.RowCount - 1].Cells[6]).Value = playlist.libraryPlaylist.playlistSubPath;
            }
            else if (playlistSubPathExists(playlist.libraryPlaylist.playlistSubPath))
            {
                ((DataGridViewComboBoxCell)previewTable.Rows[previewTable.RowCount - 1].Cells[6]).Value = playlist.libraryPlaylist.playlistSubPath;
            }
            else
            {
                ((DataGridViewComboBoxCell)previewTable.Rows[previewTable.RowCount - 1].Cells[6]).Value = Plugin.CtlPlaylistNotFound;
            }

            previewTable.Rows[previewTable.RowCount - 1].Cells[7].Value = playlist.endDateChecked ? "True" : "False";
            previewTable.Rows[previewTable.RowCount - 1].Cells[8].Value = playlist.endDate.ToString("d");

            previewTable.Rows[previewTable.RowCount - 1].Cells[9].Value = playlist.endTimeChecked ? "True" : "False";
            previewTable.Rows[previewTable.RowCount - 1].Cells[10].Value = playlist.endTime.ToString("T");

            previewTable.Rows[previewTable.RowCount - 1].Cells[11].Value = playlist.durationChecked ? "True" : "False";
            previewTable.Rows[previewTable.RowCount - 1].Cells[12].Value = playlist.duration.ToString("HH:mm:ss");

            previewTable.Rows[previewTable.RowCount - 1].Cells[13].Value = playlist.TurnOff ? "True" : "False";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (var playlist in scheduledTask.playlists)
            {
                playlist.setStartTimer(null, DateTime.MinValue);
                playlist.setEndTimer(null, DateTime.MinValue);
            }

            scheduledTask.playlists.Clear();


            for (int i = 0; i < previewTable.RowCount; i++)
            {
                var playlist = new ScheduledPlaylist();

                playlist.playlistOrder = i;
                playlist.playPlaylist = "" + previewTable.Rows[i].Cells[0].Value == "True" ? true : false;
                playlist.autorunMusicBee = "" + previewTable.Rows[i].Cells[1].Value == "True" ? true : false;

                playlist.startDateChecked = "" + previewTable.Rows[i].Cells[2].Value == "True" ? true : false;
                playlist.startDaysOfWeek = (byte)previewTable.Rows[i].Cells[2].Tag;
                playlist.startDate = (DateTime)previewTable.Rows[i].Cells[3].Tag;

                playlist.startTimeChecked = "" + previewTable.Rows[i].Cells[4].Value == "True" ? true : false;
                playlist.startTime = (DateTime)previewTable.Rows[i].Cells[5].Tag;

                playlist.libraryPlaylist = findLibraryPlaylistBySubPath((string)previewTable.Rows[i].Cells[6].Value);

                playlist.endDateChecked = "" + previewTable.Rows[i].Cells[7].Value == "True" ? true : false;
                playlist.endDate = (DateTime)previewTable.Rows[i].Cells[8].Tag;

                playlist.endTimeChecked = "" + previewTable.Rows[i].Cells[9].Value == "True" ? true : false;
                playlist.endTime = (DateTime)previewTable.Rows[i].Cells[10].Tag;

                playlist.durationChecked = "" + previewTable.Rows[i].Cells[11].Value == "True" ? true : false;
                playlist.duration = (DateTime)previewTable.Rows[i].Cells[12].Tag;

                playlist.TurnOff = "" + previewTable.Rows[i].Cells[13].Value == "True" ? true : false;

                scheduledTask.playlists.Add(playlist);
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            addPreviewTableRow(new ScheduledPlaylist(), false);
            previewTable.Rows[previewTable.RowCount - 1].Selected = true;

            buttonDelete.Enabled = true;

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

                if (previewTable.Rows.Count == 0)
                {
                    buttonDelete.Enabled = false;
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
                previewTable.Rows[index].Cells[13].Value = (index - 1).ToString("D8");
                previewTable.Rows[index - 1].Cells[13].Value = index.ToString("D8");
                previewTable.Sort(previewTable.Columns[13], ListSortDirection.Ascending);
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = previewTable.CurrentRow.Index;

            if (index < previewTable.Rows.Count - 1)
            {
                previewTable.Rows[index].Cells[13].Value = (index + 1).ToString("D8");
                previewTable.Rows[index + 1].Cells[13].Value = index.ToString("D8");
                previewTable.Sort(previewTable.Columns[13], ListSortDirection.Ascending);
            }
        }

        private void CellFormatting(int columnIndex, int rowIndex)
        {
            if (columnIndex == 4 || columnIndex == 7 || columnIndex == 9 || columnIndex == 11)
            {
                if (normalTextCellStyle == null)
                {
                    normalTextCellStyle = previewTable.Rows[rowIndex].Cells[columnIndex + 1].Style;

                    grayedTextCellStyle = new DataGridViewCellStyle(previewTable.Rows[rowIndex].Cells[columnIndex + 1].Style);
                    grayedTextCellStyle.ForeColor = System.Drawing.SystemColors.GrayText;
                    grayedTextCellStyle.SelectionForeColor = System.Drawing.SystemColors.GrayText;
                }

                if ("" + previewTable.Rows[rowIndex].Cells[columnIndex].Value == "True")
                    previewTable.Rows[rowIndex].Cells[columnIndex + 1].Style = normalTextCellStyle;
                else
                    previewTable.Rows[rowIndex].Cells[columnIndex + 1].Style = grayedTextCellStyle;
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            previewTable.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void previewTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            if (e.ColumnIndex == 1) //Its autostart
            {
                if ("" + previewTable.Rows[e.RowIndex].Cells[1].Value == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[4].Value = "True";
                }
            }
            else if (e.ColumnIndex == 2 || e.ColumnIndex == 7) //Start/end date checkbox
            {
                if ("" + previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "True") //Date is enabled
                {
                    previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = ((DateTime)previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Tag).ToString("d");
                    previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value = "True";
                }
                else //Date is disabled
                {
                    if (e.ColumnIndex == 2) //Start date checkbox is disabled
                    {
                        previewTable.Rows[e.RowIndex].Cells[3].Value = ScheduledPlaylist.GetWeekDays((byte)previewTable.Rows[e.RowIndex].Cells[2].Tag);
                    }
                }
            }
            else if (e.ColumnIndex == 4 || e.ColumnIndex == 9) //Start/end time checkbox
            {
                if ("" + previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "False") //Time is disabled
                {
                    previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value = "False"; //Lets disable date also

                    if (e.ColumnIndex == 4) //Start time checkbox is disabled
                    {
                        previewTable.Rows[e.RowIndex].Cells[1].Value = "False";
                        previewTable.Rows[e.RowIndex].Cells[3].Value = ScheduledPlaylist.GetWeekDays((byte)previewTable.Rows[e.RowIndex].Cells[2].Tag);
                    }
                }
            }


            CellFormatting(e.ColumnIndex, e.RowIndex);
        }

        private void previewTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 || e.ColumnIndex == 8) //Its date
            {
                if ("" + previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value == "True") //Date is enabled
                {
                    DatePickerForm picker = new DatePickerForm(PluginRef, (DateTime)previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
                    picker.display(true);
                    previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = picker.dateTime.ToString("d");
                    previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = picker.dateTime;
                }
                else if (e.ColumnIndex == 3) //Start date is disabled
                {
                    DayPickerForm picker = new DayPickerForm(PluginRef, (byte)previewTable.Rows[e.RowIndex].Cells[2].Tag);
                    picker.display(true);
                    previewTable.Rows[e.RowIndex].Cells[2].Tag = picker.days;
                    previewTable.Rows[e.RowIndex].Cells[3].Value = ScheduledPlaylist.GetWeekDays(picker.days);
                }
            }
            else if ((e.ColumnIndex == 5 || e.ColumnIndex == 10) && "" + previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value == "True") //Its time & its enabled
            {
                TimePickerForm picker = new TimePickerForm(PluginRef, (DateTime)previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
                picker.display(true);

                previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = picker.dateTime;
                previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = picker.dateTime.ToString("T");
            }
            else if (e.ColumnIndex == 12) //Its duration
            {
                DurationPickerForm picker = new DurationPickerForm(PluginRef, (DateTime)previewTable.Rows[e.RowIndex].Cells[12].Tag);
                picker.display(true);

                previewTable.Rows[e.RowIndex].Cells[12].Tag = picker.duration;
                previewTable.Rows[e.RowIndex].Cells[12].Value = ((DateTime)previewTable.Rows[e.RowIndex].Cells[12].Tag).ToString("HH:mm:ss");
            }


            if (e.ColumnIndex != 0 && e.ColumnIndex != 1 && e.ColumnIndex != 6 && e.ColumnIndex != 11 && e.ColumnIndex != 12 && e.ColumnIndex != 13)
            {
                DateTime date;
                DateTime time;

                if ("" + previewTable.Rows[e.RowIndex].Cells[2].Value == "False" || "" + previewTable.Rows[e.RowIndex].Cells[7].Value == "False")
                    return;


                date = (DateTime)previewTable.Rows[e.RowIndex].Cells[3].Tag;
                time = (DateTime)previewTable.Rows[e.RowIndex].Cells[5].Tag;

                DateTime startDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);


                date = (DateTime)previewTable.Rows[e.RowIndex].Cells[8].Tag;
                time = (DateTime)previewTable.Rows[e.RowIndex].Cells[10].Tag;
                DateTime endDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);


                if (startDateTime > endDateTime)
                {
                    previewTable.Rows[e.RowIndex].Cells[8].Tag = startDateTime;
                    previewTable.Rows[e.RowIndex].Cells[10].Tag = new DateTime(2000, 01, 01, startDateTime.Hour, startDateTime.Minute, startDateTime.Second);
                }
            }
        }

        private void previewTable_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < previewTable.RowCount; i++)
                previewTable.Rows[i].Cells[13].Value = i.ToString("D8");
        }

        private void previewTable_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == name3)
            {
                e.Handled = true;

                if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[3].Tag > (DateTime)previewTable.Rows[e.RowIndex2].Cells[3].Tag)
                {
                    e.SortResult = 1;
                }
                else if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[3].Tag < (DateTime)previewTable.Rows[e.RowIndex2].Cells[3].Tag)
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = 0;
                }
            }
            else if (e.Column.Name == name5)
            {
                e.Handled = true;

                if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[5].Tag > (DateTime)previewTable.Rows[e.RowIndex2].Cells[5].Tag)
                {
                    e.SortResult = 1;
                }
                else if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[5].Tag < (DateTime)previewTable.Rows[e.RowIndex2].Cells[5].Tag)
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = 0;
                }
            }
            else if (e.Column.Name == name8)
            {
                e.Handled = true;

                if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[8].Tag > (DateTime)previewTable.Rows[e.RowIndex2].Cells[8].Tag)
                {
                    e.SortResult = 1;
                }
                else if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[8].Tag < (DateTime)previewTable.Rows[e.RowIndex2].Cells[8].Tag)
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = 0;
                }
            }
            else if (e.Column.Name == name10)
            {
                e.Handled = true;

                if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[10].Tag > (DateTime)previewTable.Rows[e.RowIndex2].Cells[10].Tag)
                {
                    e.SortResult = 1;
                }
                else if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[10].Tag < (DateTime)previewTable.Rows[e.RowIndex2].Cells[10].Tag)
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = 0;
                }
            }
            else if (e.Column.Name == name12)
            {
                e.Handled = true;

                if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[12].Tag > (DateTime)previewTable.Rows[e.RowIndex2].Cells[12].Tag)
                {
                    e.SortResult = 1;
                }
                else if ((DateTime)previewTable.Rows[e.RowIndex1].Cells[12].Tag < (DateTime)previewTable.Rows[e.RowIndex2].Cells[12].Tag)
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = 0;
                }
            }
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
