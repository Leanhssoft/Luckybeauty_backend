using BanHangBeautify.Suggests.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests.Repository
{
    public interface ISuggestRepository
    {
        Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienThucHienDichVu(int tenantId, Guid idChiNhanh,Guid? idNhanVien);
    }
}
