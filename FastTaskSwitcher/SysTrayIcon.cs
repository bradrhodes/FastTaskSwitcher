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
        private int _hotKeyId;


        public SysTrayIcon(IContextMenuBuilder contextMenuBuilder) : this()
        {
            _contextMenuBuilder = contextMenuBuilder;
        }

        private SysTrayIcon()
        {
            _ni = new NotifyIcon();
            RegisterHotKey();
        }

        public void Dispose()
        {
            UnregisterHotKey();
            _ni.Dispose();
        }

        public void Display()
        {
            // Attach Left-Click Event
            _ni.MouseClick += new MouseEventHandler(niMouseClickCallback);

            _ni.Icon = Resources.FTS;
            _ni.Text = "Fast Task Switcher";
            _ni.Visible = true;

            // Attach a context menu
            _ni.ContextMenuStrip = new ContextMenuStrip();
            _contextMenuBuilder.BuildContextMenu(_ni.ContextMenuStrip);
        }

        private void RegisterHotKey()
        {
            // Refactor: Make HotKeyManager non-static so that it can be injected here instead
            _hotKeyId = HotKeyManager.RegisterHotKey(Keys.Oemtilde, KeyModifiers.Alt);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyEventCallback);
        }

        private void UnregisterHotKey()
        {
            HotKeyManager.UnregisterHotKey(_hotKeyId);
        }

        private void niMouseClickCallback(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PopSearchForm();


                // Deprecated
//                var taskSearchForm = new TaskSearchForm(new EasierTaskListGetter());
//                taskSearchForm.Show();
            }
        }

        public void HotKeyEventCallback(object sender, HotKeyEventArgs e)
        {
            PopSearchForm();
        }

        private static void PopSearchForm()
        {
            // Refactor: Form name should be retrieved from somewhere instead of being a string
            var tsf = Application.OpenForms["TaskSearchForm"];
            if (tsf == null)
            {
                var taskSearchForm = new TaskSearchForm(new EasierTaskListGetter());
                taskSearchForm.Show();
                return;
            }

            tsf.Focus();
            ((TaskSearchForm)tsf).SetForeground();
        }
    }
}
