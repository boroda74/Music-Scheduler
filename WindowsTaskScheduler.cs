using System;
using TaskScheduler;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public static void CreateTask(string taskName, string fullExePath, DateTime taskStartDateTime, byte weekDays, bool wakeToRun, bool highestLevel)
        {
            //create task service instance
            ITaskService taskService = new TaskSchedulerClass();
            taskService.Connect();
            ITaskDefinition taskDefinition = taskService.NewTask(0);
            taskDefinition.Settings.Enabled = true;
            taskDefinition.Settings.Compatibility = _TASK_COMPATIBILITY.TASK_COMPATIBILITY_V2_1;
            //taskDefinition.Settings.DeleteExpiredTaskAfter = taskStartDateTime.AddDays(1).AddMinutes(5).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            //taskDefinition.Settings.StartWhenAvailable = wakeToRun;
            taskDefinition.Settings.WakeToRun = wakeToRun;
            if (highestLevel)
            {
                taskDefinition.Principal.RunLevel = _TASK_RUNLEVEL.TASK_RUNLEVEL_HIGHEST;
            }

            //create trigger for task creation.
            ITriggerCollection _iTriggerCollection = taskDefinition.Triggers;

            ITrigger trigger;
            if (weekDays == 0xFF) //Exact date/time
            {
                //taskDefinition.Settings.DeleteExpiredTaskAfter = taskStartDateTime.AddDays(0).AddMinutes(10).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

                ITrigger _trigger = _iTriggerCollection.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_TIME);
                _trigger.StartBoundary = taskStartDateTime.Subtract(new TimeSpan(0, PCWakeupTimeReserve, 0)).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                //_trigger.EndBoundary = taskStartDateTime.AddMinutes(1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                //_trigger.Repetition.Interval = "P7D";
                trigger = _trigger;
            }
            else //Days of week /time
            {
                IWeeklyTrigger _trigger = (IWeeklyTrigger)_iTriggerCollection.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_WEEKLY);
                _trigger.StartBoundary = taskStartDateTime.Subtract(new TimeSpan(0, PCWakeupTimeReserve, 0)).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                _trigger.DaysOfWeek = weekDays;
                trigger = _trigger;
            }

            trigger.Enabled = true;

            //get actions.
            IActionCollection actions = taskDefinition.Actions;
            _TASK_ACTION_TYPE actionType = _TASK_ACTION_TYPE.TASK_ACTION_EXEC;

            //create new action
            IAction action = actions.Create(actionType);
            IExecAction execAction = action as IExecAction;
            execAction.Path = fullExePath;
            ITaskFolder rootFolder = taskService.GetFolder(@"\");

            //register task.
            rootFolder.RegisterTaskDefinition(taskName, taskDefinition, 6, null, null, _TASK_LOGON_TYPE.TASK_LOGON_NONE, null);
        }

        public static void DeleteTask(string taskName)
        {
            ITaskService taskService = new TaskSchedulerClass();
            taskService.Connect();

            ITaskFolder rootFolder = taskService.GetFolder(@"\");
            rootFolder.DeleteTask(taskName, 0);
        }

        public static void DeleteWindowsTasksByPrefix(string taskNamePrefix)
        {
            ITaskService taskService = new TaskSchedulerClass();
            taskService.Connect();

            ITaskFolder rootFolder = taskService.GetFolder(@"\");
            System.Collections.Generic.List<string> tasks = new System.Collections.Generic.List<string>();
            IRegisteredTaskCollection allTasks = rootFolder.GetTasks(0);

            foreach (IRegisteredTask task in allTasks)
            {
                if (task.Name.Length >= taskNamePrefix.Length && task.Name.Substring(0, taskNamePrefix.Length) == taskNamePrefix)
                {
                    tasks.Add(task.Name);
                }
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                rootFolder.DeleteTask(tasks[i], 0);
            }
        }
    }
}