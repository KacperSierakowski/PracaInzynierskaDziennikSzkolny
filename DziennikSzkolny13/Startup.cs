using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DziennikSzkolny13.Startup))]
namespace DziennikSzkolny13
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
