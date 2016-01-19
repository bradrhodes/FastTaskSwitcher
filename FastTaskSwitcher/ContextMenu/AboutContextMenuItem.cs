using System;
using System.Windows.Forms;
using FastTaskSwitcher.Core.ContextMenu;

namespace FastTaskSwitcher.ContextMenu
{
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
}