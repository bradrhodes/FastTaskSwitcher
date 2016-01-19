using System.Windows.Forms;

namespace FastTaskSwitcher.Core.ContextMenu
{
    public interface IContextMenuBuilder
    {
        void BuildContextMenu(ContextMenuStrip menu);
    }
}