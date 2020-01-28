using Microsoft.Owin;
using Models.Startups;
using Owin;

[assembly: OwinStartupAttribute(typeof(BMS.Startup))]
namespace BMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Authentication.ConfigureAuth(app);
        }
    }
}
