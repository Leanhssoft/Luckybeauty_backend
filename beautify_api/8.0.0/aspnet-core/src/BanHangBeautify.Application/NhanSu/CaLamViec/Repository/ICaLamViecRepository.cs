using Abp.Application.Services.Dto;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.CaLamViec.Repository
{
    public interface ICaLamViecRepository
    {
        Task<PagedResultDto<CaLamViecDto>> GetAll(PagedRequestDto input,int tenantId);
    }
}
