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
            // Create the container
            IContainer container = new DefaultContainer();

            // Resolve the context, then store the container in the context so the container doesn't need to be passed
            var runningContext = container.Resolve<IRunningContext>();
            runningContext.Container = container;

            var mainWindow = new MainWindow(runningContext);

            mainWindow.Show();
        }
    }
}
