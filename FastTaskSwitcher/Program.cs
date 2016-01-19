using System;
using System.Windows.Forms;
using FastTaskSwitcher.ContextMenu;
using FastTaskSwitcher.Core;
using FastTaskSwitcher.Core.ContextMenu;
using FastTaskSwitcher.Properties;

namespace FastTaskSwitcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var stp = new SysTrayIcon(new ContextMenuBuilder(), PopWindow, Resources.FTS, Resources.SysTrayIcon_Display_Fast_Task_Switcher))
            {
                stp.Display();
                Application.Run();
            }
        }

        public static void PopWindow()
        {
            var tsf = Application.OpenForms["TaskSearchForm"];
            if (tsf == null)
            {
                var taskSearchForm = new TaskSearchForm(new TaskListGetter());
                taskSearchForm.Show();
                return;
            }

            tsf.Focus();
            ((TaskSearchForm)tsf).SetForeground();
        }
    }
}
