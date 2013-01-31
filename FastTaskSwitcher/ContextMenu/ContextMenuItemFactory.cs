using System;

namespace FastTaskSwitcher.ContextMenu
{
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
}