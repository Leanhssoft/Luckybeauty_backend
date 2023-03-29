using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu)]
    public class NhanSuAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_NhanVien, Guid> _repository;
        private readonly IRepository<NS_ChucVu, Guid> _chucVuRepository;
        public NhanSuAppService(IRepository<NS_NhanVien, Guid> repository, IRepository<NS_ChucVu, Guid> chucVuRepository)
        {
            _repository = repository;
            _chucVuRepository= chucVuRepository;
        }
        public async Task<NhanSuDto> CreateOrEdit(CreateOrEditNhanSuDto dto)
        {
            try
            {
                var find = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (find == null)
                {
                    return await Create(dto);
                }
                else
                {
                    return await Edit(dto, find);
                }

            }
            catch (Exception)
            {
                return new NhanSuDto();
            }
        }
        [NonAction]
        public async Task<NhanSuDto> Create(CreateOrEditNhanSuDto dto)
        {
            NS_NhanVien nhanSu = new NS_NhanVien();
            nhanSu.Id = Guid.NewGuid();
            nhanSu.IdChucVu = dto.IdChucVu;
            nhanSu.MaNhanVien = dto.MaNhanVien;
            nhanSu.TenNhanVien = dto.TenNhanVien;
            nhanSu.CCCD = dto.CCCD;
            nhanSu.GioiTinh = dto.GioiTinh;
            nhanSu.DiaChi = dto.DiaChi;
            nhanSu.SoDienThoai = dto.SoDienThoai;
            nhanSu.NgaySinh = dto.NgaySinh;
            nhanSu.NgayCap = dto.NgayCap;
            nhanSu.NoiCap = dto.NoiCap;
            nhanSu.KieuNgaySinh = dto.KieuNgaySinh;
            nhanSu.Avatar = dto.Avatar;
            nhanSu.TenantId = AbpSession.TenantId ?? 1;
            nhanSu.CreationTime = DateTime.Now;
            nhanSu.CreatorUserId = AbpSession.UserId;
            nhanSu.NgayTao = DateTime.Now;
            nhanSu.IsDeleted = false;
            var result = ObjectMapper.Map<NhanSuDto>(nhanSu);
            await _repository.InsertAsync(nhanSu);
            return result;
        }
        [NonAction]
        public async Task<NhanSuDto> Edit(CreateOrEditNhanSuDto dto, NS_NhanVien nhanSu)
        {
            nhanSu.IdChucVu = dto.IdChucVu;
            nhanSu.MaNhanVien = dto.MaNhanVien;
            nhanSu.TenNhanVien = dto.TenNhanVien;
            nhanSu.CCCD = dto.CCCD;
            nhanSu.GioiTinh = dto.GioiTinh;
            nhanSu.DiaChi = dto.DiaChi;
            nhanSu.SoDienThoai = dto.SoDienThoai;
            nhanSu.NgaySinh = dto.NgaySinh;
            nhanSu.NgayCap = dto.NgayCap;
            nhanSu.NoiCap = dto.NoiCap;
            nhanSu.KieuNgaySinh = dto.KieuNgaySinh;
            nhanSu.Avatar = dto.Avatar;
            nhanSu.LastModificationTime = DateTime.Now;
            nhanSu.LastModifierUserId = AbpSession.UserId;
            nhanSu.NgaySua = DateTime.Now;
            var result = ObjectMapper.Map<NhanSuDto>(nhanSu);
            await _repository.UpdateAsync(nhanSu);
            return result;
        }
        [HttpPost]
        public async Task<NhanSuDto> Delete(Guid id)
        {
            var find = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (find != null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                _repository.Update(find);
                return ObjectMapper.Map<NhanSuDto>(find);
            }
            return new NhanSuDto();
        }
        public async Task<NS_NhanVien> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ListResultDto<NhanSuDto>> GetAll(PagedResultRequestDto input, string keyWord)
        {
            ListResultDto<NhanSuDto> result = new ListResultDto<NhanSuDto>();
            var lstNhanSu =await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(keyWord))
            {
                lstNhanSu = lstNhanSu.Where(x => x.TenNhanVien.Contains(keyWord) || x.MaNhanVien.Contains(keyWord) || x.NoiCap.Contains(keyWord)).ToList();
            }
            input.MaxResultCount = 10;
            input.SkipCount = input.SkipCount > 0 ? (input.SkipCount * 10) : 0;
            lstNhanSu = lstNhanSu.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var items = ObjectMapper.Map<List<NhanSuDto>>(lstNhanSu);
            result.Items = items;  
            return result;
        }
        public async Task<List<SuggestChucVu>> SuggestChucVus(){
            List<SuggestChucVu> result = new List<SuggestChucVu>();
            var lstChucVu = _chucVuRepository.GetAll();
            foreach (var item in lstChucVu)
            {
                SuggestChucVu rdo = new SuggestChucVu();
                rdo.TenChucVu = item.TenChucVu;
                rdo.IdChucVu = item.Id;
                result.Add(rdo);
            }
            return result;
        }
    }
}
