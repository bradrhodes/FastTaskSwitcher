using System;
using System.Windows.Forms;
using FastTaskSwitcher.ContextMenu;

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

            using (var stp = new SysTrayIcon(new ContextMenuBuilder(new ContextMenuItemFactory())))
            {
                stp.Display();
                Application.Run();
            }
        }
    }
}
