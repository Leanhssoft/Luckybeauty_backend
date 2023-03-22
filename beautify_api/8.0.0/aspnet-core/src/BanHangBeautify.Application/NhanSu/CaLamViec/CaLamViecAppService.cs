using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.CaLamViec
{
    [AbpAuthorize(PermissionNames.Pages_CaLamViec)]
    public class CaLamViecAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_CaLamViec, Guid> _repository;
        public CaLamViecAppService(IRepository<NS_CaLamViec, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<CaLamViecDto> CreateOrEdit(CreateOrEditCaLamViecDto dto)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (caLamViec == null)
            {
                return await Create(dto);
            }
            return await Edit(dto, caLamViec);
        }
        [NonAction]
        public async Task<CaLamViecDto> Create(CreateOrEditCaLamViecDto dto)
        {
            NS_CaLamViec data = new NS_CaLamViec();
            data.Id = Guid.NewGuid();
            data.MaCa = dto.MaCa;
            data.TenCa = dto.TenCa;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.GioVao = DateTime.Parse(dto.GioVao.ToString("HH:mm"));
            data.GioRa = DateTime.Parse(dto.GioRa.ToString("HH:mm"));
            data.TongGioCong = (float)(data.GioVao.Subtract(data.GioRa).TotalMinutes / 60);
            data.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<CaLamViecDto> Edit(CreateOrEditCaLamViecDto dto, NS_CaLamViec data)
        {
            data.MaCa = dto.MaCa;
            data.TenCa = dto.TenCa;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.GioVao = DateTime.Parse(dto.GioVao.ToString("HH:mm"));
            data.GioRa = DateTime.Parse(dto.GioRa.ToString("HH:mm"));
            data.TongGioCong = (float)(data.GioVao.Subtract(data.GioRa).TotalMinutes / 60);
            data.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.UpdateAsync(data);
            return result;
        }
        public async Task<CaLamViecDto> Delete(Guid Id)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == Id);
            if (caLamViec != null)
            {
                caLamViec.TrangThai = 1;
                caLamViec.IsDeleted = true;
                caLamViec.DeletionTime = DateTime.Now;
                caLamViec.DeleterUserId = AbpSession.UserId;
                await _repository.UpdateAsync(caLamViec);
                return ObjectMapper.Map<CaLamViecDto>(caLamViec);
            }
            return new CaLamViecDto();
        }
        public async Task<ListResultDto<NS_CaLamViec>> GetAll(PagedResultRequestDto input, string keyWord)
        {
            ListResultDto<NS_CaLamViec> result = new ListResultDto<NS_CaLamViec>();
            var lstCaLamViec = await _repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(keyWord))
            {
                lstCaLamViec = lstCaLamViec.Where(
                    x => x.TenCa.Contains(keyWord) || x.MaCa.Contains(keyWord) || x.TongGioCong.ToString().Contains(keyWord) ||
                    x.GioVao.ToString().Contains(keyWord) || x.GioRa.ToString().Contains(keyWord)
                    ).
                    ToList();
            }
            input.MaxResultCount = 10;
            input.SkipCount = input.SkipCount > 0 ? (input.SkipCount * 10) : 0;
            result.Items = lstCaLamViec.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }
    }
}
