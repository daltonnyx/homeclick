using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Homeclick.Startup))]
namespace Homeclick
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
