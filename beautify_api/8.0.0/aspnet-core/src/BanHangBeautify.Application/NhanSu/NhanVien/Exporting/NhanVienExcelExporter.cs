using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;

namespace BanHangBeautify.NhanSu.NhanVien.Exporting
{
    public class NhanVienExcelExporter : EpPlusExcelExporterBase, INhanVienExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public NhanVienExcelExporter(
            ITempFileCacheManager tempFileCacheManager,
            ITimeZoneConverter timeZoneConverter,
            IHostingEnvironment env,
            IAbpSession abpSession

        ) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _env = env;
        }

        public FileDto ExportDanhSachNhanVien(List<NhanSuItemDto> data)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"NhanVien_Export_Template.xlsx");
            string fileName = "DanhSachNhanVien_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcel(excelPackage, data);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcel(ExcelPackage excelpackage, List<NhanSuItemDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.MaNhanVien);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.TenNhanVien);
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.SoDienThoai);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.DiaChi);
                    if (!string.IsNullOrWhiteSpace(item.NgaySinh.ToString()))
                    {
                        ws.Cells[startRow, 6].Value = ConvertHelper.ToDateTime(item.NgaySinh).ToString("dd/MM/yyyy");
                    }
                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.GioiTinh == 1 ? "Nam" : "Nữ");
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.TenChucVu);
                    ws.Cells[startRow, 9].Value = ConvertHelper.ToString(item.CCCD);
                    ws.Cells[startRow, 10].Value = ConvertHelper.ToString(item.NoiCap);
                    ws.Cells[startRow, 11].Value = ConvertHelper.ToString(item.NgayCap);
                    if (!string.IsNullOrWhiteSpace(item.NgayVaoLam.ToString()))
                    {
                        ws.Cells[startRow, 12].Value = ConvertHelper.ToDateTime(item.NgayVaoLam).ToString("dd/MM/yyyy");
                    }
                    startRow++;
                    stt++;
                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 12].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 12].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
