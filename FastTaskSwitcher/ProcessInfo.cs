using System;
using System.Diagnostics;

namespace FastTaskSwitcher
{
    // Refactor: This should be renamed since it has more to do with tasks/windows now than processes
    public class ProcessInfo
    {
        public ProcessInfo()
        {
        }


        // Deprecated
//        public ProcessInfo(uint processId)
//        {
//            ProcessId = processId;
//        }

        // Deprecated
//        public ProcessInfo(Process process)
//        {
//            MainWindowHandle = process.MainWindowHandle;
//            MainWindowTitle = process.MainWindowTitle;
//            ProcessName = process.ProcessName;
//
//            Process = process; // TestCode
//        }

//        public uint ProcessId { get; set; }

        public IntPtr MainWindowHandle { get; set; }
        public string MainWindowTitle { get; set; }
//        public string ProcessName { get; set; } // Deprecated

        // TestCode: This should be removed
        // public Process Process { get; private set; } // Deprecated
    }
}