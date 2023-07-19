using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Repository
{
    public interface IKhachHangRespository
    {
        Task<List<KhachHangView>> GetKhachHang_noBooking(PagedKhachHangResultRequestDto input,int? tenantId);
        Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input,int tenantId);
        Task<List<KhachHangView>> JqAutoCustomer(PagedKhachHangResultRequestDto input,int? tenantId);
    }
}
