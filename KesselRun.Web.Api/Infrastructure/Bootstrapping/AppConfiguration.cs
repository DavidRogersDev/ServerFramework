using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config;

namespace KesselRun.Web.Api.Infrastructure.Bootstrapping
{
    public class AppConfiguration
    {
        public GeneralConfig GeneralConfig { get; set; }
        public HstsSettings HstsSettings { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }
}
