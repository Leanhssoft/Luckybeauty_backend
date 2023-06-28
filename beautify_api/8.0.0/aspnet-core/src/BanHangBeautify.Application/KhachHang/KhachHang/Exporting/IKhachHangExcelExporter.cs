using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Exporting
{
    public interface IKhachHangExcelExporter
    {
        FileDto ExportDanhSachKhachHang(List<KhachHangView> model);
    }
}
