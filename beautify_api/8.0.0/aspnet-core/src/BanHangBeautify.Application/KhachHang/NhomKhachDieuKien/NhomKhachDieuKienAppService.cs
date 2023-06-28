using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.NhomKhachDieuKien.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.NhomKhachDieuKien
{
    [AbpAuthorize(PermissionNames.Pages_NhomKhach_DieuKien)]
    public class NhomKhachDieuKienAppService:SPAAppServiceBase
    {
        private readonly IRepository<DM_NhomKhach_DieuKien, Guid> _repository;
        public NhomKhachDieuKienAppService(IRepository<DM_NhomKhach_DieuKien, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<NhomKhachDieuKienDto> CreateOrEdit(CreateOrEditNhomKhachDieuKienDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<NhomKhachDieuKienDto> Create(CreateOrEditNhomKhachDieuKienDto input)
        {
            NhomKhachDieuKienDto result = new NhomKhachDieuKienDto();
            DM_NhomKhach_DieuKien data = new DM_NhomKhach_DieuKien();
            data = ObjectMapper.Map<DM_NhomKhach_DieuKien>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<NhomKhachDieuKienDto> Update(CreateOrEditNhomKhachDieuKienDto input, DM_NhomKhach_DieuKien oldData)
        {
            NhomKhachDieuKienDto result = new NhomKhachDieuKienDto();
            oldData.STT = input.STT;
            oldData.IdNhomKhach = input.IdNhomKhach;
            oldData.LoaiDieuKien = input.LoaiDieuKien;
            oldData.LoaiSoSanh = input.LoaiSoSanh;
            oldData.GiaTriKhuVuc= input.GiaTriKhuVuc;
            oldData.GiaTriThoiGian = input.GiaTriThoiGian;
            oldData.GiaTriBool = input.GiaTriBool;
            oldData.GiaTriSo = input.GiaTriSo;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        public async Task<NhomKhachDieuKienDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<NhomKhachDieuKienDto>(data);
            }
            return new NhomKhachDieuKienDto();
        }
        public async Task<CreateOrEditNhomKhachDieuKienDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditNhomKhachDieuKienDto>(data);
            }
            return new CreateOrEditNhomKhachDieuKienDto();
        }
        public async Task<PagedResultDto<NhomKhachDieuKienDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<NhomKhachDieuKienDto> result = new PagedResultDto<NhomKhachDieuKienDto>();
            var listData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToListAsync();
            result.TotalCount = listData.Count;
            listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NhomKhachDieuKienDto>>(listData);
            return result;
        }
    }
}
