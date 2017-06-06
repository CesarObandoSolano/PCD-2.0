using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Plataforma.Startup))]
namespace Plataforma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
