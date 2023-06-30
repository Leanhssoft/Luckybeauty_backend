using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.LichLamViec.Dto;
using BanHangBeautify.NhanSu.LichLamViec.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu_LichLamViec)]
    public class LichLamViecAppService: SPAAppServiceBase
    {
        private readonly IRepository<NS_LichLamViec,Guid> _lichLamViecService;
        private readonly ILichLamViecRespository _lichLamViecRespository;
        public LichLamViecAppService(
            IRepository<NS_LichLamViec,Guid> lichLamViecService,
            ILichLamViecRespository lichLamViecRespository
         )
        {
            _lichLamViecService = lichLamViecService;
            _lichLamViecRespository = lichLamViecRespository;
        }

        public async Task<LichLamViecDto> CreateOrEdit(CreateOrEditLichLamViecDto input) {
            var isExist = await _lichLamViecService.FirstOrDefaultAsync(x=>x.Id== input.Id);
            if (isExist==null)
            {
                return await Create(input);
            }
            return await Update(input,isExist);
        }
        [NonAction]
        public async Task<LichLamViecDto> Create(CreateOrEditLichLamViecDto input) {
            LichLamViecDto result = new LichLamViecDto();
            NS_LichLamViec data = new NS_LichLamViec();
            data = ObjectMapper.Map<NS_LichLamViec>(input);
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.TrangThai = 0;
            result = ObjectMapper.Map<LichLamViecDto>(data);
            await _lichLamViecService.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<LichLamViecDto> Update(CreateOrEditLichLamViecDto input,NS_LichLamViec oldData) {
            LichLamViecDto result = new LichLamViecDto();
            oldData.GiaTriLap = input.GiaTriLap;
            oldData.KieuLapLai = input.KieuLapLai;
            oldData.LapLai = input.LapLai;
            oldData.TuNgay = input.TuNgay;
            oldData.DenNgay = input.DenNgay;
            oldData.IdNhanVien = input.IdNhanVien;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.LastModificationTime= DateTime.Now;
            oldData.LastModifierUserId= AbpSession.UserId;
            await _lichLamViecService.UpdateAsync(oldData);
            result = ObjectMapper.Map<LichLamViecDto>(input);
            return result;

        }
        [HttpPost]
        public async Task<LichLamViecDto> Delete(Guid id) { 
            var checkExist =await _lichLamViecService.FirstOrDefaultAsync(x=>x.Id==id);
            if (checkExist!=null)
            {
                checkExist.IsDeleted = true;
                checkExist.DeleterUserId = AbpSession.UserId;
                checkExist.DeletionTime= DateTime.Now;
                await _lichLamViecService.UpdateAsync(checkExist);
                return ObjectMapper.Map<LichLamViecDto>(checkExist);
            }
            return new LichLamViecDto();
        }
        public async Task<NS_LichLamViec> GetDetail(Guid id) {
            var checkExist =await _lichLamViecService.FirstOrDefaultAsync(x=>x.Id==id);
            if (checkExist!=null)
            {
                return checkExist;
            }
            return new NS_LichLamViec();
        }
        public async Task<CreateOrEditLichLamViecDto> GetForEdit(Guid id) {

            var checkExist =await _lichLamViecService.FirstOrDefaultAsync(x=>x.Id==id);
            if (checkExist!=null)
            {
                var createOrEdit = ObjectMapper.Map<CreateOrEditLichLamViecDto>(checkExist);
                return createOrEdit;
            }
            return new CreateOrEditLichLamViecDto();
        }
        public async Task<PagedResultDto<LichLamViecDto>> GetAll(PagedRequestDto input) {
            input.MaxResultCount = input.MaxResultCount;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            PagedResultDto<LichLamViecDto> result = new PagedResultDto<LichLamViecDto>();
            var data =await _lichLamViecService.GetAll()
                .Where(x=>x.TenantId == (AbpSession.TenantId??1) && x.IsDeleted==false)
                .OrderByDescending(x=>x.CreationTime)
                .ToListAsync();
            result.TotalCount = data.Count;
            data = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<LichLamViecDto>>(data);
            return result;
        }

        public async Task<PagedResultDto<LichLamViecNhanVien>> GetAllLichLamViecWeek(PagedRequestLichLamViecDto input)
        {
            DateTime now = DateTime.Now;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            DateTime date = input.DateFrom;
            string firstDayOfWeek = date.AddDays(DayOfWeek.Monday - date.DayOfWeek).ToString("yyyy-MM-dd");
            string lastDayOfWeek = date.AddDays(DayOfWeek.Sunday - date.DayOfWeek + 7).ToString("yyyy-MM-dd");
            input.DateFrom = DateTime.Parse(firstDayOfWeek);
            input.DateTo = DateTime.Parse(lastDayOfWeek);
            return await _lichLamViecRespository.GetAllLichLamViecWeek(input,AbpSession.TenantId ?? 1);
        }
    }
}
