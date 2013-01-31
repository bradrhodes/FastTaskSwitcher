using System;
using System.Windows.Forms;

namespace FastTaskSwitcher.ContextMenu
{
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
