using System;

namespace FastTaskSwitcher.Core
{
    public class TaskInfo
    {
        public IntPtr MainWindowHandle { get; set; }
        public string MainWindowTitle { get; set; }
    }
}