﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.MauIn.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Common;

namespace BanHangBeautify.AppDanhMuc.MauIn
{
    //[AbpAuthorize(PermissionNames.Pages_MauIn)]
    public class MauInAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_MauIn, Guid> _dmMauInRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        //public static readonly string App = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //public static readonly string Templates = Path.Combine(App, "Template");
        //private readonly IHostingEnvironment _env;

        public MauInAppService(IRepository<DM_MauIn, Guid> dmMauInRepository, IWebHostEnvironment hostEnvironment)
        {
            _dmMauInRepository = dmMauInRepository;
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost]
        public async Task<MauInDto> InsertMauIn(CreateOrEditMauInDto input)
        {
            DM_MauIn data = ObjectMapper.Map<DM_MauIn>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TrangThai = 1;
            await _dmMauInRepository.InsertAsync(data);
            return ObjectMapper.Map<MauInDto>(data);
        }
        [HttpPost]
        public async Task<MauInDto> UpdateMauIn(CreateOrEditMauInDto input)
        {
            var objUpdate = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (objUpdate != null)
            {
                objUpdate.TenMauIn = input.TenMauIn;
                objUpdate.NoiDungMauIn = input.TenMauIn;
                objUpdate.LaMacDinh = input.LaMacDinh;
                objUpdate.IdChiNhanh = input.IdChiNhanh;
                objUpdate.LoaiChungTu = input.LoaiChungTu;
                objUpdate.LastModificationTime = DateTime.Now;
                objUpdate.LastModifierUserId = AbpSession.UserId;
                await _dmMauInRepository.UpdateAsync(objUpdate);
            }
            return ObjectMapper.Map<MauInDto>(objUpdate);
        }
        [HttpPost]
        public async Task<MauInDto> Delete(Guid id)
        {
            var data = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                data.TrangThai = 0;
                _dmMauInRepository.Update(data);
                return ObjectMapper.Map<MauInDto>(data);
            }
            return new MauInDto();
        }
        public async Task<CreateOrEditMauInDto> GetForEdit(Guid id)
        {
            var data = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditMauInDto>(data);
            }
            return new CreateOrEditMauInDto();
        }
        public async Task<PagedResultDto<MauInDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<MauInDto> result = new PagedResultDto<MauInDto>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var data = await _dmMauInRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = data.Count;
            var lstMauIn = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<MauInDto>>(lstMauIn);
            return result;
        }
        /// <summary>
        /// read content from txt file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFileMauIn(string file = "HoaDonBan.txt")
        {
            var contents = string.Empty;
            var pathFile = Path.Combine(_hostEnvironment.WebRootPath, @"Template\MauIn\", file);

            if (File.Exists(pathFile))
            {
                contents = System.IO.File.ReadAllText(pathFile);
            }
            return contents;
        }
        /// <summary>
        /// read file .txt by loaiMauIn
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tenMauIn"></param>
        /// <returns></returns>
        public string GetContentMauInMacDinh(int type = 1, string tenMauIn = "")
        {
            var contents = string.Empty;
            // mau k80
            if (type == 1)
            {
                var data = Dictionary.DanhSachMauInK80.Where(x => x.Key == tenMauIn);
                if (data != null && data.Count() > 0)
                {
                    contents = GetFileMauIn(data.FirstOrDefault().Value);
                }
            }
            else
            {
                // mau a4
                var data = Dictionary.DanhSachMauInA4.Where(x => x.Key == tenMauIn);
                if (data != null && data.Count() > 0)
                {
                    contents = GetFileMauIn(data.FirstOrDefault().Value);
                }
            }
            return contents;
        }
    }
}
