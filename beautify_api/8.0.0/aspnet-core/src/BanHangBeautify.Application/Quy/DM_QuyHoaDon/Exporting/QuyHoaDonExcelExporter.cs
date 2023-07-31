using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Common;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Exporting
{
    public class QuyHoaDonExcelExporter : EpPlusExcelExporterBase, IQuyHoaDonExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public QuyHoaDonExcelExporter(
            ITempFileCacheManager tempFileCacheManager,
            ITimeZoneConverter timeZoneConverter,
            IHostingEnvironment env,
            IAbpSession abpSession) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _env = env;
        }

        public FileDto ExportDanhSachQuyHoaDon(List<GetAllQuyHoaDonItemDto> data)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"SoQuy_Export_Template.xlsx");
            string fileName = "DanhSachThuChi_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcel(excelPackage, data);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcel(ExcelPackage excelpackage, List<GetAllQuyHoaDonItemDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.LoaiPhieu);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.MaHoaDon);
                    if (item.NgayLapHoaDon.HasValue)
                    {
                        ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.NgayLapHoaDon.Value.ToString("dd/MM/yyyy hh:mm"));
                    }

                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.TenKhoanThuChi);
                    ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.TongTienThu);
                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.SHinhThucThanhToan);
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.TxtTrangThai);
                    startRow++;
                    stt++;
                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5)
        {
            var path = Path.Combine(_env.WebRootPath, $"ExcelTemplate", string.Format(fileTemplate));// string.Format(fileTemplate) = $"fileName.xlsx"
            string fileNameNew = fileName + DateTime.Now.Ticks.ToString() + ".xlsx";

            var file = new FileDto(fileNameNew, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            using (var excelPack = new ExcelPackage(new FileInfo(path)))
            {
                var ws = excelPack.Workbook.Worksheets[0];
                ws.Cells[startRow, 1].LoadFromCollection(listData);
                Save(excelPack, file);
            }
            return file;
        }
    }
}
