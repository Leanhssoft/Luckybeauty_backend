using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using AutoMapper.Internal.Mappers;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.DonViQuiDoi
{
    public class DonViQuiDoiAppService:SPAAppServiceBase
    {
        private readonly IRepository<DM_DonViQuiDoi, Guid> _repository;
        public DonViQuiDoiAppService(IRepository<DM_DonViQuiDoi,Guid> repository)
        {
            _repository= repository;
        }
        public async Task CreateOrEdit(CreateOrEditDonViQuiDoiDto dto)
        {
            var findHangHoa = await _repository.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (findHangHoa == null)
            {
                await Create(dto);
            }
            else
            {
                await Edit(dto, findHangHoa);
            }
        }
        [NonAction]
        public async Task Create(CreateOrEditDonViQuiDoiDto dto)
        {
            DM_DonViQuiDoi donViQuiDoi = new DM_DonViQuiDoi();
            donViQuiDoi.Id = Guid.NewGuid();
            donViQuiDoi.IdHangHoa = dto.IdHangHoa;
            donViQuiDoi.MaHangHoa = dto.MaHangHoa;
            donViQuiDoi.GiaBan = dto.GiaBan;
            donViQuiDoi.TenDonVi = dto.TenDonVi;
            donViQuiDoi.LaDonViTinhChuan = dto.LaDonViTinhChuan;
            donViQuiDoi.TyLeChuyenDoi = dto.TyLeChuyenDoi;
            donViQuiDoi.TenantId = AbpSession.TenantId ?? 1;
            donViQuiDoi.CreatorUserId = AbpSession.UserId;
            donViQuiDoi.CreationTime = DateTime.Now;
            await _repository.InsertAsync(donViQuiDoi);
        }
        [NonAction]
        public async Task Edit(CreateOrEditDonViQuiDoiDto dto, DM_DonViQuiDoi donViQuiDoi)
        {
            donViQuiDoi.IdHangHoa = dto.IdHangHoa;
            donViQuiDoi.MaHangHoa = dto.MaHangHoa;
            donViQuiDoi.GiaBan = dto.GiaBan;
            donViQuiDoi.TenDonVi = dto.TenDonVi;
            donViQuiDoi.LaDonViTinhChuan = dto.LaDonViTinhChuan;
            donViQuiDoi.TyLeChuyenDoi = dto.TyLeChuyenDoi;
            donViQuiDoi.LastModificationTime = DateTime.Now;
            donViQuiDoi.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(donViQuiDoi);
        }
        public async Task<ListResultDto<DM_DonViQuiDoi>> GetAll()
        {
            ListResultDto<DM_DonViQuiDoi> result = new ListResultDto<DM_DonViQuiDoi>();
            var getDonViQuiDoi = await _repository.GetAllListAsync();
            getDonViQuiDoi = getDonViQuiDoi.Where(x=> x.IsDeleted == true).ToList();
            //var donViQuiDois = ObjectMapper.Map<List<DonViQuiDoiDto>>(getDonViQuiDoi);
            result.Items = getDonViQuiDoi;
            return result;
        }
        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var donViQuiDoi = await _repository.FirstOrDefaultAsync(h => h.Id == id);
            if (donViQuiDoi != null)
            {
                donViQuiDoi.IsDeleted = true;
                donViQuiDoi.DeletionTime = DateTime.Now;
                donViQuiDoi.DeleterUserId = AbpSession.UserId;
                _repository.Update(donViQuiDoi);
                result = true;
            }
            return result;
        }
    }
}
