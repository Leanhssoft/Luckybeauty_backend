﻿using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.AppCommon;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;

namespace BanHangBeautify.NhanSu.NgayNghiLe.Exporting
{
    public class NgayNghiLeExcelExporter : EpPlusExcelExporterBase, INgayNghiLeExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public NgayNghiLeExcelExporter(
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

        public FileDto ExportDanhSachNgayNghiLe(List<NgayNghiLeDto> data)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"NgayNghiLe_Export_Template.xlsx");
            string fileName = "DanhSachNgayNghiLe_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcel(excelPackage, data);
                Save(excelPackage, file);
            }
            return file;
        }

        private void BuildExcel(ExcelPackage excelpackage, List<NgayNghiLeDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.TenNgayLe);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.TuNgay.ToString("dd/MM/yyyy"));
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.DenNgay.ToString("dd/MM/yyyy"));
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.TongSoNgay);
                    startRow++;
                    stt++;
                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
