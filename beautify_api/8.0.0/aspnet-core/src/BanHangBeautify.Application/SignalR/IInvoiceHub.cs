using BanHangBeautify.BaoKim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR
{
    public interface IInvoiceHub
    {
        Task ThongBaoGiaDich_fromBaoKim(ResponseThongBaoGiaoDich data);
    }
}
