using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.EventRouter.Common
{
    public class EventPublisher
    {
        private readonly string _eventRouterUrl;

        public EventPublisher(string eventRouterUrl)
        {
            _eventRouterUrl = eventRouterUrl;
        }
        public bool PublishEvent<TObj>(string routingKey, TObj details) 
            => !HttpHelper.Post(_eventRouterUrl.Replace("{RoutingKey}",routingKey), details).IsError;
    }
}
