using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace FastTaskSwitcher.Framework.WindsorInstallers
{
    public class DefaultInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .InNamespace("FastTaskSwitcher.Framework")
                       .WithService.DefaultInterfaces());
        }
    }
}