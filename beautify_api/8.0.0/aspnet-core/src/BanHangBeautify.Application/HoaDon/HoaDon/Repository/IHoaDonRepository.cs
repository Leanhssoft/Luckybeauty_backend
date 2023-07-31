using Abp.Application.Services.Dto;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BanHangBeautify.HoaDon.HoaDon.Repository
{
    public interface IHoaDonRepository
    {
        Task<string> GetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime ngayLapHoaDon);
        Task<string> FnGetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon);
        Task<PagedResultDto<PageHoaDonDto>> GetListHoaDon(HoaDonRequestDto param, int? tenantId = 1);
        Task<List<PageHoaDonDto>> GetInforHoaDon_byId(Guid id);
        Task<List<PageHoaDonChiTietDto>> GetChiTietHoaDon_byIdHoaDon(Guid id);
    }
}
