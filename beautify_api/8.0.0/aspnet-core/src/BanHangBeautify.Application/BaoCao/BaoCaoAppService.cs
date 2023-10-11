using Abp.Application.Services.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Dto;
using BanHangBeautify.BaoCao.BaoCaoBanHang.Repository;
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
        public BaoCaoAppService(IBaoCaoBanHangRepository baoCaoBanHangRepository)
        {
            _baoCaoBanHangRepository = baoCaoBanHangRepository;
        }
        [HttpPost]
        public async Task<PagedResultDto<BaoCaoBanHangChiTietDto>> GetBaoCaoBanHangChiTiet(PagedBaoCaoBanHangRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId ?? 0;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                DateTime timeFrom = new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 01);
                DateTime timeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                return await _baoCaoBanHangRepository.GetBaoCaoBanHangChiTiet(input, tenantId, timeFrom, timeTo);
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
                int tenantId = AbpSession.TenantId ?? 0;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                DateTime timeFrom = new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 01);
                DateTime timeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                return await _baoCaoBanHangRepository.GetBaoCaoBanHangTongHop(input, tenantId, timeFrom, timeTo);
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
    }
}
