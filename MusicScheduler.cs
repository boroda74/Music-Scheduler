using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    #region Custom types
    public class SizePositionType
    {
        public string className;
        public int x;
        public int y;
        public int w;
        public int h;
        public bool max;
    }

    public class LibraryPlaylist
    {
        public string playlistSubPath = null;
        public string playlistExtension = null;

        public LibraryPlaylist()
        {
            //Nothing to do...
        }

        public LibraryPlaylist(string playlistFullPath)
        {
            if (playlistFullPath == Plugin.CtlAutoDJ)
            {
                playlistSubPath = playlistFullPath;
                playlistExtension = "";
            }
            else
            {
                playlistSubPath = Regex.Replace(playlistFullPath, @"^.*\\Playlists\\(.*)\..*", "$1");
                playlistExtension = Regex.Replace(playlistFullPath, @"^.*\\Playlists\\.*(\..*)", "$1");
            }
        }

        public string getPlaylistFullPath()
        {
            return Plugin.PlaylistCommonPath + @"\" + playlistSubPath + playlistExtension;
        }
    }

    public class ScheduledPlaylist
    {
        private ScheduledTask task = null;

        private ScheduledPlaylist nextScheduledPlaylist = null;
        private ScheduledPlaylist previousScheduledPlaylist = null;

        private System.Threading.Timer startTimer = null;
        private System.Threading.Timer endTimer = null;

        private DateTime startDateTime;
        private DateTime endDateTime;

        public int playlistOrder;

        public bool playPlaylist = true;
        public bool skipPlaylist = false;
        public bool autorunMusicBee = false;

        public static byte FirstDayOfTheWeek = 0;

        public const byte SundayBit = 0x01;
        public const byte MondayBit = 0x02;
        public const byte TuesdayBit = 0x04;
        public const byte WednesdayBit = 0x08;
        public const byte ThursdayBit = 0x10;
        public const byte FridayBit = 0x20;
        public const byte SaturdayBit = 0x40;

        public static string Sunday;
        public static string Monday;
        public static string Tuesday;
        public static string Wednesday;
        public static string Thursday;
        public static string Friday;
        public static string Saturday;

        public bool startDateChecked = false;
        public DateTime startDate;
        public byte startDaysOfWeek = 0x7F;
        public bool startTimeChecked = false;
        public DateTime startTime;

        public LibraryPlaylist libraryPlaylist = null;

        public bool endDateChecked = false;
        public DateTime endDate;
        public bool endTimeChecked = false;
        public DateTime endTime;
        public bool durationChecked = false;
        public DateTime duration = new DateTime(2000, 01, 01, 00, 00, 00);

        public bool TurnOff;

        public ScheduledPlaylist()
        {
            var now = DateTime.Now;

            startDate = now;
            startTime = new DateTime(2000, 01, 01, now.Hour, now.Minute, now.Second);

            endDate = startDate;
            endTime = startTime;
        }

        public void setTask(ScheduledTask pTask)
        {
            task = pTask;
        }

        public ScheduledTask getTask()
        {
            return task;
        }

        public static string GetWeekDays(byte days)
        {
            if (days == 0x7F)
            {
                return Plugin.CtlDaily;
            }
            else
            {
                string weekDays = "";

                if (FirstDayOfTheWeek == 0)
                {
                    if ((days & SundayBit) == SundayBit)
                    {
                        weekDays += Sunday;
                    }
                }

                if ((days & MondayBit) == MondayBit)
                {
                    weekDays += Monday;
                }

                if ((days & TuesdayBit) == TuesdayBit)
                {
                    weekDays += Tuesday;
                }

                if ((days & WednesdayBit) == WednesdayBit)
                {
                    weekDays += Wednesday;
                }

                if ((days & ThursdayBit) == ThursdayBit)
                {
                    weekDays += Thursday;
                }

                if ((days & FridayBit) == FridayBit)
                {
                    weekDays += Friday;
                }

                if ((days & SaturdayBit) == SaturdayBit)
                {
                    weekDays += Saturday;
                }

                if (FirstDayOfTheWeek != 0)
                {
                    if ((days & SundayBit) == SundayBit)
                    {
                        weekDays += Sunday;
                    }
                }

                return weekDays;
            }
        }

        public void setStartTimer(System.Threading.Timer timer, DateTime eventDateTime)
        {
            if (startTimer != null)
            {
                startTimer.Dispose();
                lock (Plugin.AllTimers)
                {
                    Plugin.AllTimers.Remove(startTimer);
                }
            }

            startTimer = timer;

            if (startTimer != null)
            {
                lock (Plugin.AllTimers)
                {
                    Plugin.AllTimers.Add(startTimer);
                }
            }

            startDateTime = eventDateTime;
        }

        public System.Threading.Timer getStartTimer()
        {
            return startTimer;
        }

        public DateTime getStartDateTime()
        {
            return startDateTime;
        }

        public void setEndTimer(System.Threading.Timer timer, DateTime eventDateTime)
        {
            if (endTimer != null)
            {
                endTimer.Dispose();
                lock (Plugin.AllTimers)
                {
                    Plugin.AllTimers.Remove(endTimer);
                }
            }

            endTimer = timer;

            if (endTimer != null)
            {
                lock (Plugin.AllTimers)
                {
                    Plugin.AllTimers.Add(endTimer);
                }
            }

            endDateTime = eventDateTime;
        }

        public System.Threading.Timer getEndTimer()
        {
            return endTimer;
        }

        public DateTime getEndDateTime()
        {
            return endDateTime;
        }

        public void setNextPlaylist(ScheduledPlaylist playlist)
        {
            nextScheduledPlaylist = playlist;
        }

        public ScheduledPlaylist getNextPlaylist()
        {
            return nextScheduledPlaylist;
        }

        public void setPreviousPlaylist(ScheduledPlaylist playlist)
        {
            previousScheduledPlaylist = playlist;
        }

        public ScheduledPlaylist getPreviousPlaylist()
        {
            return previousScheduledPlaylist;
        }

        public void play(object state)
        {
            if (skipPlaylist || !playPlaylist)
            {
                playNext();
                return;
            }


            if (Plugin.MbApiInterface.Player_GetPlayState() == Plugin.PlayState.Playing)
            {
                stop(null);
                return;
            }
            else
            {
                if (endTimeChecked && getEndDateTime() <= DateTime.Now)
                {
                    playNext();
                    return;
                }
            }


            Plugin.MbApiInterface.NowPlayingList_Clear();

            if (Plugin.CurrentScheduledPlaylist != null && Plugin.CurrentScheduledPlaylist.task != null)
            {
                Plugin.CurrentScheduledPlaylist.skipPlaylist = true; //Playlist has been played
                Plugin.CurrentScheduledPlaylist.task.save();
            }


            DateTime now = DateTime.Now;
            TimeSpan zeroSpan = TimeSpan.Zero;

            if (durationChecked)
            {
                DateTime durationEndDateTime = now + (duration - new DateTime(2000, 01, 01));

                if (durationEndDateTime < getEndDateTime())
                    setEndTimer(new System.Threading.Timer(new TimerCallback(stop), null, durationEndDateTime - now, zeroSpan), durationEndDateTime);
            }

            Plugin.CurrentScheduledPlaylist = this;

            if (libraryPlaylist.playlistSubPath == Plugin.CtlAutoDJ)
            {
                Plugin.MbApiInterface.Player_StartAutoDj();
            }
            else
            {
                Plugin.MbApiInterface.Player_EndAutoDj();
                Plugin.MbApiInterface.Playlist_PlayNow(libraryPlaylist.getPlaylistFullPath());
            }
        }

        public void playNext()
        {
            if (nextScheduledPlaylist == null)
            {
                if (Plugin.MbApiInterface.Player_GetPlayState() == Plugin.PlayState.Playing)
                {
                    stop(null);
                }
                else
                {
                    //Lets re-enable all playlists
                    if (Plugin.CurrentScheduledPlaylist != null)
                        ScheduledTask.TaskStopped(Plugin.CurrentScheduledPlaylist.task);
                    else
                        ScheduledTask.TaskStopped(null);


                    if (TurnOff)
                    {
                        if (Plugin.SavedSettings.closeMb)
                        {
                            ((Form)Control.FromHandle(Plugin.MbApiInterface.MB_GetWindowHandle())).Close();
                            Thread.Sleep(5000);
                        }


                        if (Plugin.SavedSettings.shutdownMethod == 1)
                        {
                            Application.SetSuspendState(PowerState.Suspend, true, true);
                        }
                        else if (Plugin.SavedSettings.shutdownMethod == 2)
                        {
                            Application.SetSuspendState(PowerState.Hibernate, true, true);
                        }
                        else //if (Plugin.SavedSettings.shutdownMethod == 3)
                        {
                            var psi = new System.Diagnostics.ProcessStartInfo("shutdown", "/s /t 0");
                            psi.CreateNoWindow = true;
                            psi.UseShellExecute = false;
                            System.Diagnostics.Process.Start(psi);
                        }
                    }
                }
            }
            else
            {
                nextScheduledPlaylist.play(null);
            }
        }

        public void stop(object state)
        {
            if (Plugin.SavedSettings.stopAfterCurrent)
                Plugin.MbApiInterface.Player_StopAfterCurrent();
            else
                Plugin.MbApiInterface.Player_Stop();
        }

        public int setupStartTimer(DateTime now, TimeSpan zeroSpan, out DateTime eventDateTime) //Returns -1 if no need to use Windows Task Sheduler
        {
            if (skipPlaylist)
            {
                eventDateTime = DateTime.MinValue;
                setStartTimer(null, eventDateTime);
                return -1;
            }


            if (playPlaylist && startTimeChecked && startDateChecked) //Both start date and time are set: 0 
            {
                eventDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);

                if (eventDateTime > now)
                {
                    skipPlaylist = false;
                    setStartTimer(new System.Threading.Timer(new TimerCallback(play), null, eventDateTime - now, zeroSpan), eventDateTime);
                    return 0;
                }
                else
                {
                    skipPlaylist = true;
                    eventDateTime = DateTime.MinValue;
                    setStartTimer(null, eventDateTime);
                    return -1;
                }

            }
            else if (playPlaylist && startTimeChecked && !startDateChecked) //Only time is set (and maybe days of week): 1
            {
                eventDateTime = new DateTime(now.Year, now.Month, now.Day, startTime.Hour, startTime.Minute, startTime.Second);

                if (eventDateTime <= now)
                {
                    eventDateTime = eventDateTime.AddDays(1);
                }

                bool playback = false;

                if (eventDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    playback = playback || (startDaysOfWeek & SundayBit) == SundayBit;
                }
                else if (eventDateTime.DayOfWeek == DayOfWeek.Monday)
                {
                    playback = playback || (startDaysOfWeek & MondayBit) == MondayBit;
                }
                else if (eventDateTime.DayOfWeek == DayOfWeek.Tuesday)
                {
                    playback = playback || (startDaysOfWeek & TuesdayBit) == TuesdayBit;
                }
                else if (eventDateTime.DayOfWeek == DayOfWeek.Wednesday)
                {
                    playback = playback || (startDaysOfWeek & WednesdayBit) == WednesdayBit;
                }
                else if (eventDateTime.DayOfWeek == DayOfWeek.Thursday)
                {
                    playback = playback || (startDaysOfWeek & ThursdayBit) == ThursdayBit;
                }
                else if (eventDateTime.DayOfWeek == DayOfWeek.Friday)
                {
                    playback = playback || (startDaysOfWeek & FridayBit) == FridayBit;
                }
                else if (eventDateTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    playback = playback || (startDaysOfWeek & SaturdayBit) == SaturdayBit;
                }

                if (playback)
                {
                    skipPlaylist = false;
                    setStartTimer(new System.Threading.Timer(new TimerCallback(play), null, eventDateTime - now, zeroSpan), eventDateTime);
                    return 1;
                }
                else
                {
                    skipPlaylist = true;
                    eventDateTime = DateTime.MinValue;
                    setStartTimer(null, eventDateTime);
                    return -1;
                }
            }
            else //Playlist should be started just after previous playlist is completed: -1
            {
                skipPlaylist = false;
                eventDateTime = DateTime.MinValue;
                setStartTimer(null, eventDateTime);
                return -1;
            }
        }

        public void setupEndTimer(DateTime now, TimeSpan zeroSpan)
        {
            if (skipPlaylist)
            {
                setEndTimer(null, DateTime.MaxValue);
                return;
            }


            DateTime eventDateTime;
            System.Threading.Timer timer;

            if (playPlaylist && endTimeChecked && endDateChecked) //Both end date and time are set
            {
                eventDateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);

                if (eventDateTime > now)
                {
                    setEndTimer(new System.Threading.Timer(new TimerCallback(stop), null, eventDateTime - now, zeroSpan), eventDateTime);
                }
                else
                {
                    setEndTimer(null, DateTime.MaxValue);
                }
            }
            else if (playPlaylist && endTimeChecked && !endDateChecked) //Only time is set
            {
                eventDateTime = new DateTime(now.Year, now.Month, now.Day, endTime.Hour, endTime.Minute, endTime.Second);

                if (eventDateTime <= now)
                    eventDateTime = eventDateTime.AddDays(1);


                timer = new System.Threading.Timer(new TimerCallback(stop), null, eventDateTime - now, zeroSpan);
                setEndTimer(timer, eventDateTime);
            }
            else //Playlist should be ended just after previous playlist is completed
            {
                setEndTimer(null, DateTime.MaxValue);
            }


            return;
        }
    }

    public class ScheduledTask
    {
        public string taskName = Plugin.CtlNewTask;
        public Guid taskGuid = Guid.NewGuid();

        public int taskOrder = 0;
        public bool playTask = false;
        public List<ScheduledPlaylist> playlists = new List<ScheduledPlaylist>();

        public static ScheduledTask Read(string taskPath)
        {
            ScheduledTask task = null;

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(taskPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = null;
            try
            {
                controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ScheduledTask));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                task = (ScheduledTask)controlsDefaultsSerializer.Deserialize(file);
            }
            catch
            {
                //Ignore...
            };

            file.Close();

            return task;
        }

        public void save(string taskPath)
        {
            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(taskPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ScheduledTask));

            controlsDefaultsSerializer.Serialize(file, this);

            file.Close();
        }

        public void save()
        {
            save(Plugin.TasksFolder + @"\" + taskGuid + "." + Plugin.TaskExtention);
        }

        public static void TaskStopped(ScheduledTask pTask)
        {
            if (pTask != null)
            {
                foreach (var playlist in pTask.playlists)
                    playlist.skipPlaylist = false;

                pTask.save();
            }


            lock (Plugin.AllTimers)
            {
                foreach (var timer in Plugin.AllTimers)
                {
                    timer.Dispose();
                }

                Plugin.AllTimers.Clear();
                Plugin.CurrentScheduledPlaylist = null;
                Plugin.MbApiInterface.NowPlayingList_Clear();
            }
        }

        public void scheduleTask(bool itsStartup)
        {
            if (!playTask)
                return;


            if (playlists.Count == 0)
                return;


            //Lets connect playlists to each other
            ScheduledPlaylist previousPlaylist = null;

            ScheduledPlaylist playlist = new ScheduledPlaylist();
            playlist.setNextPlaylist(playlists[0]);
            Plugin.CurrentScheduledPlaylist = playlist;

            //playlist = null;

            for (int i = 0; i < playlists.Count; i++)
            {
                playlist = playlists[i];
                playlist.setTask(this);

                playlist.setPreviousPlaylist(previousPlaylist);

                if (previousPlaylist != null)
                    previousPlaylist.setNextPlaylist(playlist);

                previousPlaylist = playlist;
            }

            if (playlist != null)
                playlist.setNextPlaylist(null);


            //Autoplay on startup
            if (itsStartup)
            {
                for (int i = 0; i < playlists.Count; i++)
                {
                    if (playlists[i].playPlaylist)
                    {
                        if (playlists[i].startTimeChecked)
                        {
                            break;
                        }
                        else //Play playlist on MusicBee startup
                        {
                            playlists[i].play(null);
                            break;
                        }
                    }
                }
            }


            //Playlist scheduling
            DateTime now = DateTime.Now;
            TimeSpan zeroSpan = TimeSpan.Zero;

            for (int i = 0; i < playlists.Count; i++)
            {
                playlist = playlists[i];

                if (playlist.libraryPlaylist.playlistSubPath != Plugin.CtlAutoDJ && !System.IO.File.Exists(playlist.libraryPlaylist.getPlaylistFullPath()))
                    continue;


                //Start timers
                playlist.setupStartTimer(now, zeroSpan, out _);
                //End timers
                playlist.setupEndTimer(now, zeroSpan);
            }
        }

        public void scheduleTaskViaWindowsScheduler(string musicBeeStartupPath, string turnMonitorOnPath)
        {
            if (!playTask)
                return;


            if (playlists.Count == 0)
                return;


            //Playlist scheduling
            DateTime now = DateTime.Now;
            TimeSpan zeroSpan = TimeSpan.Zero;

            for (int i = 0; i < playlists.Count; i++)
            {
                ScheduledPlaylist playlist = playlists[i];

                //Start timers
                DateTime eventDateTime;
                switch (playlist.setupStartTimer(now, zeroSpan, out eventDateTime))
                {
                    case -1:
                        break;

                    case 0:
                        if (playlist.autorunMusicBee)
                        {
                            Plugin.CreateTask(Plugin.WindowsTaskPrefix + taskGuid + Plugin.TurnMonitorOnWindowsTaskInfix + playlist.playlistOrder, turnMonitorOnPath, eventDateTime, 0xFF, !Plugin.SavedSettings.dontWakeupPCOnAutostart, true);
                            Plugin.CreateTask(Plugin.WindowsTaskPrefix + taskGuid + "-" + playlist.playlistOrder, musicBeeStartupPath + @"\MusicBee.exe", eventDateTime, 0xFF, !Plugin.SavedSettings.dontWakeupPCOnAutostart, false);
                        }
                        break;

                    case 1:
                        if (playlist.autorunMusicBee)
                        {
                            Plugin.CreateTask(Plugin.WindowsTaskPrefix + taskGuid + Plugin.TurnMonitorOnWindowsTaskInfix + playlist.playlistOrder, turnMonitorOnPath, eventDateTime, playlist.startDaysOfWeek, !Plugin.SavedSettings.dontWakeupPCOnAutostart, true);
                            Plugin.CreateTask(Plugin.WindowsTaskPrefix + taskGuid + "-" + playlist.playlistOrder, musicBeeStartupPath + @"\MusicBee.exe", eventDateTime, playlist.startDaysOfWeek, !Plugin.SavedSettings.dontWakeupPCOnAutostart, false);
                        }
                        break;
                }
            }
        }
    }
    #endregion

    #region Main module
    public partial class Plugin
    {
        #region Members
        public bool developerMode = false;
        private bool uninstalled = false;

        public static MusicBeeApiInterface MbApiInterface;
        private PluginInfo about = new PluginInfo();

        public static Form MbForm;
        public List<PluginWindowTemplate> openedForms = new List<PluginWindowTemplate>();

        public Button emptyButton = new Button();

        public static string Language;
        private static string PluginSettingsFileName;

        public static string TasksFolder;
        public const string TaskExtention = "task";

        public const int PCWakeupTimeReserve = 1; //In minutues
        public static ScheduledPlaylist CurrentScheduledPlaylist = null;

        public static string PlaylistCommonPath;

        public const string WindowsTaskPrefix = "MusicBee-";
        public const string TurnMonitorOnWindowsTaskInfix = "-TurnMonitorOn-";

        public static List<System.Threading.Timer> AllTimers = new List<System.Threading.Timer>();
        #endregion

        #region Settings
        public class SavedSettingsType
        {
            public string musicBeeStartupPath;
            public string setupWindowsSchedulerTasksPath;
            public string turnMonitorOnPath;
            public bool dontWakeupPCOnAutostart;
            public int shutdownMethod; // 0 - Leave on, 1 - sleep, 2 - hibernate, 3 - power off
            public bool dontPreventPcSleep;
            public bool dontPreventMonitorSleep;

            public bool closeMb;
            public bool stopAfterCurrent;

            public bool useSkinColors;

            public List<SizePositionType> commandWindows;
        }

        public static SavedSettingsType SavedSettings;
        #endregion


        #region Other localized strings
        //Localizable strings

        public string pluginName;
        private string description;

        public string musicSchedulerCommandName;
        private string musicSchedulerCommandDescription;

        //Other localizable strings
        public static string CtlDaily;
        public static string CtlNewTask;
        public static string CtlAutoDJ;
        public static string CtlPlaylistNotFound;
        public static string MsgTaskWithThisNameAlreadyExists;
        public static string YouMightNeedToRunMusicBeeAsAdministrator;
        #endregion


        #region Common methods/functions
        public static string GetPlaylistsRoot()
        {
            string playlistCommonPath = null;

            if (MbApiInterface.Playlist_QueryPlaylists())
            {
                string playlistFullPath;
                while (!string.IsNullOrEmpty(playlistFullPath = MbApiInterface.Playlist_QueryGetNextPlaylist()))
                {
                    playlistCommonPath = Regex.Replace(playlistFullPath, @"^(.*\\Playlists)\\.*\..*", "$1");
                }
            }

            return playlistCommonPath;
        }
        #endregion

        #region Menu handlers
        public void tasksEventHandler(object sender, EventArgs e)
        {
            TasksForm tasksForm = new TasksForm(this);
            tasksForm.display();
        }
        #endregion

        #region Main plugin initialization
        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            #region General initialization
            MbApiInterface = new MusicBeeApiInterface();
            MbApiInterface.Initialise(apiInterfacePtr);
            #endregion

            #region English localization
            //Localizable strings

            //Plugin localizable strings
            pluginName = "Music Scheduler";
            description = "Plugin allows you to start/stop playing music on given time/date, etc.";
            musicSchedulerCommandName = "Music Scheduler...";
            musicSchedulerCommandDescription = "Music Scheduler: Open Music Scheduler";

            CtlDaily = "Daily";
            CtlNewTask = "<New Task>";
            CtlAutoDJ = "<Auto-DJ>";
            CtlPlaylistNotFound = "<Playlist not found>";
            MsgTaskWithThisNameAlreadyExists = "Task with name '%%taskName%%' already exists!";
            YouMightNeedToRunMusicBeeAsAdministrator = "You might need to run MusicBee as administrator";

            ScheduledPlaylist.Sunday = "Su\u2009";
            ScheduledPlaylist.Monday = "Mo\u2009";
            ScheduledPlaylist.Tuesday = "Tu\u2009";
            ScheduledPlaylist.Wednesday = "We\u2009";
            ScheduledPlaylist.Thursday = "Th\u2009";
            ScheduledPlaylist.Friday = "Fr\u2009";
            ScheduledPlaylist.Saturday = "Sa\u2009";

            PlaylistCommonPath = GetPlaylistsRoot();

            SavedSettings = new SavedSettingsType();

            //Lets set initial defaults
            SavedSettings.dontWakeupPCOnAutostart = false;
            SavedSettings.shutdownMethod = 0;
            SavedSettings.commandWindows = new List<SizePositionType>();
            #endregion

            LoadSettings(MbApiInterface.Setting_GetPersistentStoragePath());

            #region Resetting invalid/absent settings
            if (SavedSettings == null)
                SavedSettings = new SavedSettingsType();

            if (SavedSettings.commandWindows == null)
                SavedSettings.commandWindows = new List<SizePositionType>();
            #endregion


            #region Localizations
            Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            if (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday)
                ScheduledPlaylist.FirstDayOfTheWeek = 1;

            if (!System.IO.File.Exists(Application.StartupPath + @"\Plugins\ru\mb_MusicScheduler.resources.dll")) Language = "en"; //For testing

            if (Language == "ru")
            {
                //Lets redefine localizable strings

                //Plugin localizable strings
                pluginName = "Планировщик воспроизведения музыки";
                description = "Плагин позволяет вам запускать/останавливать воспроизведение музыки по заданному времени и т.д. и т.п.";
                musicSchedulerCommandName = "Планировщик воспроизведения музыки...";
                musicSchedulerCommandDescription = "Планировщик воспроизведения музыки: Открыть планировщик";

                CtlDaily = "Ежедневно";
                CtlNewTask = "<Новая задача>";
                CtlAutoDJ = "<Авто-DJ>";
                CtlPlaylistNotFound = "<Плейлист не найден>";
                MsgTaskWithThisNameAlreadyExists = "Задача с названием '%%taskName%%' уже существует!";
                YouMightNeedToRunMusicBeeAsAdministrator = "Возможно, надо запустить MusicBee от имени администратора";

                ScheduledPlaylist.Sunday = "Вс\u2009";
                ScheduledPlaylist.Monday = "Пн\u2009";
                ScheduledPlaylist.Tuesday = "Вт\u2009";
                ScheduledPlaylist.Wednesday = "Ср\u2009";
                ScheduledPlaylist.Thursday = "Чт\u2009";
                ScheduledPlaylist.Friday = "Пт\u2009";
                ScheduledPlaylist.Saturday = "Сб\u2009";
            }
            else
            {
                Language = "en";
            }
            #endregion


            #region Final initialization
            //Final initialization
            MbForm = (Form)Form.FromHandle(MbApiInterface.MB_GetWindowHandle());


            MbApiInterface.MB_AddMenuItem("mnuTools/" + musicSchedulerCommandName, musicSchedulerCommandDescription, tasksEventHandler);

            if (System.IO.File.Exists(Application.StartupPath + @"\Plugins\DevMode.txt"))
                developerMode = true;


            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = pluginName;
            about.Description = description;
            about.Author = "boroda";
            about.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            about.Type = PluginType.General;
            about.VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;  // .net version
            about.VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor; // plugin version
            about.Revision = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build; // number of days since 2000-01-01 at build time
            about.MinInterfaceVersion = 20;
            about.MinApiRevision = 39;
            about.ReceiveNotifications = ReceiveNotificationFlags.PlayerEvents;
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            #endregion

            return about;
        }
        #endregion

        #region Other plugin interface methods
        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            //string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window

            return false;
        }

        public static void LoadSettings(string appDataFolder)
        {
            //Lets try to read defaults for controls from settings file
            PluginSettingsFileName = System.IO.Path.Combine(appDataFolder, "mb_MusicScheduler.Settings.xml");

            TasksFolder = System.IO.Path.Combine(appDataFolder, "Music Scheduler Tasks");
            if (!System.IO.Directory.Exists(TasksFolder))
                System.IO.Directory.CreateDirectory(TasksFolder);

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(PluginSettingsFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = null;
            try
            {
                controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                SavedSettings = (SavedSettingsType)controlsDefaultsSerializer.Deserialize(file);
            }
            catch
            {
                //Ignore...
            };

            file.Close();
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            string setupWindowsSchedulerTasksPath = Application.StartupPath + @"\Plugins\SetupWindowsSchedulerTasks.exe";
            string turnMonitorOnPath = Application.StartupPath + @"\Plugins\TurnMonitorOn.exe";
            if (!System.IO.File.Exists(setupWindowsSchedulerTasksPath))
            {
                setupWindowsSchedulerTasksPath = MbApiInterface.Setting_GetPersistentStoragePath() + @"Plugins\SetupWindowsSchedulerTasks.exe";
                turnMonitorOnPath = MbApiInterface.Setting_GetPersistentStoragePath() + @"Plugins\TurnMonitorOn.exe";
            }

            SavedSettings.musicBeeStartupPath = Application.StartupPath; //For use in "SetupWindowsSchedulerTasks.exe"
            SavedSettings.setupWindowsSchedulerTasksPath = setupWindowsSchedulerTasksPath; //To run "SetupWindowsSchedulerTasks.exe"
            SavedSettings.turnMonitorOnPath = turnMonitorOnPath; //For use in "SetupWindowsSchedulerTasks.exe"

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(PluginSettingsFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));

            controlsDefaultsSerializer.Serialize(file, SavedSettings);

            file.Close();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            lock (AllTimers)
            {
                foreach (var timer in AllTimers)
                {
                    timer.Dispose();
                }

                AllTimers.Clear();
            }


            if (reason != PluginCloseReason.StopNoUnload)
            {
                emptyButton.Dispose();
                emptyButton = null;
            }

            if (!uninstalled && reason != PluginCloseReason.StopNoUnload)
                SaveSettings();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            //Delete tasks
            if (System.IO.Directory.Exists(TasksFolder))
            {
                System.IO.Directory.Delete(TasksFolder, true);
            }

            //Delete settings file
            if (System.IO.File.Exists(PluginSettingsFileName))
            {
                System.IO.File.Delete(PluginSettingsFileName);
            }

            uninstalled = true;
        }

        private void scheduleTasks()
        {
            string[] taskFiles = System.IO.Directory.GetFiles(TasksFolder, "*." + TaskExtention);

            for (int i = 0; i < taskFiles.Length; i++)
            {
                ScheduledTask task = ScheduledTask.Read(taskFiles[i]);
                task.scheduleTask(true);
            }
        }

        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    Close(PluginCloseReason.StopNoUnload);
                    scheduleTasks();

                    break;
            }
        }
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_SYSTEMREQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public static void PreventSleep(bool preventDisplaySleep)
        {
            if (preventDisplaySleep)
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEMREQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
            else
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEMREQUIRED);
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.PluginStartup:
                    // perform startup initialisation
                    SystemEvents.PowerModeChanged += OnPowerChange;
                    scheduleTasks();

                    break;
                case NotificationType.PlayStateChanged:
                    if (!Plugin.SavedSettings.dontPreventPcSleep && MbApiInterface.Player_GetPlayState() == PlayState.Playing)
                    {
                        PreventSleep(!Plugin.SavedSettings.dontPreventMonitorSleep);
                    }

                    if (MbApiInterface.Player_GetPlayState() == PlayState.Stopped || MbApiInterface.Player_GetPlayState() == PlayState.Paused)
                    {
                        AllowSleep();
                    }

                    if (CurrentScheduledPlaylist != null && MbApiInterface.Player_GetPlayState() == PlayState.Stopped)
                    {
                        CurrentScheduledPlaylist.playNext();
                    }

                    break;
            }
        }
        #endregion
    }
    #endregion
}
