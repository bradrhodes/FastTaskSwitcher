using System;
using System.Windows.Forms;
using FastTaskSwitcher.Core.ContextMenu;

namespace FastTaskSwitcher.ContextMenu
{
    public class ContextMenuBuilder : IContextMenuBuilder
    {
        private readonly IContextMenuItemFactory _contextMenuItemFactory;

        public ContextMenuBuilder(IContextMenuItemFactory contextMenuItemFactory)
        {
            if (contextMenuItemFactory == null) throw new ArgumentNullException("contextMenuItemFactory");
            _contextMenuItemFactory = contextMenuItemFactory;
        }

        public void BuildContextMenu(ContextMenuStrip menu)
        {
            menu.Items.Add(_contextMenuItemFactory.GetMenuItem("About").GetMenuItem());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(_contextMenuItemFactory.GetMenuItem("Exit").GetMenuItem());
        }
    }
}