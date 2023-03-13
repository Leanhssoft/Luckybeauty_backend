using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien
{
    public class NhanSuAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_NhanVien, Guid> _repository;
        public NhanSuAppService(IRepository<NS_NhanVien, Guid> repository)
        {
             _repository= repository;
        }
        public async Task<bool> CreateOrEdit(CreateOrEditNhanSuDto dto)
        {
            bool result = false;
            try
            {
                var find =await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (find==null)
                {
                    await Create(dto);
                }
                else
                {
                    await Edit(dto,find);
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        [NonAction]
        public async Task Create(CreateOrEditNhanSuDto dto)
        {
            NS_NhanVien nhanSu = new NS_NhanVien();
            nhanSu.Id = Guid.NewGuid();
            nhanSu.IdChucVu = dto.IdChucVu;
            nhanSu.MaNhanVien = dto.MaNhanVien;
            nhanSu.TenNhanVien = dto.TenNhanVien;
            nhanSu.CCCD = dto.CCCD;
            nhanSu.GioiTinh = dto.GioiTinh;
            nhanSu.DiaChi= dto.DiaChi;
            nhanSu.SoDienThoai = dto.SoDienThoai;
            nhanSu.NgaySinh = dto.NgaySinh;
            nhanSu.NgayCap = dto.NgayCap;
            nhanSu.NoiCap = dto.NoiCap;
            nhanSu.KieuNgaySinh= dto.KieuNgaySinh;
            nhanSu.Avatar = dto.Avatar;
            nhanSu.TenantId = AbpSession.TenantId ?? 1;
            nhanSu.CreationTime = DateTime.Now;
            nhanSu.CreatorUserId = AbpSession.UserId;
            nhanSu.NgayTao =DateTime.Now;
            await _repository.InsertAsync(nhanSu);
        }
        [NonAction]
        public async Task Edit(CreateOrEditNhanSuDto dto, NS_NhanVien nhanSu)
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
            await _repository.UpdateAsync(nhanSu);
        }
        public async Task<bool> Delete(Guid id)
        {
            var find =await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (find!=null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                _repository.Update(find);
                return true;
            }
            return false;
        }
        public async Task<ListResultDto<NS_NhanVien>> GetAll()
        {
            ListResultDto<NS_NhanVien> result = new ListResultDto<NS_NhanVien>();
            var lstNhanSu = await _repository.GetAllListAsync();
            lstNhanSu = lstNhanSu.Where(x=>x.IsDeleted==false).ToList();
            //var nhanSus = ObjectMapper.Map<List<NhanSuDto>>(lstNhanSu);
            result.Items = lstNhanSu;
            return result;
        }
    }
}
