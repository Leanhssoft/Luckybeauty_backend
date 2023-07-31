using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin.Repository
{
    public interface IKHCheckInRespository
    {
        Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking(PagedKhachHangResultRequestDto input, int? tenantId);
    }
}
