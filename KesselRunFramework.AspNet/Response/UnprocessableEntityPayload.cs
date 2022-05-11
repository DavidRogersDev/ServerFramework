using System.Collections.Generic;

namespace KesselRunFramework.AspNet.Response
{
    public class UnprocessableEntityPayload
    {
        public string Title { get; } = "One or more validation errors occurred.";
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
