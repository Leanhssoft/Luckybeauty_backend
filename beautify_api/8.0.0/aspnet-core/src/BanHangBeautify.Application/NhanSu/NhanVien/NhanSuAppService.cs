using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.NhanSu.NhanVien.Responsitory;
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
        private readonly IRepository<NS_QuaTrinh_CongTac, Guid> _quaTrinhCongTac;
        private readonly INhanSuRepository _nhanSuRepository;
        public NhanSuAppService(IRepository<NS_NhanVien, Guid> repository,
            IRepository<NS_ChucVu, Guid> chucVuRepository, 
            IRepository<NS_QuaTrinh_CongTac, Guid> quaTrinhCongTac,
            INhanSuRepository nhanSuRepository
         )
        {
            _repository = repository;
            _chucVuRepository = chucVuRepository;
            _quaTrinhCongTac = quaTrinhCongTac;
            _nhanSuRepository = nhanSuRepository;
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
            //nhanSu.IdPhongBan = dto.IdPhongBan;
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
            nhanSu.LastModificationTime = DateTime.Now;
            nhanSu.LastModifierUserId = AbpSession.UserId;
            nhanSu.IsDeleted = false;
            var result = ObjectMapper.Map<NhanSuItemDto>(nhanSu);
            result.NgayVaoLam = nhanSu.CreationTime;
            result.TenChucVu = _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu??Guid.Empty)!=null ? _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty).TenChucVu:string.Empty;
            await _repository.InsertAsync(nhanSu);
            var qtct =  CreateFirstQuaTrinhCongTac(nhanSu.Id, dto.IdChiNhanh);
            await _quaTrinhCongTac.InsertAsync(qtct);
            return result;
        }
        [NonAction]
        public NS_QuaTrinh_CongTac CreateFirstQuaTrinhCongTac(Guid idNhanVien, Guid? idChiNhanh)
        {
            NS_QuaTrinh_CongTac qtct = new NS_QuaTrinh_CongTac();
            qtct.Id = Guid.NewGuid();
            qtct.IdNhanVien = idNhanVien;
            qtct.IdChiNhanh = idChiNhanh;
            qtct.TuNgay = DateTime.Now;
            qtct.TrangThai = 0;
            qtct.TenantId = AbpSession.TenantId ?? 1;
            qtct.CreationTime = DateTime.Now;
            qtct.CreatorUserId = AbpSession.UserId;
            qtct.IsDeleted = false;
            return qtct;
        }
        [NonAction]
        public async Task<NhanSuItemDto> Edit(CreateOrEditNhanSuDto dto, NS_NhanVien nhanSu)
        {
            nhanSu.IdChucVu = dto.IdChucVu;
            //nhanSu.IdPhongBan = dto.IdPhongBan;
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
            var result = ObjectMapper.Map<NhanSuItemDto>(nhanSu);
            result.NgayVaoLam = nhanSu.CreationTime;
            result.TenChucVu = _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty) != null ? _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty).TenChucVu : string.Empty;
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
            var nhanSu =await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (nhanSu != null)
            {
                var result = ObjectMapper.Map<CreateOrEditNhanSuDto>(nhanSu);
                return result;
            }
            
            return new CreateOrEditNhanSuDto();
        }
        public async Task<PagedResultDto<NhanSuItemDto>> GetAll(PagedNhanSuRequestDto input)
        {
            input.Filter = (input.Filter ?? string.Empty).Trim();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.MaxResultCount = input.MaxResultCount;
            input.TenantId = input.TenantId != null ? input.TenantId :(AbpSession.TenantId??1);
            return await _nhanSuRepository.GetAllNhanSu(input);
        }
        public async Task<PagedResultDto<NhanSuItemDto>> Search(PagedResultRequestDto input, string keyWord)
        {
            PagedResultDto<NhanSuItemDto> result = new PagedResultDto<NhanSuItemDto>();
            try
            {
                if (string.IsNullOrEmpty(keyWord))
                {
                    keyWord = "";
                }
                var lstNhanSu = await _repository.GetAll().Include(x => x.NS_ChucVu).
                                           Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).
                                           OrderByDescending(x => x.CreationTime).
                                           ToListAsync();
                lstNhanSu = lstNhanSu.Where(x => (x.TenNhanVien != null && x.TenNhanVien.Contains(keyWord)) ||
                                           (x.MaNhanVien != null && x.MaNhanVien.Contains(keyWord))).ToList();

                result.TotalCount = lstNhanSu.Count;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
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
