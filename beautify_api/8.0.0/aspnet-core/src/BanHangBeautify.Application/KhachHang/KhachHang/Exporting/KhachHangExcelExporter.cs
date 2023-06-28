using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Common;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.DataExporting.Excel.NPOI;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using NPOI.HPSF;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Exporting
{
    public class KhachHangExcelExporter : NpoiExcelExporterBase, IKhachHangExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        public KhachHangExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        public FileDto ExportDanhSachKhachHang(List<KhachHangView> model)
        {
            return CreateExcelPackage("khach_hang_excel.xlsx", excelpackage =>
            {
                var sheet = excelpackage.CreateSheet("Khách hàng");
                AddHeader(
                    sheet,
                    "STT",
                    "Mã KH",
                    "Tên khách hàng",
                    "Số điện thoại",
                    "Địa chỉ",
                    "Ngày sinh",
                    "Giới tính",
                    "Nhóm khách",
                    "Nguồn khách",
                    "Người phụ trách",
                    "Tổng tích điểm",
                    "Cuộc hẹn gần nhất"
               );
                AddObjects( 
                    sheet, 
                    model,
                    _=>_.Id,
                    x=>x.MaKhachHang,
                    _ => _.TenKhachHang,
                    _ => _.SoDienThoai,
                    _ => _.DiaChi,
                    _ => _.NgaySinh,
                    _ => _.GioiTinh,
                    _=>_.TenNhomKhach,
                    _=>_.TenNguonKhach,
                    _=>_.NhanVienPhuTrach,
                    _=>_.TongTichDiem,
                    _=>_.CuocHenGanNhat
                );
                for (var i = 1; i <= model.Count; i++)
                {
                    //Formatting cells
                    sheet.GetRow(i).Cells[0].SetCellValue(i.ToString());
                    SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                }

                for (var i = 0; i < 11; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            });
        }
    }
}
