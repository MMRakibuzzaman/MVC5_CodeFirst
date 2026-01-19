using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1291263_MVC_CodeFirst.Startup))]
namespace _1291263_MVC_CodeFirst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
