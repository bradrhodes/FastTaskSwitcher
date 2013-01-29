using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FastTaskSwitcher
{
    public interface IContextMenuBuilder
    {
        void BuildContextMenu(ContextMenuStrip menu);
    }

    class ContextMenuBuilder : IContextMenuBuilder
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

    public interface IContextMenuItemFactory
    {
        IContextMenuItem GetMenuItem(string menuItemName);
    }

    class ContextMenuItemFactory : IContextMenuItemFactory
    {
        public IContextMenuItem GetMenuItem(string menuItemName)
        {
            switch (menuItemName)
            {
                case "About":
                    return new AboutContextMenuItem();
                case "Exit":
                    return new ExitContextMenuItem();
            }

            throw new ArgumentException(String.Format("{0} is not a valid menu item", menuItemName));
        }
    }


    public interface IContextMenuItem
    {
        ToolStripMenuItem GetMenuItem();
        void HandleClick(object sender, EventArgs e);
    }

    class AboutContextMenuItem : IContextMenuItem
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

    class ExitContextMenuItem : IContextMenuItem
    {
        public ToolStripMenuItem GetMenuItem()
        {
            return new ToolStripMenuItem("Exit", null, this.HandleClick);
        }

        public void HandleClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
