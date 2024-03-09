using Abp.Application.Services.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Repository
{
    public interface IBaoCaoBanHangRepository
    {
        public Task<PagedResultDto<BaoCaoBanHangChiTietDto>> GetBaoCaoBanHangChiTiet(ParamSearchBaoCaoBanHang input, int tenantId);
        public Task<PagedResultDto<BaoCaoBanHangTongHopDto>> GetBaoCaoBanHangTongHop(ParamSearchBaoCaoBanHang input, int tenantId);
    }
}
