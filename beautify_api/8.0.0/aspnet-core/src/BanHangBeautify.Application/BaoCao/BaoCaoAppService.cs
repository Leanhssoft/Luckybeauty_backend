using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppCommon;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Repository;
using BanHangBeautify.BaoCao.BaoCaoGoiDichVu;
using BanHangBeautify.BaoCao.BaoCaoHoaHong;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Respository;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Repository;
using BanHangBeautify.BaoCao.BaoCaoTheGiaTri;
using BanHangBeautify.BaoCao.Exporting;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.BaoCao
{
    [AbpAuthorize]
    public class BaoCaoAppService : SPAAppServiceBase
    {
        IBaoCaoBanHangRepository _baoCaoBanHangRepository;
        IBaoCaoLichHenRepository _baoCaoLichHenRepository;
        IBaoCaoSoQuyRepository _baoCaoSoQuyRepository;
        IBaoCaoHoaHongRepository _baoCaoHoaHongRepository;
        IBaoCaoGoiDVRepository _baocaoGDVRepository;
        IBaoCaoTGTRepository _baocaoTGTRepository;
        IBaoCaoExcelExporter _baoCaoExcelExporter;
        private readonly IExcelBase _excelBase;
        public BaoCaoAppService(
            IBaoCaoBanHangRepository baoCaoBanHangRepository,
            IBaoCaoExcelExporter baoCaoExcelExporter,
            IBaoCaoLichHenRepository baoCaoLichHenRepository,
            IBaoCaoSoQuyRepository baoCaoSoQuyRepository,
            IBaoCaoHoaHongRepository baoCaoHoaHongRepository,
            IBaoCaoGoiDVRepository baocaoGDVRepository,
            IBaoCaoTGTRepository baocaoTGTRepository,
            IExcelBase excelBase
        )
        {
            _baoCaoBanHangRepository = baoCaoBanHangRepository;
            _baoCaoExcelExporter = baoCaoExcelExporter;
            _baoCaoLichHenRepository = baoCaoLichHenRepository;
            _baoCaoSoQuyRepository = baoCaoSoQuyRepository;
            _baoCaoHoaHongRepository = baoCaoHoaHongRepository;
            _baocaoGDVRepository = baocaoGDVRepository;
            _baocaoTGTRepository = baocaoTGTRepository;
            _excelBase = excelBase;
        }
        #region Báo cáo bán hàng
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoBanHangChiTietDto>> GetBaoCaoBanHangChiTiet(ParamSearchBaoCaoBanHang input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                return await _baoCaoBanHangRepository.GetBaoCaoBanHangChiTiet(input, tenantId);
            }
            catch (Exception)
            {
                return new PagedResultDto<BaoCaoBanHangChiTietDto>()
                {
                    Items = new List<BaoCaoBanHangChiTietDto>(),
                    TotalCount = 0,
                };
            }
        }
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoBanHangTongHopDto>> GetBaoCaoBanHangTongHop(ParamSearchBaoCaoBanHang input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                return await _baoCaoBanHangRepository.GetBaoCaoBanHangTongHop(input, tenantId);
            }
            catch (Exception)
            {
                return new PagedResultDto<BaoCaoBanHangTongHopDto>()
                {
                    Items = new List<BaoCaoBanHangTongHopDto>(),
                    TotalCount = 0,
                };
            }
        }
        public async Task<FileDto> ExportBaoCaoBanHangChiTiet(ParamSearchBaoCaoBanHang input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            var data = await _baoCaoBanHangRepository.GetBaoCaoBanHangChiTiet(input, tenantId);
            List<BaoCaoBanHangChiTietDto> dataExcel = (List<BaoCaoBanHangChiTietDto>)data.Items;
            var dataNew = dataExcel.Select(x => new
            {
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.TenKhachHang,
                x.SoDienThoai,
                x.TenNhomHang,
                x.MaHangHoa,
                x.TenHangHoa,
                x.SoLuong,
                x.DonGiaTruocCK,
                x.giaVon,
                x.ThanhTienTruocCK,
                x.TienChietKhau,
                x.ThanhTienSauCK,
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoBanHangChiTiet_", @"BaoCaoBanHangChiTiet_Export_Template.xlsx", dataNew, 5, input?.ReportValueCell, 10);
        }
        public async Task<FileDto> ExportBaoCaoBanHangTongHop(ParamSearchBaoCaoBanHang input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            var data = await _baoCaoBanHangRepository.GetBaoCaoBanHangTongHop(input, tenantId);
            List<BaoCaoBanHangTongHopDto> dataExcel = (List<BaoCaoBanHangTongHopDto>)data.Items;
            var dataNew = dataExcel.Select(x => new
            {
                x.TenNhomHang,
                x.MaHangHoa,
                x.TenHangHoa,
                x.SoLuong,
                x.ThanhTienTruocCK,
                x.giaVon,
                x.TienChietKhau,
                x.ThanhTienSauCK,
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoBanHangTongHop_", @"BaoCaoBanHangTongHop_Export_Template.xlsx", dataNew, 5, input?.ReportValueCell, 10);
        }
        #endregion

        #region Báo cáo lịch hẹn
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoLichHenDto>> GetBaoCaoLichHen(PagedBaoCaoLichHenRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                return await _baoCaoLichHenRepository.GetBaoCaoLichHen(input, tenantId);
            }
            catch (Exception)
            {
                return new PagedResultDto<BaoCaoLichHenDto>()
                {
                    Items = new List<BaoCaoLichHenDto>(),
                    TotalCount = 0,
                };
            }
        }
        public async Task<FileDto> ExportBaoCaoLichHen(PagedBaoCaoLichHenRequestDto input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _baoCaoLichHenRepository.GetBaoCaoLichHen(input, tenantId);
            List<BaoCaoLichHenDto> model = new List<BaoCaoLichHenDto>();
            model = (List<BaoCaoLichHenDto>)data.Items;
            return _baoCaoExcelExporter.ExportBaoCaoLichHen(model);
        }
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoKhachHangCheckInDto>> GetBaoCaoKhachHang_CheckIn(ParamSearchBaoCaoCheckin input)
        {
            return await _baoCaoLichHenRepository.GetBaoCaoKhachHang_CheckIn(input);
        }
        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoKhachHang_CheckIn(ParamSearchBaoCaoCheckin input)
        {
            var data = await _baoCaoLichHenRepository.GetBaoCaoKhachHang_CheckIn(input);
            var dataExcel = ObjectMapper.Map<List<BaoCaoKhachHangCheckInDto>>(data.Items);
            string dateFromTo = string.Empty;

            if (input.SoNgayChuaCheckIn_From != null)
            {
                if (input.SoNgayChuaCheckIn_To != null)
                {
                    if (input.SoNgayChuaCheckIn_From == input.SoNgayChuaCheckIn_To)
                    {
                        dateFromTo = $"Thời gian: {input.SoNgayChuaCheckIn_From} ngày";
                    }
                    else
                    {
                        dateFromTo = $"Thời gian: từ {input.SoNgayChuaCheckIn_From} - {input.SoNgayChuaCheckIn_To} ngày";
                    }
                }
            }
            List<Excel_CellData> lst = new()
            {
                new Excel_CellData { RowIndex = 2, ColumnIndex = 1, CellValue = dateFromTo }
            };
            var dataNew = dataExcel.Select(x => new
            {
                x.MaKhachHang,
                x.TenKhachHang,
                x.SoDienThoai,
                x.SoLanCheckIn,
                x.NgayCheckInGanNhat,
                x.SoNgayChuaCheckIn,
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoKhachHang_CheckIn", @"BaoCao\BaoCaoKhachHang_CheckIn_Template.xlsx", dataNew, 4, lst);
        }
        #endregion

        #region Báo cáo sổ quỹ
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoSoQuyDto>> GetBaoCaoSoQuy_TienMat(PagedBaoCaoSoQuyRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                return await _baoCaoSoQuyRepository.GetBaoCaoSoQuy_TienMat(input, tenantId);
            }
            catch (Exception)
            {
                return new PagedResultDto<BaoCaoSoQuyDto>()
                {
                    Items = new List<BaoCaoSoQuyDto>(),
                    TotalCount = 0,
                };
            }
        }
        [HttpPost]
        public async Task<FileDto> ExportBaoCaoSoQuy_TienMat(PagedBaoCaoSoQuyRequestDto input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _baoCaoSoQuyRepository.GetBaoCaoSoQuy_TienMat(input, tenantId);
            List<BaoCaoSoQuyDto> model = new List<BaoCaoSoQuyDto>();
            model = (List<BaoCaoSoQuyDto>)data.Items;
            return _baoCaoExcelExporter.ExportBaoCaoSoQuy_TienMat(model);
        }
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoSoQuyDto>> GetBaoCaoSoQuy_NganHang(PagedBaoCaoSoQuyRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                return await _baoCaoSoQuyRepository.GetBaoCaoSoQuy_NganHang(input, tenantId);
            }
            catch (Exception)
            {
                return new PagedResultDto<BaoCaoSoQuyDto>()
                {
                    Items = new List<BaoCaoSoQuyDto>(),
                    TotalCount = 0,
                };
            }
        }
        [HttpPost]
        public async Task<FileDto> ExportBaoCaoSoQuy_NganHang(PagedBaoCaoSoQuyRequestDto input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _baoCaoSoQuyRepository.GetBaoCaoSoQuy_NganHang(input, tenantId);
            List<BaoCaoSoQuyDto> model = new List<BaoCaoSoQuyDto>();
            model = (List<BaoCaoSoQuyDto>)data.Items;
            return _baoCaoExcelExporter.ExportBaoCaoSoQuy_NganHang(model);
        }

        [HttpPost]
        public async Task<PagedResultDto<BaoCaoTaiChinh_ChiTietSoQuyDto>> GetBaoCaoTaichinh_ChiTietSoQuy(ParamSearchBaoCaoTaiChinh input)
        {
            return await _baoCaoSoQuyRepository.GetBaoCaoTaichinh_ChiTietSoQuy(input);
        }

        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoTaichinh_ChiTietSoQuy(ParamSearchBaoCaoTaiChinh input)
        {
            var data = await _baoCaoSoQuyRepository.GetBaoCaoTaichinh_ChiTietSoQuy(input);
            var dataExcel = ObjectMapper.Map<List<BaoCaoTaiChinh_ChiTietSoQuyDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.MaPhieuThuChi,
                x.NgayLapPhieu,
                x.TenNguoiNopTien,
                x.MaHoaDonLienQuans,
                x.Thu_TienMat,
                x.Thu_TienChuyenKhoan,
                x.Thu_TienQuyetThe,
                x.Chi_TienMat,
                x.Chi_TienChuyenKhoan,
                x.Chi_TienQuyetThe,
                x.TienThu,
                x.TienChi,
                x.TongThuChi,
                x.NoiDungThu,
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoTaiChinh_ChiTietSoQuy", @"BaoCao\BaoCaoTaiChinh_ChiTietSoQuy_Template.xlsx", dataNew, 5);
        }

        [HttpPost]
        public async Task<PagedResultDto<BaoCaoChiTietCongNoDto>> GetBaoCaoChiTietCongNo(ParamSearchBaoCaoTaiChinh input)
        {
            return await _baoCaoSoQuyRepository.GetBaoCaoChiTietCongNo(input);
        }
        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoChiTietCongNo(ParamSearchBaoCaoTaiChinh input)
        {
            var data = await _baoCaoSoQuyRepository.GetBaoCaoChiTietCongNo(input);
            var dataExcel = ObjectMapper.Map<List<BaoCaoChiTietCongNoDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.MaKhachHang,
                x.TenKhachHang,
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.TenHangHoa,
                x.SoLuong,
                x.DonGiaSauVAT,
                x.ThanhTienSauVAT,
                x.TongThanhToan,
                x.KhachDaTra,
                x.ConNo,
                x.GhiChuHD
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoChiTietCongNo_", @"BaoCao\BaoCaoTaiChinh_ChiTietCongNo_Template.xlsx", dataNew, 5);
        }
        #endregion
        #region bao cao hoa hong
        [HttpPost]
        public async Task<PagedResultDto<PageBaoCaoHoaHongTongHopDto>> BaoCaoHoaHongTongHop(ParamSearchBaoCaoHoaHong input)
        {
            try
            {
                return await _baoCaoHoaHongRepository.BaoCaoHoaHongTongHop(input);
            }
            catch (Exception)
            {
                return new PagedResultDto<PageBaoCaoHoaHongTongHopDto>()
                {
                    Items = new List<PageBaoCaoHoaHongTongHopDto>(),
                    TotalCount = 0,
                };
            }
        }
        [HttpPost]
        public async Task<IActionResult> BaoCaoHoaHongChiTiet(ParamSearchBaoCaoHoaHong input)
        {
            try
            {
                var lst = await _baoCaoHoaHongRepository.BaoCaoHoaHongChiTiet(input);
                var lstGr = lst.Items.GroupBy(x => new
                {
                    x.MaHoaDon,
                    x.NgayLapHoaDon,
                    x.MaKhachHang,
                    x.TenKhachHang,
                    x.IdHoaDonChiTiet,
                    x.MaHangHoa,
                    x.TenHangHoa,
                    x.TenNhomHang,
                    x.SoLuong,
                    x.ThanhTienSauCK,
                    x.SumSoLuong,
                    x.SumThanhTienSauCK,
                    x.SumHoaHongTuVan,
                    x.SumHoaHongThucHien,
                    x.SumTongHoaHong
                }).Select(x => new
                {
                    x.Key.MaHoaDon,
                    x.Key.NgayLapHoaDon,
                    x.Key.MaKhachHang,
                    x.Key.TenKhachHang,
                    x.Key.IdHoaDonChiTiet,
                    x.Key.MaHangHoa,
                    x.Key.TenHangHoa,
                    x.Key.TenNhomHang,
                    x.Key.SoLuong,
                    x.Key.ThanhTienSauCK,
                    x.Key.SumSoLuong,
                    x.Key.SumThanhTienSauCK,
                    x.Key.SumHoaHongTuVan,
                    x.Key.SumHoaHongThucHien,
                    x.Key.SumTongHoaHong,
                    RowSpan = x.Count(),
                    lstDetail = x,
                }).OrderByDescending(x => x.NgayLapHoaDon);

                return new JsonResult(new { res = true, items = lstGr, totalCount = lst.TotalCount });
            }
            catch (Exception)
            {
                return new JsonResult(new { res = false, totalCount = 0 });
            }
        }
        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoHoaHongTongHop(ParamSearchBaoCaoHoaHong input)
        {
            var data = await _baoCaoHoaHongRepository.BaoCaoHoaHongTongHop(input);
            var dataExcel = ObjectMapper.Map<List<PageBaoCaoHoaHongTongHopDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.MaNhanVien,
                x.TenNhanVien,
                x.HoaHongThucHien_TienChietKhau,
                x.HoaHongTuVan_TienChietKhau,
                x.TongHoaHong
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoHoaHongTongHop_", @"BaoCao\BaoCaoHoaHongTongHop_Template.xlsx", dataNew, 5);
        }
        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoHoaHongChiTiet(ParamSearchBaoCaoHoaHong input)
        {
            var data = await _baoCaoHoaHongRepository.BaoCaoHoaHongChiTiet(input);
            var dataExcel = ObjectMapper.Map<List<PageBaoCaoHoaHongChiTietDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.MaKhachHang,
                x.TenKhachHang,
                x.MaHangHoa,
                x.TenHangHoa,
                x.MaNhanVien,
                x.TenNhanVien,
                x.SoLuong,
                x.ThanhTienSauCK,
                x.HoaHongThucHien_PTChietKhau,
                x.HoaHongThucHien_TienChietKhau,
                x.HoaHongTuVan_PTChietKhau,
                x.HoaHongTuVan_TienChietKhau,
                x.TongHoaHong
            }).ToList();
            return _excelBase.WriteToExcel("BaoCaoHoaHongChiTiet_", @"BaoCao\BaoCaoHoaHongChiTiet_Template.xlsx", dataNew, 6);
        }
        #endregion
        #region baocao gdv
        [HttpPost]
        public async Task<dynamic> BaoCaoSuDungGDV_ChiTiet(ParamSearch input)
        {
            try
            {
                PagedResultDto<ChiTietNhatKySuDungGDVDto> data = await _baocaoGDVRepository.BaoCaoSuDungGDV_ChiTiet(input);
                var dtGr = data.Items?.GroupBy(x => new
                {
                    x.IdGoiDV,
                    x.MaGoiDichVu,
                    x.NgayMuaGDV,
                    x.MaKhachHang,
                    x.TenKhachHang,
                    x.SoDienThoai,
                }).Select(x => new
                {
                    x.Key.IdGoiDV,
                    x.Key.MaGoiDichVu,
                    x.Key.NgayMuaGDV,
                    x.Key.MaKhachHang,
                    x.Key.TenKhachHang,
                    x.Key.SoDienThoai,
                    data.TotalCount,
                    rowSpan = x.Count(),
                    chitiets = x.ToList().OrderByDescending(o => o.IdChiTietMua)
                });
                return dtGr;
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoSuDungGDVChiTiet(ParamSearch input)
        {
            PagedResultDto<ChiTietNhatKySuDungGDVDto> data = await _baocaoGDVRepository.BaoCaoSuDungGDV_ChiTiet(input);
            var dataExcel = ObjectMapper.Map<List<ChiTietNhatKySuDungGDVDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.TenKhachHang,
                x.SoDienThoai,
                x.MaGoiDichVu,
                x.NgayMuaGDV,
                x.MaHangHoa,
                x.TenHangHoa,
                SoLuongMua = x.SoLuongMua == 0 ? "" : x.SoLuongMua.ToString(),
                DonGiaSauCK = x.SoLuongMua == 0 ? "" : x.DonGiaSauCK.ToString(),
                ThanhTienSauCK = x.SoLuongMua == 0 ? "" : x.ThanhTienSauCK.ToString(),
                x.MaHoaDonSD,
                x.NgayLapHoaDonSD,
                SoLuongDung = x.SoLuongSD == 0 ? "" : x.SoLuongSD.ToString(),
            }).ToList();
            List<Excel_CellData> lst = new()
            {
                new Excel_CellData { RowIndex = 2, ColumnIndex = 1, CellValue = $"Thời gian: {input.FromDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}" +
                    $" - {input.ToDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}"
             },
            };
            return _excelBase.WriteToExcel("BaoCaoGDV_NhatKySuDung_", @"BaoCao\BaoCaoGDV_NhatKySuDung.xlsx", dataNew, 6, lst);
        }
        #endregion

        #region bc TGT
        [HttpPost]
        public async Task<dynamic> GetNhatKySuDungTGT_ChiTiet(ParamSearchNhatKyGDV input)
        {
            try
            {
                PagedResultDto<NhatKySuDungTGTDto> data = await _baocaoTGTRepository.GetNhatKySuDungTGT_ChiTiet(input);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<FileDto> ExportToExcel_BaoCaoSuDungTGTChiTiet(ParamSearchNhatKyGDV input)
        {
            PagedResultDto<NhatKySuDungTGTDto> data = await _baocaoTGTRepository.GetNhatKySuDungTGT_ChiTiet(input);
            var dataExcel = ObjectMapper.Map<List<NhatKySuDungTGTDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.SLoaiChungTu,
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.MaKhachHang,
                x.TenKhachHang,
                x.SoDienThoai,
                x.GtriDieuChinh,
                x.PhatSinhTang,
                x.PhatSinhGiam
            }).ToList();
            List<Excel_CellData> lst = new()
            {
                new Excel_CellData { RowIndex = 2, ColumnIndex = 1, CellValue = $"Thời gian: {input.FromDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}" +
                    $" - {input.ToDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}"
             },
            };
            return _excelBase.WriteToExcel("BaoCaoGDV_NhatKySuDung_", @"BaoCao\BaoCaoTGT_NhatKySuDung.xlsx", dataNew, 6, lst);
        }

        #endregion
    }
}
