using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace MusicBeePlugin
{
    public partial class PluginWindowTemplate : Form
    {
        protected static Plugin PluginRef;
        protected static Plugin.MusicBeeApiInterface MbApiInterface;

        private FormWindowState windowState;
        private int windowWidth;
        private int windowHeight;

        public PluginWindowTemplate()
        {
            InitializeComponent();
        }

        public PluginWindowTemplate(Plugin plugin)
        {
            InitializeComponent();

            PluginRef = plugin;

            initializeForm();
        }

        protected void initializeForm()
        {
            MbApiInterface = Plugin.MbApiInterface;

            try
            {
                Plugin.MbForm.AddOwnedForm(this);
            }
            catch
            {
                Plugin.MbForm = (Form)Form.FromHandle(MbApiInterface.MB_GetWindowHandle());
                Plugin.MbForm.AddOwnedForm(this);
            }
        }

        public virtual void display(bool modalForm = false)
        {
            lock (PluginRef.openedForms)
            {
                foreach (PluginWindowTemplate form in PluginRef.openedForms)
                {
                    if (form.GetType() == this.GetType())
                    {
                        this.Dispose(true);

                        if (form.Visible)
                            form.Activate();
                        else
                            form.Show();

                        return;
                    }
                }

                PluginRef.openedForms.Add(this);
            }

            if (modalForm)
                this.ShowDialog();
            else
                this.Show();
        }

        private void ToolsPluginTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (PluginRef.openedForms)
            {
                PluginRef.openedForms.Remove(this);
            }

            string fullName = GetType().FullName;
            SizePositionType currentCommandWindow = null;

            foreach (var commandWindow in Plugin.SavedSettings.commandWindows)
            {
                if (commandWindow.className == fullName)
                {
                    currentCommandWindow = commandWindow;
                    break;
                }
            }

            if (currentCommandWindow == null)
            {
                currentCommandWindow = new SizePositionType();
                currentCommandWindow.className = fullName;

                Plugin.SavedSettings.commandWindows.Add(currentCommandWindow);
            }


            if (windowState == FormWindowState.Maximized)
            {
                currentCommandWindow.max = true;
            }
            else if (windowState == FormWindowState.Minimized)
            {
                //Nothing changing in saved settings...
            }
            else
            {
                currentCommandWindow.x = this.DesktopLocation.X;
                currentCommandWindow.y = this.DesktopLocation.Y;
                currentCommandWindow.w = this.Size.Width;
                currentCommandWindow.h = this.Size.Height;
                currentCommandWindow.max = false;
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
            return; //For debugging

            if (Plugin.SavedSettings.useSkinColors)
            {
                this.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                this.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

                foreach (Control control in this.Controls)
                {
                    if (control.GetType() == typeof(Button))
                    {
                        control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                        control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));
                    }
                    else
                    {
                        control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                        control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));
                    }
                }
            }


            string fullName = this.GetType().FullName;
            foreach (var commandWindow in Plugin.SavedSettings.commandWindows)
            {
                if (commandWindow.className == fullName)
                {
                    if (commandWindow.w != 0)
                    {
                        this.DesktopLocation = new Point(commandWindow.x, commandWindow.y);
                        this.Size = new Size(commandWindow.w, commandWindow.h);

                        if (commandWindow.max)
                            WindowState = FormWindowState.Maximized;

                        break;
                    }
                }
            }
        }
    }
}
