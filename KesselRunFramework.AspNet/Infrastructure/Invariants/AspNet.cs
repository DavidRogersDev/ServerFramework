namespace KesselRunFramework.AspNet.Infrastructure.Invariants
{
    public sealed class AspNet
    {
        public sealed class Mvc
        {
            public const string Action = "action";
            public const string ActionTemplate = "[action]";
            public const string Controller = "controller";
            public const string ControllerTemplate = "[controller]";
            public const string DefaultControllerTemplate = "v{version:apiVersion}/" + ControllerTemplate;
            public const string GroupIdAction = ActionTemplate + "/{groupId:int}";
            public const string CategoryIdAction = ActionTemplate + "/{categoryId:int}";
            public const string PriorityAction = ActionTemplate + "/{categoryId}/{groupId}";
            public const string JobsForDateAction = ActionTemplate + "/{date}";
            public const string JobsToDateAction = ActionTemplate + "/{startdate}/{todate}";
        }
    }
}
