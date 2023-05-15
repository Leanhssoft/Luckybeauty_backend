using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.MauIn.Dto;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.MauIn
{
    public class MauInAppService: SPAAppServiceBase
    {
        private readonly IRepository<DM_MauIn,Guid> _dmMauInRepository;
        public MauInAppService(IRepository<DM_MauIn, Guid> dmMauInRepository)
        {
            _dmMauInRepository = dmMauInRepository;
        }
        public async Task<MauInDto> CreateOrEdit(CreateOrEditMauInDto input)
        {
            var checkExist = await _dmMauInRepository.FirstOrDefaultAsync(x=>x.Id== input.Id);
            if (checkExist==null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<MauInDto> Create(CreateOrEditMauInDto input)
        {
            MauInDto result = new MauInDto();
            DM_MauIn data = new DM_MauIn();
            data = ObjectMapper.Map<DM_MauIn>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            await _dmMauInRepository.InsertAsync(data);
            result = ObjectMapper.Map<MauInDto>(data);
            return result;
        }
        [NonAction]
        public async Task<MauInDto> Update(CreateOrEditMauInDto input,DM_MauIn oldData)
        {
            oldData.TenMauIn = input.TenMauIn;
            oldData.NoiDungMauIn = input.TenMauIn;
            oldData.LaMacDinh = input.LaMacDinh;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.LoaiChungTu = input.LoaiChungTu;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _dmMauInRepository.UpdateAsync(oldData);
            return ObjectMapper.Map<MauInDto>(oldData);
        }
        [HttpPost]
        public async Task<MauInDto> Delete(Guid id)
        {
            var data = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data!=null)
            {
                data.IsDeleted = true;
                data.DeleterUserId= AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                _dmMauInRepository.Update(data);
                return ObjectMapper.Map<MauInDto>(data);
            }
            return new MauInDto();
        }
        public async Task<CreateOrEditMauInDto> GetForEdit(Guid id)
        {
            var data = await _dmMauInRepository.FirstOrDefaultAsync(x=>x.Id==id);
            if (data!=null)
            {
                return ObjectMapper.Map<CreateOrEditMauInDto>(data);
            }
            return new CreateOrEditMauInDto();
        }
        public async Task<PagedResultDto<MauInDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<MauInDto> result = new PagedResultDto<MauInDto>();
            input.Keyword= string.IsNullOrEmpty(input.Keyword)?"":input.Keyword;
            input.SkipCount = input.SkipCount>0 ? input.SkipCount * input.MaxResultCount : 0;
            var data = await _dmMauInRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = data.Count;
            var lstMauIn = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<MauInDto>>(lstMauIn);
            return result;
        }
    }
}
