using Microsoft.Owin;
using Owin;
using static Go_Parking.Controllers.RolesController;

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
