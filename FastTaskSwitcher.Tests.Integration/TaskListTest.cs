using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastTaskSwitcher;

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
                var taskListGetter = new EasierTaskListGetter();

                var result = taskListGetter.GetTaskList();

                Assert.IsNotNull(result);
            }
        }
    }
}
