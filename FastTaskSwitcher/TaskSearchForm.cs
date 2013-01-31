using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FastTaskSwitcher
{
    public partial class TaskSearchForm : Form, ITaskSearchWindow
    {
        private readonly ITaskListGetter _taskListGetter;
        private IList<ProcessInfo> _runningTasks;
        private IList<ProcessInfo> _filteredRunningTasks; 

        private TaskSearchForm()
        {
            InitializeComponent();

            _runningTasks = new BindingList<ProcessInfo>();
            _filteredRunningTasks = _runningTasks.ToList();
            this.listBox.DataSource = _filteredRunningTasks;

            this.listBox.DisplayMember = "MainWindowTitle";
        }

        public TaskSearchForm(ITaskListGetter taskListGetter) : this()
        {
            if (taskListGetter == null) throw new ArgumentNullException("taskListGetter");
            _taskListGetter = taskListGetter;
        }

        private void TaskSearchForm_Load(object sender, EventArgs e)
        {
            // Create a copy of the running tasks at the time the form is loaded
            _runningTasks = _taskListGetter.GetTaskList().ToList();
            _filteredRunningTasks = _runningTasks.ToList();

            this.listBox.DataSource = _filteredRunningTasks;

            SetForeground();
        }

        public void SetForeground()
        {
            WinApi.SetForegroundWindow(this.Handle);
        }

        public event EventHandler SearchEvent;
        public event EventHandler TaskSwitchEvent;
        public event EventHandler CancelEvent;

        public string SearchText
        {
            get { return this.searchBox.Text.Trim(); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.listBox.SelectedItem == null)
            {
                this.Close();
                return;
            }

            var hWnd = ((ProcessInfo) this.listBox.SelectedItem).MainWindowHandle;

            WinApi.ShowWindow(hWnd, 9);

            if(hWnd == WinApi.GetForegroundWindow())
                return;

            IntPtr thread1 = WinApi.GetCurrentThreadId();
            IntPtr process2;
            IntPtr thread2 = WinApi.GetWindowThreadProcessId(hWnd, out process2);

            if (thread1 != thread2)
            {
                WinApi.AttachThreadInput(thread1, thread2, 1);
                WinApi.SetForegroundWindow(hWnd);
                WinApi.AttachThreadInput(thread1, thread2, 0);
            }
            else
            {
                WinApi.SetForegroundWindow(hWnd);
            }

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
//            _filteredRunningTasks = _runningTasks.Where(x => x.MainWindowTitle.Contains(SearchText) /* | x.ProcessName.Contains(SearchText) Deprecated */).ToList(); // Deprecated
            // Case-insesitive search
            _filteredRunningTasks =
                _runningTasks.Where(
                    x => x.MainWindowTitle.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();

            this.listBox.DataSource = _filteredRunningTasks;
        }
    }
}
