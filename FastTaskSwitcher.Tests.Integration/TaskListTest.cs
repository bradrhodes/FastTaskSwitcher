using FastTaskSwitcher.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FastTaskSwitcher.Tests.Integration
{
    public class TaskListGetterTest
    {
        [TestClass]
        public class WhenGettingTheTaskList
        {
            [TestMethod]
            public void ItShouldReturnAListOfAllRunningTasks()
            {
                var taskListGetter = new TaskListGetter();

                var result = taskListGetter.GetTaskList();

                Assert.IsNotNull(result);
            }
        }
    }
}
