using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.LichSuNap_ChuyenTien
{
    public interface ILichSuNap_ChuyenTienAppService
    {
        Task<double?> GetTongSuDung_ofBrandname(int tenantId);
    }
}
