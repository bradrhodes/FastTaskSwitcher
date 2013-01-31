using System.Windows.Forms;

namespace FastTaskSwitcher.ContextMenu
{
    public interface IContextMenuBuilder
    {
        void BuildContextMenu(ContextMenuStrip menu);
    }
}