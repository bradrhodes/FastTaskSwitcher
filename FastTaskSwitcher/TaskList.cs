using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FastTaskSwitcher
{
    public interface ITaskListGetter
    {
        IEnumerable<ProcessInfo> GetTaskList();
    }

    class EasierTaskListGetter : ITaskListGetter
    {
        private IList<ProcessInfo> _taskList;

        public EasierTaskListGetter()
        {
            _taskList = new List<ProcessInfo>();
        }

        public IEnumerable<ProcessInfo> GetTaskList()
        {
            _taskList.Clear();

            WinApi.EnumWindows(new WinApi.EnumWindowsProc(EnumWindowsProc), IntPtr.Zero);

            return _taskList;
        }

        private bool EnumWindowsProc(IntPtr hwnd, IntPtr param)
        {
            if (!IsAltTabWindow(hwnd))
                return true;

            StringBuilder sb = new StringBuilder(255);
            int nLength = WinApi.GetWindowText(hwnd, sb, sb.Capacity + 1);
            string title = sb.ToString();

            if(String.IsNullOrEmpty(title))
                return true;

            _taskList.Add(new ProcessInfo() { MainWindowHandle = hwnd, MainWindowTitle = title });

            return true;
        }

        private bool IsAltTabWindow(IntPtr hwnd)
        {
            IntPtr hwndWalk = WinApi.GetAncestor(hwnd, 3);

            IntPtr hwndTry;

            while ((hwndTry = WinApi.GetLastActivePopup(hwndWalk)) != hwndTry)
            {
                if (WinApi.IsWindowVisible(hwndTry))
                    break;
                hwndWalk = hwndTry;
            }

            return hwndWalk == hwnd;
        }
    }

    internal class TaskListGetter : ITaskListGetter
    {
        private IList<ProcessInfo> _taskList;

        private IList<IntPtr> _applicationList;
        private IList<IntPtr> _backgroundList;

        public TaskListGetter()
        {
            _taskList = new List<ProcessInfo>();
            _applicationList = new List<IntPtr>();
            _backgroundList = new List<IntPtr>();
        }

        public IEnumerable<ProcessInfo> GetTaskList()
        {
            // Clear out the task list
            _taskList.Clear();
            _applicationList.Clear();
            _backgroundList.Clear();

            // Call the WinAPI to enumerate the windows
            WinApi.EnumWindows(new WinApi.EnumWindowsProc(EnumWindowsProc), IntPtr.Zero);

            CreateTaskList();

            return _taskList;
        }

        // Refactor: Abstract
        private void CreateTaskList()
        {
            CreateTasksFromHwnd(_applicationList);
            CreateTasksFromHwnd(_backgroundList);
        }

        // Refactor: Abstract
        private void CreateTasksFromHwnd(IEnumerable<IntPtr> list)
        {
            foreach (var hwnd in list)
            {
                StringBuilder sb = new StringBuilder(255);
                int nLength = WinApi.GetWindowText(hwnd, sb, sb.Capacity + 1);
                string title = sb.ToString();

                if(String.IsNullOrEmpty(title))
                    continue;

                var processInfo = new ProcessInfo();
                processInfo.MainWindowHandle = hwnd;
                processInfo.MainWindowTitle = title;

                _taskList.Add(processInfo);
            }
        }

        // Refactor: Abstract this
        private bool EnumWindowsProc(IntPtr hwnd, IntPtr param)
        {
            // Todo: Enumerate the windows
            var windowType = GetWindowType(hwnd);

            switch (windowType)
            {
                case WindowTypes.Application:
                    this._applicationList.Add(hwnd);
                    break;
                case WindowTypes.Background:
                    this._backgroundList.Add(hwnd);
                    break;
            }
            return true;
        }

        // Refactor: Copied this code from Switcher, refactor to be easier to read
        private WindowTypes GetWindowType(IntPtr hwnd)
        {
            var windowType = WindowTypes.Hidden;
            WinApi.WindowStyles windowStyles =
                (WinApi.WindowStyles) ((int) WinApi.GetWindowLongPtr(hwnd, WinApi.WindowLong.Style));
            WinApi.WindowExStyles windowExStyles =
                (WinApi.WindowExStyles) ((int) WinApi.GetWindowLongPtr(hwnd, WinApi.WindowLong.ExStyle));

            if ((windowStyles & WinApi.WindowStyles.Visible) == WinApi.WindowStyles.Visible &&
                (windowStyles & WinApi.WindowStyles.Disabled) == WinApi.WindowStyles.None)
            {
                if ((windowExStyles & WinApi.WindowExStyles.AppWindow) == WinApi.WindowExStyles.AppWindow)
                {
                    if (IsScreenVisible(hwnd))
                    {
                        windowType = WindowTypes.Application;
                    }
                }
                else
                {
                    if ((windowExStyles & WinApi.WindowExStyles.ToolWindow) == (WinApi.WindowExStyles) 0u)
                    {
                        windowType = WindowTypes.Application;
                    }
                    else
                    {
                        windowType = WindowTypes.Background;
                    }
                    if ((windowExStyles & WinApi.WindowExStyles.NoActivate) == WinApi.WindowExStyles.NoActivate)
                    {
                        windowType = WindowTypes.Hidden;
                    }
                    if (windowType != WindowTypes.Hidden)
                    {
                        IntPtr window = WinApi.GetWindow(hwnd, WinApi.GetWindowMode.Owner);
                        if (window != IntPtr.Zero &&
                            GetWindowType(window) == WindowTypes.Application)
                        {
                            windowType = WindowTypes.Hidden;
                        }
                    }
                    if (windowType != WindowTypes.Hidden && !IsScreenVisible(hwnd))
                    {
                        windowType = WindowTypes.Hidden;
                    }
                    uint num;
                    byte b;
                    WinApi.LayeredWindowFlags layeredWindowFlags;
                    if (windowType != WindowTypes.Hidden &&
                        (windowExStyles & WinApi.WindowExStyles.Layered) == WinApi.WindowExStyles.Layered &&
                        WinApi.GetLayeredWindowAttributes(hwnd, out num, out b, out layeredWindowFlags) &&
                        (layeredWindowFlags & WinApi.LayeredWindowFlags.Alpha) == WinApi.LayeredWindowFlags.Alpha &&
                        b == 0)
                    {
                        windowType = WindowTypes.Hidden;
                    }
                }
            }
            return windowType;
        }

        private static bool IsScreenVisible(IntPtr hwnd)
        {
            bool result = false;
            WinApi.Rect rect;
            if (WinApi.GetWindowRect(hwnd, out rect) && WinApi.MonitorFromWindow(hwnd, WinApi.MonitorFromWindowFlags.DefaultToNull) != IntPtr.Zero && rect.Width > 0 && rect.Height > 0)
            {
                result = true;
            }
            return result;
        }
    }


    // Cleanup
    public enum WindowTypes
    {
        Application,
        Background,
        Hidden
    }

    


    // Refactor: This class needs to be abstracted
//    class ProcessGetter : ITaskListGetter
//    {
//        public IEnumerable<ProcessInfo> GetTaskList()
//        {
//            
//        }
//
//        public IEnumerable<ProcessInfo> GetProcesses()
//        {
//			uint[] array = new uint[512];
//			int num = Marshal.SizeOf(typeof(uint));
//			uint num2;
//			if (WinApi.EnumProcesses(array, (uint)(array.Length * num), out num2))
//			{
//				int num3 = (int)(num2 / (uint)num);
//				ProcessInfo[] array2 = new ProcessInfo[num3];
//				for (int i = 0; i < num3; i++)
//				{
//					array2[i] = new ProcessInfo(array[i]);
//				}
//				return array2;
//			}
//			return new ProcessInfo[0];
//        }
//    }

//    // Cleanup
//    class TaskListGetter : ITaskListGetter
//    {
//        // Note: Setup the calls to the Windows API
//
//        /// <summary>
//        /// filter function
//        /// </summary>
//        /// <param name="hWnd"></param>
//        /// <param name="lParam"></param>
//        /// <returns></returns>
//        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);
//
//        /// <summary>
//        /// check if windows visible
//        /// </summary>
//        /// <param name="hWnd"></param>
//        /// <returns></returns>
//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        public static extern bool IsWindowVisible(IntPtr hWnd);
//
//        /// <summary>
//        /// return windows text
//        /// </summary>
//        /// <param name="hWnd"></param>
//        /// <param name="lpWindowText"></param>
//        /// <param name="nMaxCount"></param>
//        /// <returns></returns>
//        [DllImport("user32.dll", EntryPoint = "GetWindowText",
//        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);
//
//        /// <summary>
//        /// enumarator on all desktop windows
//        /// </summary>
//        /// <param name="hDesktop"></param>
//        /// <param name="lpEnumCallbackFunction"></param>
//        /// <param name="lParam"></param>
//        /// <returns></returns>
//        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
//        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
//        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);
//
//        // Refactor: This is a mess
//        public IEnumerable<ProcessInfo> GetTaskList()
//        {
////            return Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero && p.ProcessName != "explorer")
////                .Select(p => new ProcessInfo(p));
//
//            //var allProcesses = Process.GetProcesses();
//
//            var result = new List<ProcessInfo>();
//
//            var collection = new List<string>();  // Testcode
//
//            EnumDelegate filter = delegate(IntPtr hwnd, int lParam)
//                {
//                    StringBuilder strbTitle = new StringBuilder(255);
//                    int nLength = GetWindowText(hwnd, strbTitle, strbTitle.Capacity + 1);
//                    string strTitle = strbTitle.ToString();
//
//                    if (IsWindowVisible(hwnd) && string.IsNullOrEmpty(strTitle) == false)
//                    {
//                        var processInfo = new ProcessInfo();
//                        processInfo.MainWindowTitle = strTitle;
//                        processInfo.MainWindowHandle = hwnd;
//
//                        // Get other info from allProcesses
//
////                        var tempProcess =
////                            allProcesses.Where(p => p.MainWindowHandle == processInfo.MainWindowHandle).FirstOrDefault();
////
////                        processInfo.ProcessName = tempProcess == null ? String.Empty : tempProcess.ProcessName;
//                        processInfo.ProcessName = String.Empty;
//
//                        result.Add(processInfo);
//                                              
//                        collection.Add(strTitle);  // testcode
//                    }
//
//                    return true;
//                };
//
//            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
//            {
//                return result;
//            }
//
//            return result;
//        }
//    }
}
