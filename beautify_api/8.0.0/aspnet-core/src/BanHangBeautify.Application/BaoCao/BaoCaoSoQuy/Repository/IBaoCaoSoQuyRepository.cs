using Abp.Application.Services.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoSoQuy.Repository
{
    public interface IBaoCaoSoQuyRepository
    {
        public Task<PagedResultDto<BaoCaoSoQuyDto>> GetBaoCaoSoQuy_TienMat(PagedBaoCaoSoQuyRequestDto input, int tenantId);
        public Task<PagedResultDto<BaoCaoSoQuyDto>> GetBaoCaoSoQuy_NganHang(PagedBaoCaoSoQuyRequestDto input, int tenantId);
        Task<PagedResultDto<BaoCaoTaiChinh_ChiTietSoQuyDto>> GetBaoCaoTaichinh_ChiTietSoQuy(ParamSearchBaoCaoTaiChinh input);
        Task<PagedResultDto<BaoCaoChiTietCongNoDto>> GetBaoCaoChiTietCongNo(ParamSearchBaoCaoTaiChinh input);
    }
}
