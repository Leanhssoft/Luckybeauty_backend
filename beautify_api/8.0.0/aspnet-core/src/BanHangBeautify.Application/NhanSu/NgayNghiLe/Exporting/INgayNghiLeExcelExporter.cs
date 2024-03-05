using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Exporting
{
    public interface INgayNghiLeExcelExporter
    {
        FileDto ExportDanhSachNgayNghiLe(List<NgayNghiLeDto> data);
    }
}
