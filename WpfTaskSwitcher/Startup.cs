using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FastTaskSwitcher.Framework;

namespace WpfTaskSwitcher
{
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            IContainer container = new DefaultContainer();
            var runningContext = container.Resolve<IRunningContext>();


        }
    }
}
