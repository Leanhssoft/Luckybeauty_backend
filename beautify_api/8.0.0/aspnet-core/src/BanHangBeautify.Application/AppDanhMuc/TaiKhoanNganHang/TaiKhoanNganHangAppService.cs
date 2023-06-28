using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang
{
    [AbpAuthorize(PermissionNames.Pages_TaiKhoanNganHang)]
    public class TaiKhoanNganHangAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_TaiKhoanNganHang,Guid> _repository;
        public TaiKhoanNganHangAppService(IRepository<DM_TaiKhoanNganHang,Guid> repository)
        {
            _repository = repository;
        }
        public async Task<TaiKhoanNganHangDto> CreateOrEdit(CreateOrEditTaiKhoanNganHangDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x=>x.Id== input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<TaiKhoanNganHangDto> Create(CreateOrEditTaiKhoanNganHangDto input) {
            DM_TaiKhoanNganHang data = new DM_TaiKhoanNganHang();
            data = ObjectMapper.Map<DM_TaiKhoanNganHang>(data);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TenantId = AbpSession.TenantId ?? 1;
            await _repository.InsertAsync(data);
            return ObjectMapper.Map<TaiKhoanNganHangDto>(data);
        }
        [NonAction]
        public async Task<TaiKhoanNganHangDto> Update(CreateOrEditTaiKhoanNganHangDto input,DM_TaiKhoanNganHang oldData) {
            TaiKhoanNganHangDto result = new TaiKhoanNganHangDto();
            oldData.IdNganHang = input.IdNganHang;
            oldData.GhiChu = input.GhiChu;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.TenChuThe = input.TenChuThe;
            oldData.SoTaiKhoan = input.SoTaiKhoan;
            oldData.TrangThai = input.TrangThai;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        public async Task<TaiKhoanNganHangDto> Delete(Guid id) { 
            var data = await _repository.FirstOrDefaultAsync(x=> x.Id == id);
            if (data != null)
            {
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession?.UserId;
                data.IsDeleted = true;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<TaiKhoanNganHangDto>(data);
            }
            return new TaiKhoanNganHangDto();
        }
        public async Task<CreateOrEditTaiKhoanNganHangDto> GetForEdit(Guid id) { 
            var data = await _repository.FirstOrDefaultAsync(x=>x.Id == id);
            if (data!=null)
            {
                return ObjectMapper.Map<CreateOrEditTaiKhoanNganHangDto>(data);
            }
            return new CreateOrEditTaiKhoanNganHangDto();
        }
        public async Task<PagedResultDto<TaiKhoanNganHangDto>> GetAll(PagedRequestDto input) {
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            PagedResultDto<TaiKhoanNganHangDto> result = new PagedResultDto<TaiKhoanNganHangDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<TaiKhoanNganHangDto>>(data);
            return result;
        }
    }
}
