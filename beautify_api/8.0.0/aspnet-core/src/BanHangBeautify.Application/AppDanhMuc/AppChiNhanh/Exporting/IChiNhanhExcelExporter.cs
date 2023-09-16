using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Exporting
{
    public interface IChiNhanhExcelExcelExporter
    {
        FileDto ExportDanhSachChiNhanh(List<ChiNhanhDto> data);
    }
}
