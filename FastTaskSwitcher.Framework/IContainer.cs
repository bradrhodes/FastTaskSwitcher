using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace FastTaskSwitcher.Framework
{
    public interface IContainer
    {
        T Resolve<T>();
    }

    public class DefaultContainer : IContainer
    {
        private readonly IWindsorContainer _windsorContainer;

        public DefaultContainer()
        {
            _windsorContainer = new WindsorContainer()
                .Install(FromAssembly.This());
        }

        // If needed, add some logic for manual resolving, for now just use Windsor
        public T Resolve<T>()
        {
            return _windsorContainer.Resolve<T>();
        }
    }
}
