using Abp.Application.Services.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Repository;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Dto;
using BanHangBeautify.BaoCao.BaoCaoLichHen.Respository;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Dto;
using BanHangBeautify.BaoCao.BaoCaoSoQuy.Repository;
using BanHangBeautify.BaoCao.Exporting;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao
{
    public class BaoCaoAppService: SPAAppServiceBase
    {
        IBaoCaoBanHangRepository _baoCaoBanHangRepository;
        IBaoCaoLichHenRepository _baoCaoLichHenRepository;
        IBaoCaoSoQuyRepository _baoCaoSoQuyRepository;
        IBaoCaoExcelExporter _baoCaoExcelExporter;
        public BaoCaoAppService(
            IBaoCaoBanHangRepository baoCaoBanHangRepository,
            IBaoCaoExcelExporter baoCaoExcelExporter,
            IBaoCaoLichHenRepository baoCaoLichHenRepository,
            IBaoCaoSoQuyRepository baoCaoSoQuyRepository
        )
        {
            _baoCaoBanHangRepository = baoCaoBanHangRepository;
            _baoCaoExcelExporter = baoCaoExcelExporter;
            _baoCaoLichHenRepository = baoCaoLichHenRepository;
            _baoCaoSoQuyRepository = baoCaoSoQuyRepository;
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
        #endregion
    }
}
