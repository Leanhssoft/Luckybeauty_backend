using Abp.Application.Services.Dto;
using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Repository
{
    public interface INgayNghiLeRepository
    {
        public Task<PagedResultDto<NgayNghiLeDto>> GetAll(PagedRequestDto input, int tenantId);
    }
}
