using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Go_Parking.Startup))]
namespace Go_Parking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }

    }
}
