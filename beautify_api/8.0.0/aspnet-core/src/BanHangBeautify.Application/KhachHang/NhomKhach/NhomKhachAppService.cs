using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.NhomKhach.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.NhomKhach
{
    [AbpAuthorize(PermissionNames.Pages_NhomKhach)]
    public class NhomKhachAppService : SPAAppServiceBase
    {
        private IRepository<DM_NhomKhachHang, Guid> _repository;
        public NhomKhachAppService(IRepository<DM_NhomKhachHang, Guid> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhomKhach_Create, PermissionNames.Pages_NhomKhach_Edit)]
        public async Task<NhomKhachDto> CreateOrEditNhomKhach(CreateOrEditNhomKhachDto dto)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (checkExist != null)
            {
                return await EditNhomKhach(dto, checkExist);
            }
            return await CreateNhomKhach(dto);
        }

        [NonAction]
        public async Task<NhomKhachDto> CreateNhomKhach(CreateOrEditNhomKhachDto dto)
        {
            NhomKhachDto result = new NhomKhachDto();
            var nhomKhach = ObjectMapper.Map<DM_NhomKhachHang>(dto);
            var count = _repository.Count();
            nhomKhach.Id = Guid.NewGuid();
            nhomKhach.MaNhomKhach = "NKH0" + (count + 1).ToString();
            nhomKhach.CreationTime = DateTime.Now;
            nhomKhach.CreatorUserId = AbpSession.UserId;
            nhomKhach.TenantId = AbpSession.TenantId ?? 1;
            nhomKhach.IsDeleted = false;
            await _repository.InsertAsync(nhomKhach);
            result = ObjectMapper.Map<NhomKhachDto>(nhomKhach);
            return result;
        }
        [NonAction]
        public async Task<NhomKhachDto> EditNhomKhach(CreateOrEditNhomKhachDto dto, DM_NhomKhachHang oldData)
        {
            NhomKhachDto result = new NhomKhachDto();
            oldData.TenNhomKhach = dto.TenNhomKhach;
            oldData.MoTa = dto.MoTa;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<NhomKhachDto>(oldData);

            return result;
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Pages_NhomKhach_Delete)]
        public async Task<NhomKhachDto> Delete(Guid id)
        {
            NhomKhachDto result = new NhomKhachDto();
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 1;
                _repository.Update(delete);
                result = ObjectMapper.Map<NhomKhachDto>(delete);
            }
            return result;
        }
        public async Task<DM_NhomKhachHang> GetNguonKhachDetail(Guid Id)
        {
            var nhomKhach = await _repository.GetAsync(Id);
            return nhomKhach;
        }
        [HttpPost]
        public async Task<CreateOrEditNhomKhachDto> GetForEdit(Guid id)
        {
            var nhomKhach = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            return ObjectMapper.Map<CreateOrEditNhomKhachDto>(nhomKhach);
        }
        public async Task<PagedResultDto<DM_NhomKhachHang>> GetAll(PagedNhomKhachResultRequestDto input)
        {
            PagedResultDto<DM_NhomKhachHang> ListResultDto = new PagedResultDto<DM_NhomKhachHang>();
            var lstData = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstData = lstData.Where(x => x.MaNhomKhach.Contains(input.Keyword) || x.TenNhomKhach.Contains(input.Keyword)).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;

            ListResultDto.Items = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return ListResultDto;
        }

    }
}
