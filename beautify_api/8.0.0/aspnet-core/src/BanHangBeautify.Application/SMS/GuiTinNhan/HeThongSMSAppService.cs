﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.SMS.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.SMS.GuiTinNhan
{
    [AbpAuthorize(PermissionNames.Pages_HeThongSMS)]
    public class HeThongSMSAppService : SPAAppServiceBase
    {
        private readonly IRepository<HeThong_SMS, Guid> _hethongSMS;
        public readonly IHeThongSMSRepository _repoSMS;

        public HeThongSMSAppService(IRepository<HeThong_SMS, Guid> repository, IHeThongSMSRepository repoSMS)
        {
            _hethongSMS = repository;
            _repoSMS = repoSMS;
        }
        [HttpPost]
        public async Task<CreateOrEditHeThongSMSDto> Insert_HeThongSMS(CreateOrEditHeThongSMSDto input)
        {
            HeThong_SMS data = ObjectMapper.Map<HeThong_SMS>(input);
            data.Id = Guid.NewGuid();
            data.ThoiGianGui = DateTime.Now;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IdNguoiGui = AbpSession.UserId;
            data.IsDeleted = false;
            await _hethongSMS.InsertAsync(data);
            CreateOrEditHeThongSMSDto result = ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            return result;
        }
        [HttpGet]
        public async Task<CreateOrEditHeThongSMSDto> HeThongSMS_DeleteById(Guid id)
        {
            var data = await _hethongSMS.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _hethongSMS.UpdateAsync(data);
                return ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            }
            return new CreateOrEditHeThongSMSDto();
        }
        [HttpGet]
        public async Task<CreateOrEditHeThongSMSDto> GetForEdit(Guid id)
        {
            var data = await _hethongSMS.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            }
            return new CreateOrEditHeThongSMSDto();
        }
        [HttpPost]
        public async Task<PagedResultDto<CreateOrEditHeThongSMSDto>> GetListSMS(ParamSearch input)
        {
            return await _repoSMS.GetListSMS(input);
        }
        [HttpPost]
        public async Task<List<CustomerBasicDto>> JqAutoCustomer_byIdLoaiTin(CommonClass.ParamSearch input, int? idLoaiTin = 1)
        {
            try
            {
                return await _repoSMS.JqAutoCustomer_byIdLoaiTin(input, idLoaiTin);
            }
            catch (Exception)
            {
                return new List<CustomerBasicDto>();
            }
        }

        [HttpPost]
        public async Task<PagedResultDto<PageKhachHangSMSDto>> GetListCustomer_byIdLoaiTin(CommonClass.ParamSearch input, int? idLoaiTin = 1)
        {
            try
            {
                return await _repoSMS.GetListCustomer_byIdLoaiTin(input, idLoaiTin);
            }
            catch (Exception)
            {
                return new PagedResultDto<PageKhachHangSMSDto>();
            }
        }
        public async Task<bool> ThemMoi_NhatKyGuiTin(NhatKyGuiTinSMSDto input)
        {
            var data = await _repoSMS.InsertNhatKyGuiTinSMS(input, AbpSession.TenantId ?? 1);
            return data > 0; ;
        }
    }
}
