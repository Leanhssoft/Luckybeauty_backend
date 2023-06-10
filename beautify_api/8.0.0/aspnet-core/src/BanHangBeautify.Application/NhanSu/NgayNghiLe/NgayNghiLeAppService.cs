using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
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
        private readonly IRepository<DM_NgayNghiLe, Guid> _ngayNghiLeRepository;
        public NgayNghiLeAppService(IRepository<DM_NgayNghiLe, Guid> ngayNghiLeRepository)
        {
            _ngayNghiLeRepository = ngayNghiLeRepository;
        }
        public async Task<PagedResultDto<NgayNghiLeDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<NgayNghiLeDto> result = new PagedResultDto<NgayNghiLeDto>();
            input.SkipCount = input.SkipCount == 0 || input.SkipCount == 1 ? 0 : ((input.SkipCount - 1) * 10);
            input.Keyword = !string.IsNullOrEmpty(input.Keyword) ? input.Keyword : "";
            var data =await _ngayNghiLeRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x=>x.CreationTime).ToListAsync();
            result.TotalCount = data.Count;
            data = data.Where(x=>x.TenNgayLe.Contains(input.Keyword)).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NgayNghiLeDto>>(data);
            return result;
        }
        [HttpPost]
        public async Task<CreateOrEditNgayNghiLeDto> GetForEdit(Guid id)
        {
            CreateOrEditNgayNghiLeDto result = new CreateOrEditNgayNghiLeDto();
            var data =await _ngayNghiLeRepository.FirstOrDefaultAsync(x=>x.Id== id);
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
            var checkExists =await _ngayNghiLeRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
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
            _ngayNghiLeRepository.Insert(dto);
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
            _ngayNghiLeRepository.Update(oldData);
            var result = ObjectMapper.Map<NgayNghiLeDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var checkExists =await _ngayNghiLeRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExists!=null)
            {
                checkExists.IsDeleted= true;
                checkExists.DeletionTime= DateTime.Now;
                checkExists.DeleterUserId = AbpSession.UserId;
                _ngayNghiLeRepository.Update(checkExists);
                result = true;
            }
            return result;
        }
    }
}