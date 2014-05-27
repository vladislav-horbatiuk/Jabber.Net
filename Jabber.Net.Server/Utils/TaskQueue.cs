using System;
using System.Collections.Generic;
using System.Threading;

namespace Jabber.Net.Server.Utils
{
    static class TaskQueue
    {
        private static readonly IDictionary<string, Task> tasks;
        private static readonly Timer timer;


        static TaskQueue()
        {
            tasks = new Dictionary<string, Task>();
            timer = new Timer(OnTimer, null, Timeout.Infinite, Timeout.Infinite);
        }


        public static void AddTask(string id, Action task)
        {
            AddTask(id, task, TimeSpan.Zero);
        }

        public static void AddTask(string id, Action task, TimeSpan timeout)
        {
            Args.NotNull(id, "id");
            Args.NotNull(task, "task");
            Args.Requires<ArgumentOutOfRangeException>(TimeSpan.Zero <= timeout, "Invalid timeout: " + timeout);

            if (timeout == TimeSpan.Zero)
            {
                task.BeginInvoke(null, null);
                return;
            }

            lock (tasks)
            {
                tasks.Add(id, new Task(DateTime.UtcNow.Add(timeout), task));
                if (tasks.Count == 1)
                {
                    timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
                }
            }
        }

        public static void RemoveTask(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                lock (tasks)
                {
                    tasks.Remove(id);
                    if (tasks.Count == 0)
                    {
                        timer.Change(Timeout.Infinite, Timeout.Infinite);
                    }
                }
            }
        }


        private static void OnTimer(object _)
        {
            var actions = new List<Action>();
            lock (tasks)
            {
                foreach (var pair in new Dictionary<string, Task>(tasks))
                {
                    if (pair.Value.When <= DateTime.UtcNow)
                    {
                        tasks.Remove(pair.Key);
                        actions.Add(pair.Value.Action);
                    }
                }
                if (tasks.Count == 0)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }

            foreach (var action in actions)
            {
                action.BeginInvoke(null, null);
            }
        }


        private class Task
        {
            public DateTime When
            {
                get;
                private set;
            }

            public Action Action
            {
                get;
                private set;
            }

            public Task(DateTime when, Action action)
            {
                When = when;
                Action = action;
            }
        }
    }
}
