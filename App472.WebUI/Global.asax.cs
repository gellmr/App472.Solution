using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using App472.Domain.Entities;
using Ninject;
using Ninject.Web.Common.WebHost;
using App472.WebUI.Infrastructure.Binders;
using App472.WebUI.Infrastructure;

namespace App472.WebUI
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted(){
            base.OnApplicationStarted();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(App472.Domain.Entities.Cart), new CartModelBinder());
        }

        protected override IKernel CreateKernel(){
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }
        private static void RegisterServices(IKernel kernel){
            // e.g. kernel.Load(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(
                new App472.WebUI.Infrastructure.NinjectDependencyResolver(kernel)
            );
        }

        // Strongly typed object in the session. See
        // https://stackoverflow.com/questions/560084/session-variables-in-asp-net-mvc
        void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add(MyExtensions.TRUrlsSessKeyName, new TabReturnUrls());
        }
    }
}
