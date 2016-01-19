using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FastTaskSwitcher.Core;
using FastTaskSwitcher.Core.ContextMenu;
using Application = System.Windows.Application;

namespace FastTaskSwitcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            using (var stp = new SysTrayIcon(new ContextMenuBuilder(), PopWindow, null, "FastTaskSwitcher"))
            {
                stp.Display();
            }
        }

        private void PopWindow()
        {
            var searchWindow = new SearchWindow();
            searchWindow.Show();
        }
    }

    class ContextMenuBuilder : IContextMenuBuilder
    {
        public void BuildContextMenu(ContextMenuStrip menu)
        {
            menu.Items.Add(new AboutContextMenuItem().GetMenuItem());
        }
    }

    public class AboutContextMenuItem : IContextMenuItem
    {
        public ToolStripMenuItem GetMenuItem()
        {
            return new ToolStripMenuItem("About", null, this.HandleClick);
        }

        public void HandleClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
    }
}
