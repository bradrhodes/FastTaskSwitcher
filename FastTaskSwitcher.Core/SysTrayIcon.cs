using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using FastTaskSwitcher.Core.ContextMenu;

namespace FastTaskSwitcher.Core
{
    public class SysTrayIcon : IDisposable
    {
        private readonly IContextMenuBuilder _contextMenuBuilder;
        private readonly Action _popWindow;
        private readonly Icon _icon;
        private readonly string _text;
        private readonly NotifyIcon _ni;
        private int _hotKeyId;


        public SysTrayIcon(IContextMenuBuilder contextMenuBuilder, Action popWindow, Icon icon, string text) : this()
        {
            if (popWindow == null) throw new ArgumentNullException("popWindow");
            if (icon == null) throw new ArgumentNullException("icon");
            _contextMenuBuilder = contextMenuBuilder;
            _popWindow = popWindow;
            _icon = icon;
            _text = text;
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

            _ni.Icon = _icon;
            _ni.Text = _text;
            _ni.Visible = true;

            // Attach a context menu
            _ni.ContextMenuStrip = new ContextMenuStrip();
            _contextMenuBuilder.BuildContextMenu(_ni.ContextMenuStrip);
        }

        private void RegisterHotKey()
        {
            // Refactor: Make HotKeyManager non-static so that it can be injected here instead
            _hotKeyId = HotKeyManager.RegisterHotKey(Keys.Oemtilde, KeyModifiers.Control);
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
            }
        }

        private void HotKeyEventCallback(object sender, HotKeyEventArgs e)
        {
            PopSearchForm();
        }

        private void PopSearchForm()
        {
            _popWindow();
        }
    }
}