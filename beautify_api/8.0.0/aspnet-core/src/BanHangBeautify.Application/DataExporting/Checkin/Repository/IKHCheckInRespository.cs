using Abp.Application.Services.Dto;
using BanHangBeautify.DataExporting.Checkin.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DataExporting.Checkin.Repository
{
    public interface IKHCheckInRespository
    {
        Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking(PagedKhachHangResultRequestDto input, int? tenantId);
        Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking2(PagedKhachHangResultRequestDto input, int? tenantId);
    }
}
