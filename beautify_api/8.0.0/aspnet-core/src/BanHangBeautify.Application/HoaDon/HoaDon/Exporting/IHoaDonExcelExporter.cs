using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.HoaDon.HoaDon.Exporting
{
    public interface IHoaDonExcelExporter
    {
        FileDto ExportDanhSachHoaDon(List<PageHoaDonDto> data);
    }
}
