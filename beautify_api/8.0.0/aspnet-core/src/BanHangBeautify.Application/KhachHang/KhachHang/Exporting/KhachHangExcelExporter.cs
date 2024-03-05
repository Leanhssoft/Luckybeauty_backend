using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.AppCommon;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
namespace BanHangBeautify.KhachHang.KhachHang.Exporting
{
    public class KhachHangExcelExporter : EpPlusExcelExporterBase, IKhachHangExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public KhachHangExcelExporter(
            ITimeZoneConverter timeZoneConverter,
             IHostingEnvironment env,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _env = env;
        }
        public FileDto ExportDanhSachKhachHang(List<KhachHangView> model)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"KhachHang_Export_Template.xlsx");
            string fileName = "DanhSachKhachHang_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcel(excelPackage, model);
                Save(excelPackage, file);
            }
            return file;
        }

        public FileDto ExportDanhSachKhachHangLoi(List<CreateOrEditKhachHangDto> model)
        {
            throw new NotImplementedException();
        }

        private void BuildExcel(ExcelPackage excelpackage, List<KhachHangView> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.MaKhachHang);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.TenKhachHang);
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.SoDienThoai);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.DiaChi);
                    if (!string.IsNullOrWhiteSpace(item.NgaySinh.ToString()))
                    {
                        ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.NgaySinh.Value.ToString("dd/MM/yyyy"));
                    }
                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.GioiTinh);
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.TenNhomKhach);
                    ws.Cells[startRow, 9].Value = ConvertHelper.ToInt64(item.TongChiTieu);
                    ws.Cells[startRow, 10].Value = ConvertHelper.ToString(item.TongTichDiem);
                    if (!string.IsNullOrWhiteSpace(item.CuocHenGanNhat.ToString()))
                    {
                        ws.Cells[startRow, 11].Value = ConvertHelper.ToDateTime(item.CuocHenGanNhat);
                    }
                    startRow++;
                    stt++;
                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}