using System;

namespace FastTaskSwitcher
{
    public interface ITaskSearchWindow
    {
        event EventHandler SearchEvent;
        event EventHandler TaskSwitchEvent;
        event EventHandler CancelEvent;
        string SearchText { get; }
    }
}