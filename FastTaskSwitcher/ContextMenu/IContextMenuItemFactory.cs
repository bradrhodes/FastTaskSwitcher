namespace FastTaskSwitcher.ContextMenu
{
    public interface IContextMenuItemFactory
    {
        IContextMenuItem GetMenuItem(string menuItemName);
    }
}