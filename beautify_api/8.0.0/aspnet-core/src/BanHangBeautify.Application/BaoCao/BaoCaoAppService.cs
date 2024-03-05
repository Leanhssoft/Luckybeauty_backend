using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Repository;
using BanHangBeautify.BaoCao.BaoCaoHoaHong;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Respository;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Repository;
using BanHangBeautify.BaoCao.Exporting;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao
{
    [AbpAuthorize]
    public class BaoCaoAppService : SPAAppServiceBase
    {
        IBaoCaoBanHangRepository _baoCaoBanHangRepository;
        IBaoCaoLichHenRepository _baoCaoLichHenRepository;
        IBaoCaoSoQuyRepository _baoCaoSoQuyRepository;
        IBaoCaoHoaHongRepository _baoCaoHoaHongRepository;
        IBaoCaoExcelExporter _baoCaoExcelExporter;
        private readonly IExcelBase _excelBase;
        public BaoCaoAppService(
            IBaoCaoBanHangRepository baoCaoBanHangRepository,
            IBaoCaoExcelExporter baoCaoExcelExporter,
            IBaoCaoLichHenRepository baoCaoLichHenRepository,
            IBaoCaoSoQuyRepository baoCaoSoQuyRepository,
            IBaoCaoHoaHongRepository baoCaoHoaHongRepository,
            IExcelBase excelBase
        )
        {
            _baoCaoBanHangRepository = baoCaoBanHangRepository;
            _baoCaoExcelExporter = baoCaoExcelExporter;
            _baoCaoLichHenRepository = baoCaoLichHenRepository;
            _baoCaoSoQuyRepository = baoCaoSoQuyRepository;
            _baoCaoHoaHongRepository = baoCaoHoaHongRepository;
            _excelBase = excelBase;
        }
        #region Báo cáo bán hàng
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoBanHangChiTietDto>> GetBaoCaoBanHangChiTiet(PagedBaoCaoBanHangRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
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
        public async Task<PagedResultDto<BaoCaoBanHangTongHopDto>> GetBaoCaoBanHangTongHop(PagedBaoCaoBanHangRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 1;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
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
        public async Task<FileDto> ExportBaoCaoBanHangChiTiet(PagedBaoCaoBanHangRequestDto input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _baoCaoBanHangRepository.GetBaoCaoBanHangChiTiet(input, tenantId);
            List<BaoCaoBanHangChiTietDto> model = new List<BaoCaoBanHangChiTietDto>();
            model = (List<BaoCaoBanHangChiTietDto>)data.Items;
            return _baoCaoExcelExporter.ExportBaoCaoBanHangChiTiet(model);
        }
        public async Task<FileDto> ExportBaoCaoBanHangTongHop(PagedBaoCaoBanHangRequestDto input)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _baoCaoBanHangRepository.GetBaoCaoBanHangTongHop(input, tenantId);
            List<BaoCaoBanHangTongHopDto> model = new List<BaoCaoBanHangTongHopDto>();
            model = (List<BaoCaoBanHangTongHopDto>)data.Items;
            return _baoCaoExcelExporter.ExportBaoCaoBanHangTongHop(model);
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
            return _excelBase.WriteToExcel("BaoCaoChiTietCongNo", @"BaoCao\BaoCaoTaiChinh_ChiTietCongNo_Template.xlsx", dataNew, 5);
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
            return _excelBase.WriteToExcel("BaoCaoHoaHongChiTiet", @"BaoCao\BaoCaoHoaHongChiTiet_Template.xlsx", dataNew, 6);
        }
        #endregion
    }
}
