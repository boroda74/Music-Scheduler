using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace MusicBeePlugin
{
    public partial class PluginWindowTemplate : Form
    {
        private delegate void StopButtonClicked(PluginWindowTemplate clickedForm);
        private delegate void TaskStarted();
        protected delegate bool PrepareOperation();

        private StopButtonClicked stopButtonClicked;
        private TaskStarted taskStarted;

        protected static Plugin TagToolsPlugin;
        protected static Plugin.MusicBeeApiInterface MbApiInterface;

        private Thread backgroundThread = null;
        protected ThreadStart job;

        public bool backgroundTaskIsScheduled = false;
        public bool backgroundTaskIsNativeMB = false;
        public bool backgroundTaskIsCanceled = false;

        public bool previewIsGenerated = false;

        protected Button clickedButton;
        protected string clickedButtonText;

        protected Button closeButton;
        protected string closeButtonText;

        protected Button previewButton;

        private FormWindowState windowState;
        private int windowWidth;
        private int windowHeight;

        public bool backgroundTaskIsWorking()
        {
            lock (taskStarted)
            {
                if (backgroundTaskIsScheduled && !backgroundTaskIsCanceled)
                {
                    return true;
                }
            }

            return false;
        }

        public PluginWindowTemplate()
        {
            InitializeComponent();
        }

        public PluginWindowTemplate(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected void initializeForm()
        {
            MbApiInterface = Plugin.MbApiInterface;

            try
            {
                TagToolsPlugin.mbForm.AddOwnedForm(this);
            }
            catch
            {
                TagToolsPlugin.mbForm = (Form)Form.FromHandle(MbApiInterface.MB_GetWindowHandle());
                TagToolsPlugin.mbForm.AddOwnedForm(this);
            }

            clickedButton = TagToolsPlugin.emptyButton;

            lock (TagToolsPlugin.openedForms)
            {
                if (TagToolsPlugin.numberOfNativeMbBackgroundTasks > 0)
                    disableQueryingButtons();
                else
                    enableQueryingButtons();
            }

            stopButtonClicked = stopButtonClickedMethod;
            taskStarted = taskStartedMethod;

            TagToolsPlugin.fillTagNames();
        }

        public virtual void display()
        {
            lock (TagToolsPlugin.openedForms)
            {
                //foreach (ToolsPluginTemplate form in tagToolsPlugin.openedForms)
                //{
                //    if (form.GetType() == this.GetType())
                //    {
                //        this.Dispose(true);

                //        if (form.Visible)
                //            form.Activate();
                //        else
                //            form.Show();

                //        return;
                //    }
                //}

                TagToolsPlugin.openedForms.Add(this);
            }

            this.Show();
        }

        private void serializedOperation()
        {
            bool taskWasStarted = false;

            TagToolsPlugin.initializeSbText();

            try
            {
                if (backgroundTaskIsCanceled)
                {
                    if (Plugin.SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();
                }
                else
                {
                    if (Plugin.SavedSettings.playStartedSound)
                        System.Media.SystemSounds.Exclamation.Play();

                    backgroundThread = Thread.CurrentThread;
                    backgroundThread.Priority = ThreadPriority.BelowNormal;


                    if (clickedButton != TagToolsPlugin.emptyButton)
                        Invoke(taskStarted);

                    taskWasStarted = true;
                    job();
                }
            }
            catch (ThreadAbortException)
            {
                backgroundTaskIsCanceled = true;
            }
            finally
            {
                lock (taskStarted)
                {
                    if (!Plugin.SavedSettings.dontPlayCompletedSound)
                        System.Media.SystemSounds.Asterisk.Play();

                    backgroundTaskIsScheduled = false;
                    backgroundTaskIsCanceled = false;

                    if (backgroundTaskIsNativeMB && taskWasStarted)
                    {
                        TagToolsPlugin.numberOfNativeMbBackgroundTasks--;
                    }
                }

                if (clickedButton != TagToolsPlugin.emptyButton)
                    Invoke(stopButtonClicked, new Object[] { this });

                TagToolsPlugin.refreshPanels(true);

                TagToolsPlugin.setResultingSbText();

                if (!Visible)
                {
                    if (Plugin.SavedSettings.closeShowHiddenWindows == 1)
                        Close();
                    else
                        Visible = true;
                }
            }
        }

        public void switchOperation(ThreadStart operation, Button clickedButtonParam, Button previewButtonParam, Button closeButtonParam, bool backgroundTaskIsNativeMbParam = true)
        {
            if (backgroundTaskIsScheduled && !backgroundTaskIsWorking())
            {
                if (backgroundTaskIsCanceled)
                {
                    backgroundTaskIsCanceled = false;

                    lock (TagToolsPlugin.openedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            TagToolsPlugin.numberOfNativeMbBackgroundTasks++;
                        }
                    }

                    queryingOrUpdatingButtonClick(this);
                }
                else
                {
                    backgroundTaskIsCanceled = true;

                    lock (TagToolsPlugin.openedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            TagToolsPlugin.numberOfNativeMbBackgroundTasks--;
                        }
                    }

                    stopButtonClicked(this);
                }
            }
            else if (backgroundTaskIsWorking())
            {
                backgroundTaskIsCanceled = true;
            }
            else
            {
                backgroundTaskIsNativeMB = backgroundTaskIsNativeMbParam;

                backgroundTaskIsCanceled = false;
                backgroundTaskIsScheduled = true;
                backgroundThread = null;

                clickedButton = clickedButtonParam;
                closeButton = closeButtonParam;
                previewButton = previewButtonParam;

                job = operation;

                if (backgroundTaskIsNativeMB)
                {
                    lock (TagToolsPlugin.openedForms)
                    {
                        TagToolsPlugin.numberOfNativeMbBackgroundTasks++;
                    }

                    MbApiInterface.MB_CreateBackgroundTask(serializedOperation, this);
                }
                else
                {
                    Thread tempThread = new Thread(serializedOperation);
                    tempThread.Start();
                }

                queryingOrUpdatingButtonClick(this);
            }
        }

        protected string getBackgroundTasksWarning()
        {
            if (TagToolsPlugin.numberOfNativeMbBackgroundTasks == 1)
                return TagToolsPlugin.ctlDirtyError1sf + TagToolsPlugin.numberOfNativeMbBackgroundTasks + TagToolsPlugin.ctlDirtyError2sf;
            else if (TagToolsPlugin.numberOfNativeMbBackgroundTasks > 1)
                return TagToolsPlugin.ctlDirtyError1mf + TagToolsPlugin.numberOfNativeMbBackgroundTasks + TagToolsPlugin.ctlDirtyError2mf;
            else
                return String.Empty;
        }

        public virtual void enableDisablePreviewOptionControls(bool enable)
        {
        }

        public virtual void enableQueryingButtons()
        {
        }

        public virtual void disableQueryingButtons()
        {
        }

        public virtual void enableQueryingOrUpdatingButtons()
        {
        }

        public virtual void disableQueryingOrUpdatingButtons()
        {
        }

        public void queryingOrUpdatingButtonClick(PluginWindowTemplate clickedForm)
        {
            lock (TagToolsPlugin.openedForms)
            {
                foreach (PluginWindowTemplate form in TagToolsPlugin.openedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
                    {
                        form.disableQueryingButtons();
                    }
                }

                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    clickedForm.enableQueryingButtons();
                    clickedForm.disableQueryingOrUpdatingButtons();

                    clickedButtonText = clickedButton.Text;
                    clickedButton.Text = TagToolsPlugin.cancelButtonName;
                    clickedButton.Enabled = true;

                    closeButtonText = closeButton.Text;
                    closeButton.Text = TagToolsPlugin.hideButtonName;
                    closeButton.Enabled = true;
                }
                else //Querying operation
                {
                    clickedForm.disableQueryingOrUpdatingButtons();

                    clickedButtonText = clickedButton.Text;
                    clickedButton.Text = TagToolsPlugin.stopButtonName;
                    clickedButton.Enabled = true;

                    closeButton.Enabled = false;
                }

                enableDisablePreviewOptionControls(false);
            }
        }

        public void stopButtonClickedMethod(PluginWindowTemplate clickedForm)
        {
            lock (TagToolsPlugin.openedForms)
            {
                foreach (PluginWindowTemplate form in TagToolsPlugin.openedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm) //Updating operation
                    {
                        if (TagToolsPlugin.numberOfNativeMbBackgroundTasks > 0 && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
                        {
                            form.disableQueryingButtons();
                        }
                        else
                        {
                            form.enableQueryingButtons();
                        }
                    }
                }

                clickedForm.disableQueryingButtons();
                clickedForm.enableQueryingOrUpdatingButtons();

                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    clickedButton.Text = clickedButtonText;

                    if (previewIsGenerated)
                        previewButton.Text = TagToolsPlugin.clearButtonName;
                    else
                        previewButton.Text = TagToolsPlugin.previewButtonName;

                    if (backgroundTaskIsScheduled)
                        previewButton.Enabled = false;
                    else
                        closeButton.Text = closeButtonText;
                }
                else //Querying operation
                {
                    if (previewIsGenerated)
                        clickedButton.Text = TagToolsPlugin.clearButtonName;
                    else
                        clickedButton.Text = clickedButtonText;

                    closeButton.Enabled = true;
                }

                enableDisablePreviewOptionControls(true);
            }
        }

        private void taskStartedMethod()
        {
            lock (TagToolsPlugin.openedForms)
            {
                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    enableQueryingButtons();

                    clickedButton.Text = TagToolsPlugin.stopButtonName;
                }
            }
        }

        protected void clickOnPreviewButton(DataGridView previewList, PrepareOperation prepareOperation, ThreadStart operation, Button clickedButtonParam, Button closeButtonParam)
        {
            if (!previewIsGenerated || backgroundTaskIsWorking())
            {
                if (prepareOperation())
                    switchOperation(operation, clickedButtonParam, clickedButtonParam, closeButtonParam, false);
            }
            else
            {
                previewList.Rows.Clear();
                previewIsGenerated = false;
                clickedButtonParam.Text = TagToolsPlugin.previewButtonName;
                enableDisablePreviewOptionControls(true);
            }
        }

        private void ToolsPluginTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundTaskIsScheduled)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                lock (TagToolsPlugin.openedForms)
                {
                    TagToolsPlugin.openedForms.Remove(this);
                }

                string fullName = GetType().FullName;
                for (int i = 0; i < Plugin.NumberOfCommandWindows; i++)
                {
                    if (Plugin.SavedSettings.forms[i] == fullName)
                    {
                        if (windowState == FormWindowState.Maximized)
                        {
                            Plugin.SavedSettings.sizesPositions[i].max = true;
                            break;
                        }
                        else if (windowState == FormWindowState.Minimized)
                        {
                            //Nothing changing in saved settings...
                            break;
                        }
                        else
                        {
                            Plugin.SavedSettings.sizesPositions[i].x = this.DesktopLocation.X;
                            Plugin.SavedSettings.sizesPositions[i].y = this.DesktopLocation.Y;
                            Plugin.SavedSettings.sizesPositions[i].w = this.Size.Width;
                            Plugin.SavedSettings.sizesPositions[i].h = this.Size.Height;
                            Plugin.SavedSettings.sizesPositions[i].max = false;
                            break;
                        }
                    }
                }
            }
        }

        private void ToolsPluginTemplate_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                windowWidth = this.RestoreBounds.Width;
                windowHeight = this.RestoreBounds.Height;

                Hide();
            }
            else
            {
                windowState = WindowState;
            }
        }

        private void ToolsPluginTemplate_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && windowWidth != 0)
            {
                WindowState = windowState;
                this.Width = windowWidth;
                this.Height = windowHeight;

                windowWidth = 0;
            }
        }

        private void PluginWindowTemplate_Load(object sender, EventArgs e)
        {
            //if (Plugin.SavedSettings.useSkinColors)
            //{
            //    this.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
            //    this.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

            //    foreach (Control control in this.Controls)
            //    {
            //        if (control.GetType() == clickedButton.GetType())
            //        {
            //            control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
            //            control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));
            //        }
            //        else
            //        {
            //            control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
            //            control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));
            //        }
            //    }
            //}


            //string fullName = this.GetType().FullName;
            //for (int i = 0; i < Plugin.NumberOfCommandWindows; i++)
            //{
            //    if (Plugin.SavedSettings.forms[i] == fullName)
            //    {
            //        if (Plugin.SavedSettings.sizesPositions[i].w != 0)
            //        {
            //            this.DesktopLocation = new Point(Plugin.SavedSettings.sizesPositions[i].x, Plugin.SavedSettings.sizesPositions[i].y);
            //            this.Size = new Size(Plugin.SavedSettings.sizesPositions[i].w, Plugin.SavedSettings.sizesPositions[i].h);

            //            if (Plugin.SavedSettings.sizesPositions[i].max)
            //                WindowState = FormWindowState.Maximized;

            //            break;
            //        }
            //    }
            //}
        }
    }
}
