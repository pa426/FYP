using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}