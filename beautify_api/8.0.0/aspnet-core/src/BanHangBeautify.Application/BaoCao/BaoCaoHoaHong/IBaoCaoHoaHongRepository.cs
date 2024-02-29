using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoHoaHong
{
    public interface IBaoCaoHoaHongRepository
    {
        Task<PagedResultDto<PageBaoCaoHoaHongTongHopDto>> BaoCaoHoaHongTongHop(ParamSearchBaoCaoHoaHong input);
        Task<PagedResultDto<PageBaoCaoHoaHongChiTietDto>> BaoCaoHoaHongChiTiet(ParamSearchBaoCaoHoaHong input);
    }
}
