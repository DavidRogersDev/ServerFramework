using System.Collections.Generic;

namespace KesselRunFramework.AspNet.Response
{
    public class BadRequest400Payload
    {
        public string Title { get; } = "One or more validation errors occurred.";
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}