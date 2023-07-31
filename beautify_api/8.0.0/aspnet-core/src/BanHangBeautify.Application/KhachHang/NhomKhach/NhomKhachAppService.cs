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
        [AbpAuthorize(PermissionNames.Pages_NhomKhach_Create)]
        public async Task<NhomKhachDto> CreateNhomKhach(CreateOrEditNhomKhachDto dto)
        {
            NhomKhachDto result = new NhomKhachDto();
            var nhomKhach = ObjectMapper.Map<DM_NhomKhachHang>(dto);
            nhomKhach.Id = Guid.NewGuid();
            nhomKhach.CreationTime = DateTime.Now;
            nhomKhach.CreatorUserId = AbpSession.UserId;
            nhomKhach.TenantId = AbpSession.TenantId ?? 1;
            nhomKhach.IsDeleted = false;
            await _repository.InsertAsync(nhomKhach);
            result = ObjectMapper.Map<NhomKhachDto>(nhomKhach);
            return result;
        }
        public async Task<NhomKhachDto> EditNhomKhach(CreateOrEditNhomKhachDto dto)
        {
            NhomKhachDto result = new NhomKhachDto();
            var nhomKhach = ObjectMapper.Map<DM_NhomKhachHang>(dto);
            nhomKhach.LastModificationTime = DateTime.Now;
            nhomKhach.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(nhomKhach);
            result = ObjectMapper.Map<NhomKhachDto>(nhomKhach);

            return result;
        }
        [HttpPost]
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
