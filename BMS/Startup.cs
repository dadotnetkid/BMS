using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BMS.Startup))]
namespace BMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
