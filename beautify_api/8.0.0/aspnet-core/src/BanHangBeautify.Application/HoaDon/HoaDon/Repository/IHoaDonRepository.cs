using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BanHangBeautify.HoaDon.HoaDon.Repository
{
    public interface IHoaDonRepository
    {
        Task<string> GetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime ngayLapHoaDon);
        Task<string> FnGetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon);
        Task<PagedResultDto<PageHoaDonDto>> GetListHoaDon(HoaDonRequestDto param, int? tenantId = 1);
    }
}
