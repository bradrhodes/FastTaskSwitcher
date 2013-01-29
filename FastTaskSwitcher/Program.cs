using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

            using (SysTrayIcon stp = new SysTrayIcon(new ContextMenuBuilder(new ContextMenuItemFactory())))
            {
                stp.Display();
                Application.Run();
            }
        }
    }
}
