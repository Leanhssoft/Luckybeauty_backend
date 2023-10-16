using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.Exporting
{
    public interface IBaoCaoExcelExporter
    {
        FileDto ExportBaoCaoBanHangTongHop(List<BaoCaoBanHangTongHopDto> model);
        FileDto ExportBaoCaoBanHangChiTiet(List<BaoCaoBanHangChiTietDto> model);
        FileDto ExportBaoCaoLichHen(List<BaoCaoLichHenDto> model);

        FileDto ExportBaoCaoSoQuy_TienMat(List<BaoCaoSoQuyDto> model);
        FileDto ExportBaoCaoSoQuy_NganHang(List<BaoCaoSoQuyDto> model);
    }
}
