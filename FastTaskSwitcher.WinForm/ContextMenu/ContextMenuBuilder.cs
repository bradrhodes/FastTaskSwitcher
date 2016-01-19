using System.Windows.Forms;
using FastTaskSwitcher.Core.ContextMenu;

namespace FastTaskSwitcher.WinForm.ContextMenu
{
    public class ContextMenuBuilder : IContextMenuBuilder
    {
        public void BuildContextMenu(ContextMenuStrip menu)
        {
            menu.Items.Add(new AboutContextMenuItem().GetMenuItem());
        }
    }
}