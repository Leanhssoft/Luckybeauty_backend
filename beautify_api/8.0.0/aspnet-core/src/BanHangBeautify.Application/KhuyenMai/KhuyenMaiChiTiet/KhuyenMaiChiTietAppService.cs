using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhuyenMai.KhuyenMai.Dto;
using BanHangBeautify.KhuyenMai.KhuyenMaiChiTiet.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiChiTiet
{
    [AbpAuthorize(PermissionNames.Pages_KhuyenMai)]
    public class KhuyenMaiChiTietAppService: SPAAppServiceBase
    {
        private readonly IRepository<DM_KhuyenMai_ChiTiet, Guid> _repository;
        public KhuyenMaiChiTietAppService(IRepository<DM_KhuyenMai_ChiTiet, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<KhuyenMaiChiTietDto> CreateOrEdit(CreateOrEditKhuyenMaiChiTietDto input) {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist!=null)
            {
                return await Update(input,checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<KhuyenMaiChiTietDto> Create(CreateOrEditKhuyenMaiChiTietDto input)
        {
            KhuyenMaiChiTietDto result = new KhuyenMaiChiTietDto();
            DM_KhuyenMai_ChiTiet data = new DM_KhuyenMai_ChiTiet();
            data = ObjectMapper.Map<DM_KhuyenMai_ChiTiet>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TenantId = AbpSession.TenantId??1;
            return result;
        }
        [NonAction]
        public async Task<KhuyenMaiChiTietDto> Update(CreateOrEditKhuyenMaiChiTietDto input,DM_KhuyenMai_ChiTiet oldData)
        {
            KhuyenMaiChiTietDto result = new KhuyenMaiChiTietDto();
            oldData.IdDonViQuiDoiMua = input.IdDonViQuyDoiMua;
            oldData.IdDonViQuiDoiTang = input.IdDonViQuyDoiTang;
            oldData.IdKhuyenMai = input.IdKhuyenMai;
            oldData.IdNhomHangMua = input.IdNhomHangMua;
            oldData.IdNhomHangTang = input.IdNhomHangTang;
            oldData.SoLuongMua = input.SoLuongMua;
            oldData.SoLuongTang = input.SoLuongTang;
            oldData.STT = input.STT;
            oldData.GiaKhuyenMai = input.GiaKhuyenMai;
            oldData.GiamGia = input.GiamGia;
            oldData.TongTienHang = input.TongTienHang;
            oldData.GiamGiaTheoPhanTram = input.GiamGiaTheoPhanTram;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<KhuyenMaiChiTietDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<KhuyenMaiChiTietDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data!=null)
            {
                data.IsDeleted = true;
                data.DeleterUserId= AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                _repository.Update(data);
                return ObjectMapper.Map<KhuyenMaiChiTietDto>(data);
            }
            return new KhuyenMaiChiTietDto();
        }
        public async Task<CreateOrEditKhuyenMaiChiTietDto> GetFOrEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditKhuyenMaiChiTietDto>(data);
            }
            return new CreateOrEditKhuyenMaiChiTietDto();
        }
        public async Task<PagedResultDto<KhuyenMaiChiTietDto>> GetALl(PagedRequestDto input)
        {
            PagedResultDto<KhuyenMaiChiTietDto> result = new PagedResultDto<KhuyenMaiChiTietDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<KhuyenMaiChiTietDto>>(data);
            return result;
        }
    }
}
