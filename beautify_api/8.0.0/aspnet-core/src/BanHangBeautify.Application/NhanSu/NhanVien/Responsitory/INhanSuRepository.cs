using Abp.Application.Services.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien.Responsitory
{
    public interface INhanSuRepository
    {
        Task<PagedResultDto<NhanSuItemDto>> GetAllNhanSu(PagedNhanSuRequestDto input);
    }
}
