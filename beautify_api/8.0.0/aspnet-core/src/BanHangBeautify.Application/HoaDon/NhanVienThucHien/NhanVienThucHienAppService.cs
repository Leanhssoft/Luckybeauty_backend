using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.NhanVienThucHien.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien
{
    [AbpAuthorize(PermissionNames.Pages_NhanVienThucHien)]
    public class NhanVienThucHienAppService:SPAAppServiceBase
    {
        private readonly IRepository<BH_NhanVienThucHien, Guid> _repository;
        public NhanVienThucHienAppService(IRepository<BH_NhanVienThucHien, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<NhanVienThucHienDto> CreateOrEdit(CreateOrEditNhanVienThucHienDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<NhanVienThucHienDto> Create(CreateOrEditNhanVienThucHienDto input)
        {
            NhanVienThucHienDto result = new NhanVienThucHienDto();
            BH_NhanVienThucHien data = new BH_NhanVienThucHien();
            data = ObjectMapper.Map<BH_NhanVienThucHien>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.IdHoaDonChiTiet = input.IdChiTietHoaDon;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<NhanVienThucHienDto>(input);
            return result;
        }
        [NonAction]
        public async Task<NhanVienThucHienDto> Update(CreateOrEditNhanVienThucHienDto input, BH_NhanVienThucHien oldData)
        {
            NhanVienThucHienDto result = new NhanVienThucHienDto();
            oldData.IdNhanVien = input.IdNhanVien;
            oldData.IdHoaDon = input.IdHoaDon;
            oldData.IdQuyHoaDon = input.IdQuyHoaDon;
            oldData.IdHoaDonChiTiet = input.IdChiTietHoaDon;
            oldData.PTChietKhau = input.PTChietKhau;
            oldData.TienChietKhau = input.TienChietKhau;
            oldData.HeSo = input.HeSo;
            oldData.TinhHoaHongTruocCK = input.TinhHoaHongTruocCK;
            oldData.LoaiChietKhau = input.LoaiChietKhau;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<NhanVienThucHienDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<NhanVienThucHienDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<NhanVienThucHienDto>(data);
            }
            return new NhanVienThucHienDto();
        }
        public async Task<CreateOrEditNhanVienThucHienDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditNhanVienThucHienDto>(data);
            }
            return new CreateOrEditNhanVienThucHienDto();
        }
        public async Task<PagedResultDto<NhanVienThucHienDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<NhanVienThucHienDto> result = new PagedResultDto<NhanVienThucHienDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NhanVienThucHienDto>>(lstData);
            return result;
        }
    }
}
