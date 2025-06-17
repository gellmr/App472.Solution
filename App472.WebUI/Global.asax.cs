using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common.WebHost;
using App472.WebUI.Infrastructure.Binders;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using App472.WebUI.Domain.Entities;

namespace App472.WebUI
{

    public class MvcApplication : NinjectHttpApplication
    {
        private void InitializeDBContexts(){
            // Choose debug or release database initialization. Debug will use DropCreateDatabaseAlways
            if (StaticHelpers.IsDebugRelease){
                // System.Data.Entity.Database.SetInitializer(new App472.WebUI.App_Start.Debug.EFDBInitializer());
                System.Data.Entity.Database.SetInitializer(new App472.WebUI.App_Start.Debug.IDDBInitializer());
            }else{
                // System.Data.Entity.Database.SetInitializer(new App472.WebUI.App_Start.Release.EFDBInitializer());
                System.Data.Entity.Database.SetInitializer(new App472.WebUI.App_Start.Release.IDDBInitializer());
            }
            // Touch the context, so we trigger database inititalisation
            new IDDBContext().Database.Initialize(true); // Pass true to force initialization to run
        }

        protected override void OnApplicationStarted(){
            InitializeDBContexts();
            base.OnApplicationStarted();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
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
            HttpContext.Current.Session.Add(SessExtensions.SessUserKeyName, new NotLoggedInSessUser()); // Initiate the session user object
            HttpContext.Current.Session.Add(SessExtensions.TRUrlsSessKeyName, new TabReturnUrls()); // Dictionary of Guids against return URLs for navigation. Allows back links to work if we have multiple tabs open.
        }
    }

    // See https://stackoverflow.com/questions/1611410/how-to-check-if-a-app-is-in-debug-or-release
    static class StaticHelpers
    {
        public static bool IsDebugRelease{
            get{
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}
