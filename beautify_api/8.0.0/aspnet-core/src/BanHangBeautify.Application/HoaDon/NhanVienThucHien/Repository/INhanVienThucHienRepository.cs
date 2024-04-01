using BanHangBeautify.HoaDon.NhanVienThucHien.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Repository
{
    public interface INhanVienThucHienRepository
    {
        Task<List<CreateOrEditNhanVienThucHienDto>> GetNhanVienThucHien_byIdHoaDon(Guid idHoaDon, Guid? idQuyHoaDon = null);
        Task<List<CreateOrEditNhanVienThucHienDto>> GetNhanVienThucHien_byIdHoaDonChiTiet(Guid idHoaDonChiTiet);
        Task<bool> UpdateNhanVienThucHien_byIdHoaDon(int? tenantId, Guid idHoaDon, List<CreateOrEditNhanVienThucHienDto> lstNV = null);
    }
}
