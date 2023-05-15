using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.LichLamViec_Ca.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec_Ca
{
    public class LichLamViecCaAppService: SPAAppServiceBase
    {
        private readonly IRepository<NS_LichLamViec_Ca, Guid> _lichLamViecCa;
        public LichLamViecCaAppService(IRepository<NS_LichLamViec_Ca, Guid> lichLamViecCa)
        {
            _lichLamViecCa = lichLamViecCa;
        }
        public async Task<LichLamViecCaDto> CreateOrEdit(CreateOrEditLichLamViecCaDto input)
        {
            var check = await _lichLamViecCa.FirstOrDefaultAsync(x=>x.Id==input.Id);
            if (check == null)
            {
                return await Create(input);
            }
            return await Update(input, check);
        }
        [NonAction]
        public async Task<LichLamViecCaDto> Create(CreateOrEditLichLamViecCaDto input)
        {
            LichLamViecCaDto result = new LichLamViecCaDto();
            NS_LichLamViec_Ca data = new NS_LichLamViec_Ca();
            data = ObjectMapper.Map<NS_LichLamViec_Ca>(input);
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _lichLamViecCa.InsertAsync(data);
            result = ObjectMapper.Map<LichLamViecCaDto>(input);
            return result;
        }
        [NonAction]
        public async Task<LichLamViecCaDto> Update(CreateOrEditLichLamViecCaDto input,NS_LichLamViec_Ca oldData)
        {
            LichLamViecCaDto result = new LichLamViecCaDto();
            oldData.GiaTri = input.GiaTri;
            oldData.IdCaLamViec = input.IdCaLamViec;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _lichLamViecCa.UpdateAsync(oldData);
            result = ObjectMapper.Map<LichLamViecCaDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<LichLamViecCaDto> Delete(Guid id)
        {
            var check =await _lichLamViecCa.FirstOrDefaultAsync(x => x.Id == id);
            check.IsDeleted = false;
            check.DeletionTime = DateTime.Now;
            _lichLamViecCa.Update(check);
            return ObjectMapper.Map<LichLamViecCaDto>(check);
        }
        public async Task<NS_LichLamViec_Ca> GetDetail(Guid id)
        {
            return await _lichLamViecCa.GetAllIncluding().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PagedResultDto<NS_LichLamViec_Ca>> GetAll(PagedRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Keyword))
            {
                input.Keyword = "";
            }
            input.SkipCount = input.SkipCount == 0 || input.SkipCount == 1 ? 0 : (input.SkipCount - 1) * input.MaxResultCount;
            PagedResultDto<NS_LichLamViec_Ca> result = new PagedResultDto<NS_LichLamViec_Ca>();
            var listData = await _lichLamViecCa.GetAllIncluding().Where(x=>x.TenantId==(AbpSession.TenantId??0)&& x.IsDeleted==false).OrderByDescending(x=>x.CreationTime).ToListAsync();
            result.TotalCount= listData.Count;
            listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = listData;
            return result;
        }
    }
}
