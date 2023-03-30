using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang
{
    [AbpAuthorize(PermissionNames.Pages_KhachHang)]
    public class KhachHangAppService : SPAAppServiceBase
    {
        private IRepository<DM_KhachHang, Guid> _repository;
        public KhachHangAppService(IRepository<DM_KhachHang, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<KhachHangDto> CreateKhachHang(CreateOrEditKhachHangDto dto)
        {
            KhachHangDto result = new KhachHangDto();
            var khachHang = ObjectMapper.Map<DM_KhachHang>(dto);
            khachHang.Id = Guid.NewGuid();
            khachHang.CreationTime = DateTime.Now;
            khachHang.CreatorUserId = AbpSession.UserId;
            khachHang.TenantId = AbpSession.TenantId ?? 1;
            khachHang.IsDeleted = false;
            await _repository.InsertAsync(khachHang);
            result = ObjectMapper.Map<KhachHangDto>(khachHang);
            return result;
        }
        public async Task<KhachHangDto> EditKhachHang(CreateOrEditKhachHangDto dto)
        {
            KhachHangDto result = new KhachHangDto();
            var khachHang = ObjectMapper.Map<DM_KhachHang>(dto);
            khachHang.LastModificationTime = DateTime.Now;
            khachHang.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(khachHang);
            result = ObjectMapper.Map<KhachHangDto>(khachHang);

            return result;
        }
        [HttpPost]
        public async Task<KhachHangDto> Delete(Guid id)
        {
            KhachHangDto result = new KhachHangDto();
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 0;
                _repository.Update(delete);
                result = ObjectMapper.Map<KhachHangDto>(delete);
            }
            return result;
        }
        public async Task<DM_KhachHang> GetKhachHangDetail(Guid Id)
        {
            var KhachHang = await _repository.GetAsync(Id);
            return KhachHang;
        }

        public async Task<PagedResultDto<DM_KhachHang>> GetAll(PagedKhachHangResultRequestDto input)
        {
            PagedResultDto<DM_KhachHang> ListResultDto = new PagedResultDto<DM_KhachHang>();
            var lstData = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstData = lstData.Where(
                    x => x.TenKhachHang.Contains(input.Keyword) || x.MaKhachHang.Contains(input.Keyword) ||
                    x.MaSoThue.Contains(input.Keyword) || x.SoDienThoai.Contains(input.Keyword) ||
                    x.DiaChi.Contains(input.Keyword) || x.Email.Contains(input.Keyword)
                   ).ToList();
            }
            if (input.SkipCount > 0)
            {
                input.SkipCount = input.SkipCount * 10;
            }

            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            ListResultDto.Items = lstData;
            return ListResultDto;
        }
    }
}
