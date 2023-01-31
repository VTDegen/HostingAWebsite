using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMSNotification.Startup))]
namespace SMSNotification
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
