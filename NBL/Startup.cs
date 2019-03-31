using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NBL.Startup))]
namespace NBL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
