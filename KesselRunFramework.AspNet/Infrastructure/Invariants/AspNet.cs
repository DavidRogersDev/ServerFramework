namespace KesselRunFramework.AspNet.Infrastructure.Invariants
{
    public sealed class AspNet
    {
        public sealed class Request
        {
            public const string EndpointName = "EndpointName";
            public const string FormParams = "FormParams";
            public const string Protocol = "Protocol";
            public const string QueryString = "QueryString";
            public const string Scheme = "Scheme";
        }

        public sealed class Mvc
        {
            public const string Action = "action";
            public const string ActionTemplate = "[action]";
            public const string Controller = "controller";
            public const string ControllerTemplate = "[controller]";
            public const string DefaultControllerTemplate = "v{version:apiVersion}/" + ControllerTemplate;
            public const string IdAction = ActionTemplate + "/{id:int}";
        }
    }
}
