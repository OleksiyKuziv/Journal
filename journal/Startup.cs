using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(journal.Startup))]
namespace journal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
