using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhuyenMai.KhuyenMai.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMai
{
    [AbpAuthorize(PermissionNames.Pages_KhuyenMai)]
    public class KhuyenMaiAppService:SPAAppServiceBase
    {
        private readonly IRepository<DM_KhuyenMai,Guid> _khuyenMaiRepository;
        public KhuyenMaiAppService(IRepository<DM_KhuyenMai, Guid> khuyenMaiRepository)
        {
            _khuyenMaiRepository = khuyenMaiRepository;
        }
        public async Task<KhuyenMaiDto> CreateOrEdit(CreateOrEditKhuyenMaiDto input)
        {
            var checkExist = await _khuyenMaiRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist!=null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<KhuyenMaiDto> Create(CreateOrEditKhuyenMaiDto input)
        {
            KhuyenMaiDto result =new KhuyenMaiDto();
            DM_KhuyenMai data = new DM_KhuyenMai();
            data = ObjectMapper.Map<DM_KhuyenMai>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _khuyenMaiRepository.InsertAsync(data);
            result = ObjectMapper.Map<KhuyenMaiDto>(input);
            return result;
        }
        [NonAction]
        public async Task<KhuyenMaiDto> Update(CreateOrEditKhuyenMaiDto input,DM_KhuyenMai oldData)
        {
            KhuyenMaiDto result = new KhuyenMaiDto();
            oldData.GhiChu = input.GhiChu;
            oldData.TatCaChiNhanh = input.TatCaChiNhanh;
            oldData.TatCaNhanVien = input.TatCaNhanVien;
            oldData.TatCaKhachHang = input.TatCaKhachHang;
            oldData.ThuApDung = input.ThuApDung;
            oldData.NgayApDung = input.NgayApDung;
            oldData.GioApDung = input.GioApDung;
            oldData.MaKhuyenMai = input.MaKhuyenMai;
            oldData.TenKhuyenMai = input.TenKhuyenMai;
            oldData.ThoiGianApDung = input.ThoiGianApDung;
            oldData.ThoiGianKetThuc = input.ThoiGianKetThuc;
            oldData.HinhThucKM = input.HinhThucKM;
            oldData.LoaiKhuyenMai = input.LoaiKhuyenMai;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _khuyenMaiRepository.UpdateAsync(oldData);
            result = ObjectMapper.Map<KhuyenMaiDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<KhuyenMaiDto> Delete(Guid id)
        {
            var data = await _khuyenMaiRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data!=null)
            {
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                data.IsDeleted = true;
                _khuyenMaiRepository.Update(data);
                return ObjectMapper.Map<KhuyenMaiDto>(data);
            }
            return new KhuyenMaiDto();
        }
        public async Task<CreateOrEditKhuyenMaiDto> GetForEdit(Guid id)
        {
            var data = await _khuyenMaiRepository.FirstOrDefaultAsync(x=>x.Id==id);
            if (data!=null)
            {
                return ObjectMapper.Map<CreateOrEditKhuyenMaiDto>(data);
            }
            return new CreateOrEditKhuyenMaiDto();
        }
        public async Task<PagedResultDto<KhuyenMaiDto>> GetALl(PagedRequestDto input)
        {
            PagedResultDto<KhuyenMaiDto> result = new PagedResultDto<KhuyenMaiDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ?"" : input.Keyword;
            var lstData = await _khuyenMaiRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId??1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount= lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<KhuyenMaiDto>>(data);
            return result;
        }
    }
}
