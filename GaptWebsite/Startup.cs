using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GaptWebsite.Startup))]
namespace GaptWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
