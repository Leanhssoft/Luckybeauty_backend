using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.SMS.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.SMS
{
    [AbpAuthorize(PermissionNames.Pages_HeThongSMS)]
    public class HeThongSMSAppService : SPAAppServiceBase
    {
        private readonly IRepository<HeThong_SMS, Guid> _repository;
        public HeThongSMSAppService(IRepository<HeThong_SMS, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<HeThongSMSDto> CrreateOrEdit(CreateOrEditHeThongSMSDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (input == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<HeThongSMSDto> Create(CreateOrEditHeThongSMSDto input)
        {
            HeThongSMSDto result = new HeThongSMSDto();
            HeThong_SMS data = new HeThong_SMS();
            data = ObjectMapper.Map<HeThong_SMS>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<HeThongSMSDto>(input);
            return result;
        }
        [NonAction]
        public async Task<HeThongSMSDto> Update(CreateOrEditHeThongSMSDto input, HeThong_SMS oldData)
        {
            HeThongSMSDto result = new HeThongSMSDto();
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.IdTinNhan = input.IdTinNhan;
            oldData.IdKhachHang = input.IdKhachHang;
            oldData.IdNguoiGui = input.IdNguoiGui;
            oldData.IdHoaDon = input.IdHoaDon;
            oldData.SoTinGui = input.SoTinGui;
            oldData.LoaiTin = input.LoaiTin;
            oldData.NoiDungTin = input.NoiDungTin;
            oldData.ThoiGianGui = input.ThoiGianGui;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<HeThongSMSDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<HeThongSMSDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<HeThongSMSDto>(data);
            }
            return new HeThongSMSDto();
        }
        public async Task<CreateOrEditHeThongSMSDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            }
            return new CreateOrEditHeThongSMSDto();
        }
        public async Task<PagedResultDto<HeThongSMSDto>> GetAll(PagedRequestDto input)
        {
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            PagedResultDto<HeThongSMSDto> result = new PagedResultDto<HeThongSMSDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<HeThongSMSDto>>(data);
            return result;
        }
    }
}
