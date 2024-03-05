using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.NhanSu.NhanVien.Exporting
{
    public interface INhanVienExcelExporter
    {
        FileDto ExportDanhSachNhanVien(List<NhanSuItemDto> data);
    }
}
