﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto;
using BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Repository;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang
{
    [AbpAuthorize]
    public class TaiKhoanNganHangAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_TaiKhoanNganHang, Guid> _dmTaiKhoanNganHang;
        private readonly IRepository<DM_NganHang, Guid> _nganHangRepository;
        private readonly TaiKhoanNganHangRepository _repoBankAcc;

        public TaiKhoanNganHangAppService(IRepository<DM_TaiKhoanNganHang, Guid> repository,
            TaiKhoanNganHangRepository repoBankAcc, IRepository<DM_NganHang, Guid> nganHangRepository)
        {
            _dmTaiKhoanNganHang = repository;
            _repoBankAcc = repoBankAcc;
            _nganHangRepository = nganHangRepository;
        }
        public async Task<TaiKhoanNganHangDto> CreateOrEdit(CreateOrEditTaiKhoanNganHangDto input)
        {
            var checkExist = await _dmTaiKhoanNganHang.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        static string ConvertToUpperCaseWithoutDiacritics(string input)
        {
            // Chuyển chuỗi sang chữ in hoa
            string upperCaseString = input.ToUpperInvariant();

            // Chuyển đổi sang bảng mã Unicode NFC để loại bỏ dấu
            string normalizedString = upperCaseString.Normalize(NormalizationForm.FormD);

            // Loại bỏ các ký tự không phải chữ cái
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    if (c == 'Đ')
                    {
                        stringBuilder.Append('D');
                    }
                    else { stringBuilder.Append(c); }
                    
                }
            }

            return stringBuilder.ToString();
        }
        [NonAction]
        public async Task<TaiKhoanNganHangDto> Create(CreateOrEditTaiKhoanNganHangDto input)
        {
            DM_TaiKhoanNganHang data = ObjectMapper.Map<DM_TaiKhoanNganHang>(input);
            data.Id = Guid.NewGuid();
            data.TenChuThe = ConvertToUpperCaseWithoutDiacritics(input.TenChuThe);
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TrangThai = 1;
            data.TenantId = AbpSession.TenantId ?? 1;
            await _dmTaiKhoanNganHang.InsertAsync(data);
            return ObjectMapper.Map<TaiKhoanNganHangDto>(data);
        }
        [NonAction]
        public async Task<TaiKhoanNganHangDto> Update(CreateOrEditTaiKhoanNganHangDto input, DM_TaiKhoanNganHang oldData)
        {
            TaiKhoanNganHangDto result = new TaiKhoanNganHangDto();
            oldData.IdNganHang = input.IdNganHang;
            oldData.GhiChu = input.GhiChu;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.TenChuThe = ConvertToUpperCaseWithoutDiacritics(input.TenChuThe);
            oldData.SoTaiKhoan = input.SoTaiKhoan;
            oldData.TrangThai = input.TrangThai;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _dmTaiKhoanNganHang.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        public async Task<TaiKhoanNganHangDto> Delete(Guid id)
        {
            var data = await _dmTaiKhoanNganHang.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession?.UserId;
                data.IsDeleted = true;
                await _dmTaiKhoanNganHang.UpdateAsync(data);
                return ObjectMapper.Map<TaiKhoanNganHangDto>(data);
            }
            return new TaiKhoanNganHangDto();
        }
        public async Task<CreateOrEditTaiKhoanNganHangDto> GetForEdit(Guid id)
        {
            var data = await _dmTaiKhoanNganHang.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditTaiKhoanNganHangDto>(data);
            }
            return new CreateOrEditTaiKhoanNganHangDto();
        }
        public async Task<PagedResultDto<TaiKhoanNganHangDto>> GetAll(PagedRequestTaiKhoanNganHang input)
        {
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            PagedResultDto<TaiKhoanNganHangDto> result = new PagedResultDto<TaiKhoanNganHangDto>();
            var lstData = await _dmTaiKhoanNganHang.GetAll().Where(x=>x.IdChiNhanh==input.IdChiNhanh).ToListAsync();
            result.TotalCount = lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            List<TaiKhoanNganHangDto> items = new List<TaiKhoanNganHangDto>();
            foreach (var item in data)
            {
                var tknh =  ObjectMapper.Map<TaiKhoanNganHangDto>(item);
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var nganHang = _nganHangRepository.FirstOrDefault(x => x.Id == tknh.IdNganHang);
                    if (nganHang != null)
                    {
                        tknh.TenNganHang = nganHang.TenNganHang;
                        tknh.MaNganHang = nganHang.MaNganHang;
                        tknh.LogoNganHang = nganHang.Logo;
                    }
                }
                items.Add(tknh);
            }
            result.Items = items;
            return result;
        }
        public async Task<List<TaiKhoanNganHangDto>> GetAllBankAccount(Guid? idChiNhanh = null)
        {
            return await _repoBankAcc.GetAllBankAccount(idChiNhanh);
        }
        public async Task<TaiKhoanNganHangDto> GetDefault_TaiKhoanNganHang(Guid? idChiNhanh = null)
        {
            var dataAll = await _repoBankAcc.GetAllBankAccount(idChiNhanh);
            return dataAll.FirstOrDefault();
        }
    }
}
