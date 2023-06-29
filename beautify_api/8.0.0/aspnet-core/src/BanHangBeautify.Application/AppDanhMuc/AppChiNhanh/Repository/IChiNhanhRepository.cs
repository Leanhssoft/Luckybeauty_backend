using Abp.Application.Services.Dto;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Repository
{
    public interface IChiNhanhRepository
    {
        public Task<PagedResultDto<ChiNhanhDto>> GetAll(PagedRequestDto input, int tenantId);
    }
}
