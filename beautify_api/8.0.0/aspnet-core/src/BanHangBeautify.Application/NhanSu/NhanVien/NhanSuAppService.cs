using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Suggests.Dto;
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
            _chucVuRepository = chucVuRepository;
        }
        public async Task<NhanSuItemDto> CreateOrEdit(CreateOrEditNhanSuDto dto)
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
                return new NhanSuItemDto();
            }
        }
        [NonAction]
        public async Task<NhanSuItemDto> Create(CreateOrEditNhanSuDto dto)
        {
            NS_NhanVien nhanSu = new NS_NhanVien();
            nhanSu.Id = Guid.NewGuid();
            nhanSu.IdChucVu = dto.IdChucVu;
            nhanSu.MaNhanVien = dto.MaNhanVien;
            nhanSu.Ho = dto.Ho;
            nhanSu.TenLot = dto.TenLot;
            nhanSu.TenNhanVien = dto.Ho + " " + dto.TenLot;
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
            var result = ObjectMapper.Map<NhanSuItemDto>(nhanSu);
            result.NgayVaoLam = nhanSu.CreationTime;

            result.TenChucVu = _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu).TenChucVu;
            await _repository.InsertAsync(nhanSu);
            return result;
        }
        [NonAction]
        public async Task<NhanSuItemDto> Edit(CreateOrEditNhanSuDto dto, NS_NhanVien nhanSu)
        {
            nhanSu.IdChucVu = dto.IdChucVu;
            nhanSu.MaNhanVien = dto.MaNhanVien;
            nhanSu.Ho = dto.Ho;
            nhanSu.TenLot = dto.TenLot;
            nhanSu.TenNhanVien = dto.Ho + " " + dto.TenLot;
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
            var result = ObjectMapper.Map<NhanSuItemDto>(nhanSu);
            result.NgayVaoLam = nhanSu.CreationTime;
            result.TenChucVu = _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu).TenChucVu;
            await _repository.UpdateAsync(nhanSu);
            return result;
        }
        [HttpPost]
        public async Task<NhanSuItemDto> Delete(Guid id)
        {
            var find = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (find != null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                _repository.Update(find);
                return ObjectMapper.Map<NhanSuItemDto>(find);
            }
            return new NhanSuItemDto();
        }
        public async Task<NS_NhanVien> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        [HttpPost]
        public async Task<CreateOrEditNhanSuDto> GetNhanSu(Guid id)
        {
            var nhanSu= await _repository.GetAsync(id);
            var result = ObjectMapper.Map<CreateOrEditNhanSuDto>(nhanSu);
            return result;
        }
        public async Task<PagedResultDto<NhanSuDto>> GetAll(PagedResultRequestDto input, string keyWord)
        {
            PagedResultDto<NhanSuDto> result = new PagedResultDto<NhanSuDto>();
            var lstNhanSu = await _repository.GetAll().Include(x => x.NS_ChucVu).Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstNhanSu.Count;
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
        public async Task<PagedResultDto<NhanSuItemDto>> Search(PagedResultRequestDto input, string keyWord)
        {
            PagedResultDto<NhanSuItemDto> result = new PagedResultDto<NhanSuItemDto>();
            try
            {
                if (!string.IsNullOrEmpty(keyWord))
                {
                    keyWord = "";
                }
                var lstNhanSu = await _repository.GetAll().Include(x => x.NS_ChucVu).Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false ||
                                           (x.TenNhanVien.Contains(keyWord) && x.TenNhanVien != null) ||
                                           (x.MaNhanVien != null && x.MaNhanVien.Contains(keyWord))).
                                           OrderByDescending(x => x.CreationTime).
                                           ToListAsync();
                result.TotalCount = lstNhanSu.Count;
                input.MaxResultCount = 10;
                input.SkipCount = input.SkipCount > 0 ? (input.SkipCount * 10) : 0;
                var items = lstNhanSu.Skip(input.SkipCount).Take(input.MaxResultCount).Select(x => new NhanSuItemDto()
                {
                    Id = x.Id,
                    MaNhanVien = x.MaNhanVien,
                    TenNhanVien = x.TenNhanVien,
                    GioiTinh = x.GioiTinh,
                    Avatar = x.Avatar,
                    CCCD = x.CCCD,
                    DiaChi = x.DiaChi,
                    TenChucVu = x.NS_ChucVu == null ? "" : x.NS_ChucVu.TenChucVu,
                    KieuNgaySinh = x.KieuNgaySinh,
                    NgayCap = x.NgayCap,
                    NgaySinh = x.NgaySinh,
                    NgayVaoLam = x.CreationTime,
                    NoiCap = x.NoiCap,
                    SoDienThoai = x.SoDienThoai
                }).ToList();
                result.Items = items;
            }
            catch (Exception ex)
            {
                result.TotalCount= 0;
                result.Items = null;
            }
            
           
            return result;
        }
    }
}
