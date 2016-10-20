using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcOAuth.Startup))]
namespace MvcOAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
