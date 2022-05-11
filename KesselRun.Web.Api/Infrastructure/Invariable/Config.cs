namespace KesselRun.Web.Api.Infrastructure.Invariable
{
    public sealed class Config
    {
        public const string ApplicationConfigFromJson = "Application";

        public sealed class Files
        {
            public const string SerilogBaseConfig = "serilogsettings.json";
            public const string SerilogConfig = "serilogsettings.{0}.json";
        }
    }
}
