using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Common;
using BanHangBeautify.DataExporting.Excel.EpPlus;
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
    public class KhachHangExcelExporter : EpPlusExcelExporterBase,IKhachHangExcelExporter
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
            var pathTemplate = Path.Combine(_env.WebRootPath, "ExcelTemplate", "KhachHang_Export_Template.xlsx");
            var file = new FileDto("DanhSachKhachHang.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {
                BuildExcel(excelPackage, model);
                var tempFilePath = SaveTempFile(excelPackage, file.FileName);
                file.FileName = tempFilePath;
            }
            CleanUpTempFiles();
            return file;
        }
        private void CleanUpTempFiles()
        {
            var tempFolderPath = Path.Combine(_env.ContentRootPath, "App_Data", "TempFiles");
            var tempFiles = Directory.GetFiles(tempFolderPath);

            foreach (var tempFile in tempFiles)
            {
                var fileInfo = new FileInfo(tempFile);
                if (fileInfo.LastAccessTime < DateTime.Now.AddMinutes(-5))
                {
                    fileInfo.Delete();
                }
            }
        }
        private string SaveTempFile(ExcelPackage excelPackage, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            excelPackage.SaveAs(stream);
            var tempFolderPath = Path.Combine(_env.ContentRootPath, "App_Data", "TempFiles");
            Directory.CreateDirectory(tempFolderPath);
            var tempFilePath = Path.Combine(tempFolderPath, fileName);
            File.WriteAllBytes(tempFilePath, stream.ToArray());

            return tempFilePath;
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
                        ws.Cells[startRow, 6].Value = ConvertHelper.ToDateTime(item.NgaySinh);
                    }

                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.GioiTinh);
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.TenNhomKhach);
                    ws.Cells[startRow, 9].Value = ConvertHelper.ToString(item.TenNguonKhach);
                    ws.Cells[startRow, 10].Value = ConvertHelper.ToString(item.NhanVienPhuTrach);
                    ws.Cells[startRow, 11].Value = ConvertHelper.ToInt64(item.TongChiTieu);
                    ws.Cells[startRow, 12].Value = ConvertHelper.ToString(item.TongTichDiem);
                    if (!string.IsNullOrWhiteSpace(item.CuocHenGanNhat.ToString()))
                    {
                        ws.Cells[startRow, 13].Value = ConvertHelper.ToDateTime(item.CuocHenGanNhat);
                    }
                    startRow++;
                    stt++;

                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 18].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 18].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 18].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
