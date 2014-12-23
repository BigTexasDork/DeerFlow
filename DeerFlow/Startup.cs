using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeerFlow.Startup))]
namespace DeerFlow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
