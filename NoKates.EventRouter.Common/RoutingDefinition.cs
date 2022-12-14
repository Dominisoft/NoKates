namespace NoKates.EventRouter.Common
{
    public class RoutingDefinition
    {
        public string DefinitionName { get; set; }
        public string RoutingKey { get; set; }
        public string RequestType { get; set; }
        public string RequestBody { get; set; }
        public string RequestUri { get; set; }
    }
}
