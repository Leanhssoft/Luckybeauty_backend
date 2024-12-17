using BanHangBeautify.AppCommon;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using BanHangBeautify.Consts;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.Exporting
{
    public class BaoCaoExcelExporter : EpPlusExcelExporterBase, IBaoCaoExcelExporter
    {
        private readonly IHostingEnvironment _env;
        public BaoCaoExcelExporter(ITempFileCacheManager tempFileCacheManager, IHostingEnvironment env) : base(tempFileCacheManager)
        {
            _env = env;
        }

        public FileDto ExportBaoCaoBanHangChiTiet(List<BaoCaoBanHangChiTietDto> model)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"BaoCaoBanHangChiTiet_Export_Template.xlsx");
            string fileName = "BaoCaoBanHangChiTiet_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcelBanHangChiTiet(excelPackage, model);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcelBanHangChiTiet(ExcelPackage excelpackage, List<BaoCaoBanHangChiTietDto> input, int startRow = 5)
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
                    if (!string.IsNullOrWhiteSpace(item.NgayLapHoaDon.ToString()))
                    {
                        ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.NgayLapHoaDon.ToString("dd/MM/yyyy"));
                    }
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.TenKhachHang);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.SoDienThoai);
                    ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.TenNhomHang);

                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.TenHangHoa);
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.DonGiaTruocCK);
                    ws.Cells[startRow, 9].Value = ConvertHelper.ToString(item.SoLuong);
                    ws.Cells[startRow, 10].Value = ConvertHelper.ToString(item.ThanhTienTruocCK);
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
        public FileDto ExportBaoCaoBanHangTongHop(List<BaoCaoBanHangTongHopDto> model)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"BaoCaoBanHangTongHop_Export_Template.xlsx");
            string fileName = "BaoCaoBanHangTongHop_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcelBanHangTongHop(excelPackage, model);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcelBanHangTongHop(ExcelPackage excelpackage, List<BaoCaoBanHangTongHopDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.TenHangHoa);
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.MaHangHoa);
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.TenNhomHang);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.ThanhTienTruocCK);
                    ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.giaVon);
                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.SoLuong);
                    ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.ThanhTienSauCK);
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

        public FileDto ExportBaoCaoLichHen(List<BaoCaoLichHenDto> model)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"BaoCaoLichHen_Export_Template.xlsx");
            string fileName = "BaoCaoLichHen_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {

                BuildExcelLichHen(excelPackage, model);
                Save(excelPackage, file);
            }
            return file;
        }
        private void BuildExcelLichHen(ExcelPackage excelpackage, List<BaoCaoLichHenDto> input, int startRow = 5)
        {
            int firstRow = startRow;
            int stt = 1;
            try
            {
                ExcelWorksheet ws = excelpackage.Workbook.Worksheets[0];
                foreach (var item in input)
                {
                    ws.Cells[startRow, 1].Value = stt.ToString();
                    ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.BookingDate.ToString("dd/MM/yyyy HH:mm"));
                    ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.TenKhachHang);
                    ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.SoDienThoai);
                    ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.TenHangHoa);
                    switch (item.TrangThai)
                    {
                        case TrangThaiBookingConst.DatLich:
                            ws.Cells[startRow, 6].Value = ConvertHelper.ToString("Đặt lịch");
                            break;
                        case TrangThaiBookingConst.DaXacNhan:
                            ws.Cells[startRow, 6].Value = ConvertHelper.ToString("Đã xác nhận");
                            break;
                        case TrangThaiBookingConst.CheckIn:
                            ws.Cells[startRow, 6].Value = ConvertHelper.ToString("Checkin");
                            break;
                        case TrangThaiBookingConst.HoanThanh:
                            ws.Cells[startRow, 6].Value = ConvertHelper.ToString("Hoàn thành");
                            break;
                        case TrangThaiBookingConst.Huy:
                            ws.Cells[startRow, 6].Value = ConvertHelper.ToString("Hủy");
                            break;
                        default:
                            ws.Cells[startRow, 6].Value = ConvertHelper.ToString("");
                            break;
                    }

                    ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.GhiChu);
                    startRow++;
                    stt++;
                }
                if (input.Count > 0)
                {
                    ws.Cells[firstRow, 1, startRow - 1, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[firstRow, 1, startRow - 1, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileDto ExportBaoCaoSoQuy_TienMat(List<BaoCaoSoQuyDto> model)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"BaoCaoSoQuy_TienMat_Export_Template.xlsx");
            string fileName = "BaoCaoSoQuy_TienMat_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {
                int startRow = 5;
                int firstRow = 5;
                int stt = 1;
                try
                {
                    ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                    foreach (var item in model)
                    {
                        ws.Cells[startRow, 1].Value = stt.ToString();
                        ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.MaHoaDon);
                        ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.NgayLapHoaDon.ToString("dd/MM/yyyy"));
                        ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.TienThu);
                        ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.TienChi);
                        ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.TonLuyKe);
                        ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.NguoiNop);
                        ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.GhiChu);
                        startRow++;
                        stt++;
                    }
                    if (model.Count > 0)
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
                Save(excelPackage, file);
            }
            return file;
        }

        public FileDto ExportBaoCaoSoQuy_NganHang(List<BaoCaoSoQuyDto> model)
        {
            var pathTemplate = Path.Combine(_env.WebRootPath, $"ExcelTemplate", $"BaoCaoSoQuy_NganHang_Export_Template.xlsx");
            string fileName = "BaoCaoSoQuy_NganHang_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var template = new FileInfo(pathTemplate);
            using (ExcelPackage excelPackage = new ExcelPackage(template, true))
            {
                int startRow = 5;
                int firstRow = 5;
                int stt = 1;
                try
                {
                    ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                    foreach (var item in model)
                    {
                        ws.Cells[startRow, 1].Value = stt.ToString();
                        ws.Cells[startRow, 2].Value = ConvertHelper.ToString(item.MaHoaDon);
                        ws.Cells[startRow, 3].Value = ConvertHelper.ToString(item.NgayLapHoaDon.ToString("dd/MM/yyyy"));
                        ws.Cells[startRow, 4].Value = ConvertHelper.ToString(item.TienThu);
                        ws.Cells[startRow, 5].Value = ConvertHelper.ToString(item.TienChi);
                        ws.Cells[startRow, 6].Value = ConvertHelper.ToString(item.TonLuyKe);
                        ws.Cells[startRow, 7].Value = ConvertHelper.ToString(item.NguoiNop);
                        ws.Cells[startRow, 8].Value = ConvertHelper.ToString(item.GhiChu);
                        startRow++;
                        stt++;
                    }
                    if (model.Count > 0)
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
                Save(excelPackage, file);
            }
            return file;
        }
    }
}
