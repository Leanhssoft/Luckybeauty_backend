using Abp.Application.Services.Dto;
using BanHangBeautify.AppCommon;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoGoiDichVu
{
    public interface IBaoCaoGoiDVRepository
    {
        Task<PagedResultDto<ChiTietNhatKySuDungGDVDto>> BaoCaoSuDungGDV_ChiTiet(CommonClass.ParamSearch input);
    }
}
