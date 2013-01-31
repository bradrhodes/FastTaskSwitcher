using System;
using System.Windows.Forms;

namespace FastTaskSwitcher.ContextMenu
{
    public interface IContextMenuItem
    {
        ToolStripMenuItem GetMenuItem();
        void HandleClick(object sender, EventArgs e);
    }
}