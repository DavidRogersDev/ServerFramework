using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public class GeneralConfig
    {
        public string TimeZoneCity { get; set; }
        public IList<OpenApiInfo> OpenApiInfoList { get; set; }
    }
}
