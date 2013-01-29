using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastTaskSwitcher.Properties;

namespace FastTaskSwitcher
{
    class SysTrayIcon : IDisposable
    {
        private readonly IContextMenuBuilder _contextMenuBuilder;
        private NotifyIcon _ni;

        public SysTrayIcon(IContextMenuBuilder contextMenuBuilder) : this()
        {
            _contextMenuBuilder = contextMenuBuilder;
        }

        private SysTrayIcon()
        {
            _ni = new NotifyIcon();
        }

        public void Dispose()
        {
            _ni.Dispose();
        }

        public void Display()
        {
            // Attach Left-Click Event
            _ni.MouseClick += new MouseEventHandler(niMouseClick);

            _ni.Icon = Resources.FTS;
            _ni.Text = "Fast Task Switcher";
            _ni.Visible = true;

            // Attach a context menu
            _ni.ContextMenuStrip = new ContextMenuStrip();
            _contextMenuBuilder.BuildContextMenu(_ni.ContextMenuStrip);
        }

        private void niMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var taskSearchForm = new TaskSearchForm(new TaskListGetter());
                taskSearchForm.Show();
            }
        }
    }
}
