using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Ninject;
using Moq;
using App472.Domain.Abstract;
using App472.Domain.Entities;
using App472.Domain.Concrete;

using App472.WebUI.Infrastructure.Abstract;

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
            //kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<IProductsRepository>().To<EFProductRepository>();
            kernel.Bind<IOrdersRepository>().To<EFOrderRepository>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument(
                "settingsArg", new EmailSettings{WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")}
            );
            kernel.Bind<IGuestRepository>().To<EFGuestRepository>();

            //Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product{ Name = "Football", Price = 25 },
            //    new Product{ Name = "Surf board", Price = 179 },
            //    new Product{ Name = "Running shoes", Price = 95 }
            //});
            //kernel.Bind<IProductsRepository>().ToConstant(mock.Object);
        }
    }
}