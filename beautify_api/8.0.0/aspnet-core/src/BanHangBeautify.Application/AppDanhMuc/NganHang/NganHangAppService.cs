using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.NganHang.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.NganHang
{
    [AbpAuthorize(PermissionNames.Pages_NganHang)]
    public class NganHangAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_NganHang, Guid> _nganHangRepository;
        public NganHangAppService(IRepository<DM_NganHang, Guid> nganHangRepository)
        {
            _nganHangRepository = nganHangRepository;
        }
        public async Task<NganHangDto> CreateOrEdit(CreateOrEditNganHangDto input)
        {
            var checkExist = await _nganHangRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<NganHangDto> Create(CreateOrEditNganHangDto input)
        {
            NganHangDto result = new NganHangDto();
            DM_NganHang data = new DM_NganHang();
            data = ObjectMapper.Map<DM_NganHang>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TenantId = AbpSession.TenantId ?? 1;
            result = ObjectMapper.Map<NganHangDto>(data);
            return result;
        }
        [NonAction]
        public async Task<NganHangDto> Update(CreateOrEditNganHangDto input, DM_NganHang oldData)
        {
            NganHangDto result = new NganHangDto();
            oldData.MaNganHang = input.MaNganHang;
            oldData.TenNganHang = input.TenNganHang;
            oldData.ThuPhiThanhToan = input.ThuPhiThanhToan;
            oldData.TheoPhanTram = input.TheoPhanTram;
            oldData.ChungTuApDung = input.ChungTuApDung;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _nganHangRepository.UpdateAsync(oldData);
            result = ObjectMapper.Map<NganHangDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<NganHangDto> Delete(Guid id)
        {
            var data = await _nganHangRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.DeletionTime = DateTime.Now;
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                await _nganHangRepository.UpdateAsync(data);
                return ObjectMapper.Map<NganHangDto>(data);
            }
            return new NganHangDto();
        }
        public async Task<CreateOrEditNganHangDto> GetForEdit(Guid id)
        {
            var data = await _nganHangRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditNganHangDto>(data);
            }
            return new CreateOrEditNganHangDto();
        }
        public async Task<PagedResultDto<NganHangDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<NganHangDto> result = new PagedResultDto<NganHangDto>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var listData = await _nganHangRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = listData.Count();
            var data = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NganHangDto>>(data);
            return result;
        }
    }
}
