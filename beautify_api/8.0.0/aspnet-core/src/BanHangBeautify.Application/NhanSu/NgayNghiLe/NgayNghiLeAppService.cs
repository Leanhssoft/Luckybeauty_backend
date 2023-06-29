using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
using BanHangBeautify.NhanSu.NgayNghiLe.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NgayNghiLe
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu_NgayNghiLe)]
    public class NgayNghiLeAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_NgayNghiLe, Guid> _ngayNghiLeService;
        private readonly INgayNghiLeRepository _ngayNghiLeReponsitory;
        public NgayNghiLeAppService(IRepository<DM_NgayNghiLe, Guid> ngayNghiLeService, INgayNghiLeRepository ngayNghiLeReponsitory)
        {
            _ngayNghiLeService = ngayNghiLeService;
            _ngayNghiLeReponsitory = ngayNghiLeReponsitory;
        }
        public async Task<PagedResultDto<NgayNghiLeDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount == 0 || input.SkipCount == 1 ? 0 : ((input.SkipCount - 1) * input.MaxResultCount);
            input.Keyword = !string.IsNullOrEmpty(input.Keyword) ? input.Keyword : "";

            return await _ngayNghiLeReponsitory.GetAll(input, AbpSession.TenantId ?? 1);
        }
        [HttpPost]
        public async Task<CreateOrEditNgayNghiLeDto> GetForEdit(Guid id)
        {
            CreateOrEditNgayNghiLeDto result = new CreateOrEditNgayNghiLeDto();
            var data =await _ngayNghiLeService.FirstOrDefaultAsync(x=>x.Id== id);
            if (data!=null)
            {
                result = ObjectMapper.Map<CreateOrEditNgayNghiLeDto>(data);
            }
            return result;
        }
        [HttpPost]
        public async Task<NgayNghiLeDto> CreateOrEdit(CreateOrEditNgayNghiLeDto input)
        {
            NgayNghiLeDto result = new NgayNghiLeDto();
            var checkExists =await _ngayNghiLeService.FirstOrDefaultAsync(x => x.Id == input.Id);
            result = checkExists == null ? await Create(input) : await Edit(input,checkExists);
            return result;
        }
        [NonAction]
        public async Task<NgayNghiLeDto> Create(CreateOrEditNgayNghiLeDto input)
        {
            DM_NgayNghiLe dto = new DM_NgayNghiLe();
            dto.Id = Guid.NewGuid();
            dto.TenNgayLe = input.TenNgayLe;
            dto.TuNgay = input.TuNgay;
            dto.DenNgay = input.DenNgay.Date.AddDays(1).AddSeconds(-1); // Set time to 23:59:59 on the same day
            var all = dto.DenNgay.Subtract(dto.TuNgay);
            var day = Math.Round(all.TotalDays);
            dto.TongSoNgay = (int)day; // Directly cast the rounded value to an int
            dto.CreationTime = DateTime.Now;
            dto.CreatorUserId = AbpSession.UserId;
            dto.TenantId = AbpSession.TenantId ?? 1;
            dto.IsDeleted = false;
            dto.TrangThai = 0;
            _ngayNghiLeService.Insert(dto);
            var result = ObjectMapper.Map<NgayNghiLeDto>(dto);
            return result;
        }
        [NonAction]
        public async Task<NgayNghiLeDto> Edit(CreateOrEditNgayNghiLeDto input,DM_NgayNghiLe oldData)
        {
            oldData.TenNgayLe = input.TenNgayLe;
            oldData.TuNgay = input.TuNgay;
            oldData.DenNgay = input.DenNgay.Date.AddDays(1).AddSeconds(-1); // Set time to 23:59:59 on the same day
            var all = oldData.DenNgay.Subtract(oldData.TuNgay);
            var day = Math.Round(all.TotalDays);
            oldData.TongSoNgay = (int)day; // Directly cast the rounded value to an int
            oldData.LastModifierUserId = AbpSession.UserId;
            oldData.LastModificationTime = DateTime.Now;
            _ngayNghiLeService.Update(oldData);
            var result = ObjectMapper.Map<NgayNghiLeDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var checkExists =await _ngayNghiLeService.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExists!=null)
            {
                checkExists.IsDeleted= true;
                checkExists.DeletionTime= DateTime.Now;
                checkExists.DeleterUserId = AbpSession.UserId;
                _ngayNghiLeService.Update(checkExists);
                result = true;
            }
            return result;
        }
    }
}