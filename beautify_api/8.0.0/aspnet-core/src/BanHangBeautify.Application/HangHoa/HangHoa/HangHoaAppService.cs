

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa
{
    [AbpAuthorize(PermissionNames.Pages_DM_LoaiHangHoa)]
    public class HangHoaAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_HangHoa, Guid> _repository;
        public HangHoaAppService(IRepository<DM_HangHoa, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<HangHoaDto> CreateOrEdit(CreateOrEditHangHoaDto dto)
        {
            var findHangHoa = await _repository.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (findHangHoa == null)
            {
                return await Create(dto);
            }
            else
            {
                return await Edit(dto, findHangHoa);
            }
        }
        [NonAction]
        public async Task<HangHoaDto> Create(CreateOrEditHangHoaDto dto)
        {
            DM_HangHoa hangHoa = new DM_HangHoa();
            hangHoa.Id = Guid.NewGuid();
            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TrangThai = 0;
            hangHoa.TenantId = AbpSession.TenantId ?? 1;
            hangHoa.CreatorUserId = AbpSession.UserId;
            hangHoa.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<HangHoaDto>(hangHoa);
            await _repository.InsertAsync(hangHoa);
            return result;
        }
        [NonAction]
        public async Task<HangHoaDto> Edit(CreateOrEditHangHoaDto dto, DM_HangHoa hangHoa)
        {
            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TrangThai = dto.TrangThai;
            hangHoa.LastModificationTime = DateTime.Now;
            hangHoa.LastModifierUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<HangHoaDto>(hangHoa);
            await _repository.UpdateAsync(hangHoa);
            return result;
        }
        public async Task<DM_HangHoa> getDetail(Guid id)
        {
            return await _repository.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<ListResultDto<DM_HangHoa>> GetAll(HangHoaPagedResultRequestDto input)
        {
            ListResultDto<DM_HangHoa> result = new ListResultDto<DM_HangHoa>();
            var lstHangHoa = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstHangHoa = lstHangHoa.Where(x => x.TenHangHoa.Contains(input.Keyword) || x.TenHangHoa.Contains(input.Keyword)).ToList();
            }
            if (input.SkipCount > 0)
            {
                input.SkipCount *= 10;
            }
            result.Items = lstHangHoa.Skip(input.SkipCount).Take(input.SkipCount).ToList();
            return result;
        }
        [HttpPost]
        public async Task<HangHoaDto> Delete(Guid id)
        {
            HangHoaDto result = new HangHoaDto();
            var findHangHoa = await _repository.FirstOrDefaultAsync(h => h.Id == id);
            if (findHangHoa != null)
            {
                findHangHoa.IsDeleted = true;
                findHangHoa.TrangThai = 0;
                findHangHoa.DeletionTime = DateTime.Now;
                findHangHoa.DeleterUserId = AbpSession.UserId;
                _repository.Update(findHangHoa);
                result = ObjectMapper.Map<HangHoaDto>(findHangHoa);
            }
            return result;
        }
    }
}
