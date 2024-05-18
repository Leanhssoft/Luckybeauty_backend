using BanHangBeautify.BaoKim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoKimPayment
{
    public interface IBaoKimPaymentAppService
    {
        string CreateSignature(string data);
        bool VerifySignature(string content, string signature);
        Task<string> GuiLaiThongTinGiaoDich_BaoKim(ResponseThongBaoGiaoDich data);
    }
}
