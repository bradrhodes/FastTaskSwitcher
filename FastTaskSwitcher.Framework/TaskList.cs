using System.Collections.Generic;

namespace FastTaskSwitcher
{
    public interface ITaskListGetter
    {
        IEnumerable<TaskInfo> GetTaskList();
    }
}
