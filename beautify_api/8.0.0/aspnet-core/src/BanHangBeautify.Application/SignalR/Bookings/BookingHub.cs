using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR.Bookings
{
    public class BookingHub: Hub
    {
        public async Task SendAppointment()
        {
            await Clients.All.SendAsync("RecieiveAppointment");
        }
    }
}
