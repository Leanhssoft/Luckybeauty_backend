using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR.Bookings
{
    public class BookingHub : Hub
    {
        public async Task SendAppointment()
        {
            await Clients.All.SendAsync("RecieiveAppointment");
        }

    }
}
