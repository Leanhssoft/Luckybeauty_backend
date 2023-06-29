﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository;
using BanHangBeautify.Common.Consts;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon
{
    [AbpAuthorize(PermissionNames.Pages_ChietKhauHoaDon)]
    public class ChietKhauHoaDonAppService:SPAAppServiceBase
    {
        private readonly IRepository<NS_ChietKhauHoaDon, Guid> _repository;
        private readonly IChietKhauHoaDonRepository _chietKhauHoaDonRepository;
        public ChietKhauHoaDonAppService(IRepository<NS_ChietKhauHoaDon, Guid> repository,IChietKhauHoaDonRepository chietKhauHoaDonRepository)
        {
            _repository = repository;
            _chietKhauHoaDonRepository = chietKhauHoaDonRepository;
        }
        public async Task<ChietKhauHoaDonDto> CreateOrEdit(CreateOrEditChietKhauHDDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<ChietKhauHoaDonDto> Create(CreateOrEditChietKhauHDDto input)
        {
            ChietKhauHoaDonDto result = new ChietKhauHoaDonDto();
            NS_ChietKhauHoaDon data = new NS_ChietKhauHoaDon();
            data = ObjectMapper.Map<NS_ChietKhauHoaDon>(input);
            data.Id = Guid.NewGuid();
            data.ChungTuApDung = string.Join(";",input.ChungTuApDung.ToArray());
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<ChietKhauHoaDonDto>(input);
            return result;
        }
        [NonAction]
        public async Task<ChietKhauHoaDonDto> Update(CreateOrEditChietKhauHDDto input, NS_ChietKhauHoaDon oldData)
        {
            ChietKhauHoaDonDto result = new ChietKhauHoaDonDto();
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.LoaiChietKhau = input.LoaiChietKhau;
            oldData.GiaTriChietKhau = input.GiaTriChietKhau;
            oldData.ChungTuApDung = string.Join(";", input.ChungTuApDung.ToArray());
            oldData.TrangThai = 0;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<ChietKhauHoaDonDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<ChietKhauHoaDonDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<ChietKhauHoaDonDto>(data);
            }
            return new ChietKhauHoaDonDto();
        }
        public async Task<CreateOrEditChietKhauHDDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditChietKhauHDDto>(data);
            }
            return new CreateOrEditChietKhauHDDto();
        }
        public async Task<PagedResultDto<ChietKhauHoaDonItemDto>> GetAll(PagedRequestDto input,Guid? idChiNhanh)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount -1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chietKhauHoaDonRepository.GetAll(input, AbpSession.TenantId ?? 1, idChiNhanh);
        }
    }
}
