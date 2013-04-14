using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FastTaskSwitcher.Framework
{
    public interface IRunningContext
    {
        IList<TaskInfo> RunningTasks { get; }
        IList<TaskInfo> FilteredRunningTasks { get; }
        IContainer Container { get; set; }
    }

    /// <summary>
    /// This class will be used to store current context data
    /// </summary>
    public class RunningContext : IRunningContext
    {
        private readonly IList<TaskInfo> _runningTasks;
        private readonly IList<TaskInfo> _filteredRunningTasks;

        public RunningContext()
        {
            _runningTasks = new BindingList<TaskInfo>();
            _filteredRunningTasks = new List<TaskInfo>();
        }

        public IList<TaskInfo> RunningTasks
        {
            get { return _runningTasks; }
        }

        public IList<TaskInfo> FilteredRunningTasks
        {
            get { return _filteredRunningTasks; }
        }

        public IContainer Container { get; set; }
    }
}
