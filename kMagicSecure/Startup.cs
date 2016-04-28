using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(kMagicSecure.Startup))]
namespace kMagicSecure
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
