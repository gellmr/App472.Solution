using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App472.WebUI.Startup))]
namespace App472.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
