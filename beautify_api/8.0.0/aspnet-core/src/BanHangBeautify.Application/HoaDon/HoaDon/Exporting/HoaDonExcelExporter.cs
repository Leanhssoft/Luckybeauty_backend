using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;

namespace BanHangBeautify.HoaDon.HoaDon.Exporting
{
    public class HoaDonExcelExporter : EpPlusExcelExporterBase, IHoaDonExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public HoaDonExcelExporter(
            ITempFileCacheManager tempFileCacheManager,
            ITimeZoneConverter timeZoneConverter,
            IHostingEnvironment env,
            IAbpSession abpSession) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _env = env;
        }

        public FileDto ExportDanhSachHoaDon(List<PageHoaDonDto> data)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"GiaoDichThanhToan_Export_Template.xlsx");
            string fileName = "DanhSachGiaoDichThanhToan_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcel(excelPackage, data);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcel(ExcelPackage excelpackage, List<PageHoaDonDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.MaHoaDon);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.NgayLapHoaDon.ToString("dd/MM/yyyy hh:mm"));
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.TenKhachHang);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.TongTienHang);
                    ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.TongGiamGiaHD);
                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.TongThanhToan);
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.DaThanhToan);
                    ws.Cells[startRow, 9].Value = ConvertHelper.ToString(item.ConNo);
                    ws.Cells[startRow, 10].Value = ConvertHelper.ToString(item.TrangThai);
                    startRow++;
                    stt++;
                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
