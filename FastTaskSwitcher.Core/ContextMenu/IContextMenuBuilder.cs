using System;
using System.Windows.Forms;

namespace FastTaskSwitcher.Core.ContextMenu
{
    public interface IContextMenuBuilder
    {
        void BuildContextMenu(ContextMenuStrip menu);
    }

    class DefaultContextMenuBuilder : IContextMenuBuilder
    {
        private readonly IContextMenuBuilder _initialMenuBuilder;

        public DefaultContextMenuBuilder(IContextMenuBuilder initialMenuBuilder)
        {
            if (initialMenuBuilder == null) throw new ArgumentNullException("initialMenuBuilder");
            _initialMenuBuilder = initialMenuBuilder;
        }

        public void BuildContextMenu(ContextMenuStrip menu)
        {
            // Add any injected menu items
            _initialMenuBuilder.BuildContextMenu(menu);

            // Add a separator and Exit
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ExitContextMenuItem().GetMenuItem());
        }
    }
}