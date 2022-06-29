using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HTTP_5212_RNA_Group4_HospitalProject.Startup))]
namespace HTTP_5212_RNA_Group4_HospitalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
