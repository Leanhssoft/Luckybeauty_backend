using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.KhachHang.KhachHang.Exporting
{
    public interface IKhachHangExcelExporter
    {
        FileDto ExportDanhSachKhachHang(List<KhachHangView> model);
        FileDto ExportDanhSachKhachHangLoi(List<CreateOrEditKhachHangDto> model);
    }
}
