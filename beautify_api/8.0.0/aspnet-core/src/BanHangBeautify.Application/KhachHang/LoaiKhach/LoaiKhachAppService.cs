using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.LoaiKhach.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.LoaiKhach
{
    [AbpAuthorize(PermissionNames.Pages_LoaiKhach)]
    public class LoaiKhachAppService : SPAAppServiceBase, ILoaiKhachAppService
    {
        private readonly IRepository<DM_LoaiKhach, int> _repository;
        public LoaiKhachAppService(IRepository<DM_LoaiKhach, int> repository)
        {
            _repository = repository;
        }
        [AbpAuthorize(PermissionNames.Pages_LoaiKhach_Create)]
        public async Task<LoaiKhachDto> CreateLoaiKhach(CreateOrEditLoaiKhachDto dto)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var loaiKhach = ObjectMapper.Map<DM_LoaiKhach>(dto);
            loaiKhach.Id = _repository.Count() + 1;
            loaiKhach.CreationTime = DateTime.Now;
            loaiKhach.CreatorUserId = AbpSession.UserId;
            loaiKhach.TenantId = AbpSession.TenantId ?? 1;
            loaiKhach.IsDeleted = false;
            await _repository.InsertAsync(loaiKhach);
            result = ObjectMapper.Map<LoaiKhachDto>(loaiKhach);
            return result;
        }
        [AbpAuthorize(PermissionNames.Pages_LoaiKhach_Edit)]
        public async Task<LoaiKhachDto> EditLoaiKhach(CreateOrEditLoaiKhachDto dto)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var loaiKhach = ObjectMapper.Map<DM_LoaiKhach>(dto);
            loaiKhach.LastModificationTime = DateTime.Now;
            loaiKhach.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(loaiKhach);
            result = ObjectMapper.Map<LoaiKhachDto>(loaiKhach);

            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_LoaiKhach_Delete)]
        public async Task<LoaiKhachDto> Delete(int id)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 1;
                _repository.Update(delete);
                result = ObjectMapper.Map<LoaiKhachDto>(delete);
            }
            return result;
        }
        public async Task<DM_LoaiKhach> GetLoaiKhachDetail(int Id)
        {
            var loaiKhach = await _repository.GetAsync(Id);
            return loaiKhach;
        }

        public async Task<PagedResultDto<DM_LoaiKhach>> GetAll(PagedLoaiKhachResultRequestDto input)
        {
            PagedResultDto<DM_LoaiKhach> ListResultDto = new PagedResultDto<DM_LoaiKhach>();
            var lstData = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstData = lstData.Where(x => x.TenLoaiKhachHang.Contains(input.Keyword) || x.MaLoaiKhachHang.Contains(input.Keyword)).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            ListResultDto.Items = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return ListResultDto;
        }


    }
}
