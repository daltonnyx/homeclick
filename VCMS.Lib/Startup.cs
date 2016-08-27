using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VCMS.Lib.Startup))]
namespace VCMS.Lib
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
