using BanHangBeautify.AppCommon;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh.Exporting
{
    public class ChiNhanhExcelExporter: EpPlusExcelExporterBase, IChiNhanhExcelExporter
    {
        private readonly IHostingEnvironment _env;
        public ChiNhanhExcelExporter(ITempFileCacheManager tempFileCacheManager, IHostingEnvironment env) : base(tempFileCacheManager)
        {
            _env = env;
        }

        public FileDto ExportDanhSachChiNhanh(List<ChiNhanhDto> data)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"ChiNhanh_Export_Template.xlsx");
            string fileName = "DanhSachChiNhanh_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcel(excelPackage, data);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcel(ExcelPackage excelpackage, List<ChiNhanhDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.MaChiNhanh);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.TenChiNhanh);
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.DiaChi);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.SoDienThoai);
                    ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.MaSoThue);
                    if (!string.IsNullOrWhiteSpace(item.NgayApDung.ToString()))
                    {
                        ws.Cells[startRow, 7].Value = ConvertHelper.ToDateTime(item.NgayApDung).ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrWhiteSpace(item.NgayApDung.ToString()))
                    {
                        ws.Cells[startRow, 8].Value = ConvertHelper.ToDateTime(item.NgayHetHan).ToString("dd/MM/yyyy");
                    }
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
    }
}
