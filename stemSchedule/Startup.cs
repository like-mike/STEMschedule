using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(stemSchedule.Startup))]
namespace stemSchedule
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
