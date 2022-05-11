using System.Collections.Generic;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public sealed class JwtSettings
    {
        public double ClockSkew { get; set; }
        public IEnumerable<string> Issuers { get; set; }
        public string CertificateThumbprint { get; set; }
    }
}
