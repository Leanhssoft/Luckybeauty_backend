using Abp.Application.Services.Dto;
using BanHangBeautify.Entities;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.NhomKhach
{
    public interface INhomKhachAppService
    {
        Task CreateOrEditLoaiKhach();
        Task<bool> Delete(Guid id);
        Task<ListResultDto<DM_NhomKhachHang>> GetAll();
    }
}