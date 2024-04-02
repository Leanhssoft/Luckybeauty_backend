using BanHangBeautify.Suggests.Dto;
using BanHangBeautify.Users.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests.Repository
{
    public interface ISuggestRepository
    {
        Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienThucHienDichVu(int tenantId, Guid idChiNhanh, Guid? idNhanVien);
        Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienByIdDichVu(int tenantId, Guid idChiNhanh, Guid idDichVu);
        Task<List<SuggestNhanSu>> SuggestNhanSu(int tenantId, Guid idChiNhanh);
        Task<List<SuggestLoaiChungTu>> SuggestLoaiChungTu();
    }
}
