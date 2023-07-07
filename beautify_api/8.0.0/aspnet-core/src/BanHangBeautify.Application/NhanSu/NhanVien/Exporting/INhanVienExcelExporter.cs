using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien.Exporting
{
    public interface INhanVienExcelExporter
    {
        FileDto ExportDanhSachKhachHang(List<NhanSuItemDto> data);
    }
}
