using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Exporting
{
    public interface IHoaDonExcelExporter
    {
        FileDto ExportDanhSachHoaDon(List<PageHoaDonDto> data);
    }
}
