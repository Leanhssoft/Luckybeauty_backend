using Abp.Application.Services.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BanHangBeautify.AppCommon;


namespace BanHangBeautify.KhachHang.KhachHang.Repository
{
    public interface IKhachHangRespository
    {
        Task ImportDanhMucKhachHang(int? tenantId, long? userId, ImportExcelKhachHangDto data);
        Task<List<KhachHangView>> GetKhachHang_noBooking(PagedKhachHangResultRequestDto input, int? tenantId);
        Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input, int tenantId);
        Task<List<KhachHangView>> JqAutoCustomer(PagedKhachHangResultRequestDto input, int? tenantId);
        Task<PagedResultDto<LichSuHoaDonDto>> LichSuGiaoDich(Guid idKhachHang, int tenantId, PagedRequestDto input);
        Task<PagedResultDto<LichSuDatLichDto>> LichSuDatLich(Guid idKhachHang, int tenantId, PagedRequestDto input);
    }
}
