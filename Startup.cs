using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FloralHaven.Startup))]
namespace FloralHaven
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
