﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using BanHangBeautify.NhanSu.CaLamViec.Repository;
using Castle.MicroKernel.SubSystems.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.CaLamViec
{
    [AbpAuthorize(PermissionNames.Pages_CaLamViec)]
    public class CaLamViecAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_CaLamViec, Guid> _repository;
        private readonly ICaLamViecRepository _caLamViecRepository;
        public CaLamViecAppService(IRepository<NS_CaLamViec, Guid> repository,ICaLamViecRepository caLamViecRepository)
        {
            _repository = repository;
            _caLamViecRepository = caLamViecRepository;
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
            var count =await  _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId??1)).ToListAsync();
            data.Id = Guid.NewGuid();
            data.MaCa = "MS00"+ (count.Count+1).ToString();
            data.TenCa = dto.TenCa;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.GioVao = DateTime.Parse(dto.GioVao.ToString());
            data.GioRa = DateTime.Parse(dto.GioRa.ToString());
            data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60);
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
            data.GioVao = DateTime.Parse(dto.GioVao.ToString());
            data.GioRa = DateTime.Parse(dto.GioRa.ToString());
            data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60);
            data.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.UpdateAsync(data);
            return result;
        }
        [HttpPost]
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
        public async Task<CreateOrEditCaLamViecDto> GetForEdit(Guid id)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (caLamViec != null)
            { var data = ObjectMapper.Map<CreateOrEditCaLamViecDto>(caLamViec);
                data.GioVao = caLamViec.GioVao.ToString("HH:mm");
                data.GioRa = caLamViec.GioRa.ToString("HH:mm");
                return data;
            }
            return new CreateOrEditCaLamViecDto();
        }
        public async Task<PagedResultDto<CaLamViecDto>> GetAll(PagedRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Keyword))
            {
                input.Keyword = "";
                
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            return await _caLamViecRepository.GetAll(input, AbpSession.TenantId ?? 1);
        }
    }
}
