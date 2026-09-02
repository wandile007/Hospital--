using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Health__.Startup))]
namespace Health__
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
