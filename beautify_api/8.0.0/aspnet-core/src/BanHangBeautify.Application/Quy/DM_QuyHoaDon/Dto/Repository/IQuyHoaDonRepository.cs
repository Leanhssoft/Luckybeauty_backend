using Abp.Application.Services.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto.Repository
{
    public interface IQuyHoaDonRepository
    {
        Task<string> FnGetMaPhieuThuChi(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon);
        Task<PagedResultDto<GetAllQuyHoaDonItemDto>> Search(PagedQuyHoaDonRequestDto input);
        Task<List<QuyHoaDonViewItemDto>> GetNhatKyThanhToan_ofHoaDon(Guid idHoadonLienQuan);
    }
}
