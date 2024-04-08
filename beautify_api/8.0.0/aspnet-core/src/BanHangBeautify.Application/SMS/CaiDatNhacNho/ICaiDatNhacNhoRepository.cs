using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.CaiDatNhacNho
{
    public interface ICaiDatNhacNhoRepository
    {
        Task<List<CaiDatNhacNhoDto>> GetAllCaiDatNhacNho();
        Task<List<CaiDatNhacNho_GroupLoaiTinDto>> GetAllCaiDatNhacNho_GroupLoaiTin();
    }
}
