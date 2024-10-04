using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.LoaiChungTu
{
    public interface ILoaiChungTuAppService
    {
        Task<string> GetMaChungTuNew_fromMaxMaChungTu(double maxMaChungTu, byte idLoaiChungTu);
    }
}
