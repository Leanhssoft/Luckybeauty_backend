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
        public async Task<LoaiKhachDto> CreateLoaiKhach(CreateOrEditLoaiKhachDto dto)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var loaiKhach = ObjectMapper.Map<DM_LoaiKhach>(dto);
            loaiKhach.CreationTime = DateTime.Now;
            loaiKhach.CreatorUserId = AbpSession.UserId;
            loaiKhach.TenantId = AbpSession.TenantId ?? 1;
            loaiKhach.IsDeleted = false;
            await _repository.InsertAsync(loaiKhach);
            result = ObjectMapper.Map<LoaiKhachDto>(loaiKhach);
            return result;
        }
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

        public async Task<ListResultDto<DM_LoaiKhach>> GetAll(PagedLoaiKhachResultRequestDto input)
        {
            ListResultDto<DM_LoaiKhach> ListResultDto = new ListResultDto<DM_LoaiKhach>();
            var lstData = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstData = lstData.Where(x => x.TenLoaiKhachHang.Contains(input.Keyword) || x.MaLoaiKhachHang.Contains(input.Keyword)).ToList();
            }
            input.MaxResultCount = 10;
            if (input.SkipCount > 0)
            {
                input.SkipCount = input.SkipCount * 10;
            }
            ListResultDto.Items = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return ListResultDto;
        }
    }
}
