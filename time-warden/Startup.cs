using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(time_warden.Startup))]
namespace time_warden
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
