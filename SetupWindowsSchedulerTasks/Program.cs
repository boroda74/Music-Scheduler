using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MusicBeePlugin;

namespace SetupWindowsSchedulerTasks
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string appDataFoler = Regex.Replace(Application.StartupPath, @"^(.*)\\.*", "$1") + @"\AppData";
            if (!System.IO.Directory.Exists(appDataFoler))
            {
                appDataFoler = Regex.Replace(Application.StartupPath, @"^(.*)\\.*\\.*", "$1") + @"\AppData";
            }
            if (!System.IO.Directory.Exists(appDataFoler))
            {
                appDataFoler = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MusicBee";
            }
            string tasksFolder = appDataFoler + @"\Music Scheduler Tasks";

            Plugin.LoadSettings(appDataFoler);


            Plugin.DeleteWindowsTasksByPrefix(Plugin.WindowsTaskPrefix);

            if (!System.IO.Directory.Exists(tasksFolder))
                return;


            string[] taskFiles = System.IO.Directory.GetFiles(tasksFolder, "*." + Plugin.TaskExtention);

            for (int i = 0; i < taskFiles.Length; i++)
            {
                ScheduledTask task = ScheduledTask.Read(taskFiles[i]);
                task.scheduleTaskViaWindowsScheduler(Plugin.SavedSettings.musicBeeStartupPath, Plugin.SavedSettings.turnMonitorOnPath);
            }


            lock (Plugin.AllTimers)
            {
                foreach (var timer in Plugin.AllTimers)
                {
                    timer.Dispose();
                }

                Plugin.AllTimers.Clear();
            }
        }
    }
}
