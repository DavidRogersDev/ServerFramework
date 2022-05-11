using System.Diagnostics.CodeAnalysis;

namespace KesselRunFramework.AspNet.Infrastructure.Invariants
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Swagger
    {
        public sealed class EndPoint
        {
            public const string Name = "App API {0}";
            public const string Url = "/swagger/{0}/swagger.json";
        }

        public sealed class Info
        {
            public const string ContactEmail = "your.email@youbusiness.com";
            public const string ContactName = "Your Name";
            public const string Description = "A API to serve the App application.";
            public const string Title = "App API";
        }
        public sealed class SecurityDefinition
        {
            public const string Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"";
            public const string Name = "Authorization";
        }

        public sealed class Versions
        {
            public const string v1_0 = "1.0";
            public const string v1_1 = "1.1";
            public const string VersionPrefix = "v";
        }

        public sealed class DocVersions
        {
            public const string v1_0 = "v1.0";
            public const string v1_1 = "v1.1";
        }
    }
}
