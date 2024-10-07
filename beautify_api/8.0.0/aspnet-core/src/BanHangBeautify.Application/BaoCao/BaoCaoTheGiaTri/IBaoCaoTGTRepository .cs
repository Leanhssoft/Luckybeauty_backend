using Abp.Application.Services.Dto;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoTheGiaTri
{
    public interface IBaoCaoTGTRepository
    {
        Task<PagedResultDto<NhatKySuDungTGTDto>> GetNhatKySuDungTGT_ChiTiet(ParamSearchNhatKyGDV param);
    }
}
