using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Careermatcher.Startup))]
namespace Careermatcher
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
