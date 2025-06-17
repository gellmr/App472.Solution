using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Ninject;
using Moq;
using App472.WebUI.Infrastructure.Abstract;
using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Concrete;

namespace App472.WebUI.Infrastructure
{
    public class NinjectDependencyResolver: IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel k)
        {
            kernel = k;
            AddBindings();
        }
        public object GetService(Type s)
        {
            return kernel.TryGet(s);
        }
        public IEnumerable<object> GetServices(Type s)
        {
            return kernel.GetAll(s);
        }
        private void AddBindings()
        {
            kernel.Bind<IProductsRepository>().To<EFProductRepository>();
            kernel.Bind<IOrdersRepository>().To<EFOrderRepository>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument(
                "settingsArg", new EmailSettings{WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")}
            );
            kernel.Bind<IGuestRepository>().To<EFGuestRepository>();

            // Not a normal EF class... wraps AppUser and Repo access to EF objects
            kernel.Bind<IFullUserRepository>().To<App472.WebUI.Infrastructure.Concrete.FullUserRepository>();
        }
    }
}