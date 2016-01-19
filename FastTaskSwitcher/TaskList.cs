using System;
using System.Collections.Generic;
using System.Text;
using FastTaskSwitcher.Core;

namespace FastTaskSwitcher
{
    public interface ITaskListGetter
    {
        IEnumerable<TaskInfo> GetTaskList();
    }

    class TaskListGetter : ITaskListGetter
    {
        private readonly IList<TaskInfo> _taskList;

        public TaskListGetter()
        {
            _taskList = new List<TaskInfo>();
        }

        public IEnumerable<TaskInfo> GetTaskList()
        {
            _taskList.Clear();

            WinApi.EnumWindows(new WinApi.EnumWindowsProc(EnumWindowsProc), IntPtr.Zero);

            return _taskList;
        }

        private bool EnumWindowsProc(IntPtr hwnd, IntPtr param)
        {
            if (!IsAltTabWindow(hwnd))
                return true;

            var sb = new StringBuilder(255);
            int nLength = WinApi.GetWindowText(hwnd, sb, sb.Capacity + 1);
            string title = sb.ToString();

            if(String.IsNullOrEmpty(title))
                return true;

            var processInfo = new TaskInfo {MainWindowHandle = hwnd, MainWindowTitle = title};

            if(RunFilters(processInfo))
                _taskList.Add(processInfo);

            return true;
        }

        // Refactor: Abstract this into Chain of Responsibility or something similar
        private bool RunFilters(TaskInfo taskInfo)
        {
            if(taskInfo.MainWindowTitle.Trim() == "Program Manager")
                return false;

            return true;
        }

        private bool IsAltTabWindow(IntPtr hwnd)
        {
            IntPtr rootWindow = WinApi.GetAncestor(hwnd, 3);

            if (WalkToParent(rootWindow) == hwnd)
            {
                return true;
            }
            return false;
        }

        private static IntPtr WalkToParent(IntPtr hwnd)
        {
            IntPtr lastPopup = WinApi.GetLastActivePopup(hwnd);

            if(WinApi.IsWindowVisible(lastPopup))
                return lastPopup;

            if(lastPopup == hwnd)
                return IntPtr.Zero;

            return WalkToParent(lastPopup);
        }
    }
}
