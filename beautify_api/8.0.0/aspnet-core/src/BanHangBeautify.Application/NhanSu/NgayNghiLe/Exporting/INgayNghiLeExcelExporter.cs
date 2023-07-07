using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Exporting
{
    public interface INgayNghiLeExcelExporter
    {
        FileDto ExportDanhSachNgayNghiLe(List<NgayNghiLeDto> data);
    }
}
