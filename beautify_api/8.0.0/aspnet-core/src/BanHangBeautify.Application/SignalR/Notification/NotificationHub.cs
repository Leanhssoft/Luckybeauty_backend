using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR.Notification
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification()
        {
            await Clients.All.SendAsync("ReceiveNotification");
        }
    }
}
