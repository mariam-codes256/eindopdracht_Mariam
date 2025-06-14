using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SnackLab.WebApp.Startup))]

namespace SnackLab.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }
    }
}
