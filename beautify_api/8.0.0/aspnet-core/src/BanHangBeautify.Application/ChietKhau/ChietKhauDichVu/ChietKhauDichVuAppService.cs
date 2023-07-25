using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Repository;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu
{
    [AbpAuthorize(PermissionNames.Pages_ChietKhauDichVu)]
    public class ChietKhauDichVuAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_ChietKhauDichVu, Guid> _hoahongDichVu;
        private readonly IRepository<DM_HangHoa, Guid> _hangHoaRepository;
        private readonly IChietKhauDichVuRepository _chietKhauDichVuRepository;
        public ChietKhauDichVuAppService(IRepository<NS_ChietKhauDichVu, Guid> repository, IRepository<DM_HangHoa, Guid> hangHoaRepository,
             IChietKhauDichVuRepository chietKhauDichVuRepository
        )
        {
            _hoahongDichVu = repository;
            _hangHoaRepository = hangHoaRepository;
            _chietKhauDichVuRepository = chietKhauDichVuRepository;
        }
        [AbpAuthorize(PermissionNames.Pages_ChietKhauDichVu_Create, PermissionNames.Pages_ChietKhauDichVu_Edit)]
        public async Task<ExecuteResultDto> CreateOrEdit(CreateOrEditChietKhauDichVuDto input)
        {
            var checkExist = await _hoahongDichVu.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<ExecuteResultDto> Create(CreateOrEditChietKhauDichVuDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                NS_ChietKhauDichVu data = new NS_ChietKhauDichVu();
                data = ObjectMapper.Map<NS_ChietKhauDichVu>(input);
                data.Id = Guid.NewGuid();
                data.IdDonViQuiDoi = input.IdDonViQuiDoi;
                data.CreationTime = DateTime.Now;
                data.CreatorUserId = AbpSession.UserId;
                data.TenantId = AbpSession.TenantId ?? 1;
                data.IsDeleted = false;
                await _hoahongDichVu.InsertAsync(data);
                result.Message = "Thêm mới thành công !";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Detail = ex.Message;
                result.Message = "Thêm mới thất bại !";
                result.Status = "error";
            }

            return result;
        }
        [NonAction]
        public async Task<ExecuteResultDto> Update(CreateOrEditChietKhauDichVuDto input, NS_ChietKhauDichVu oldData)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                oldData.IdDonViQuiDoi = input.IdDonViQuiDoi;
                oldData.IdNhanVien = input.IdNhanVien;
                oldData.GiaTri = input.GiaTri;
                oldData.LaPhanTram = input.LaPhanTram;
                oldData.LoaiChietKhau = input.LoaiChietKhau;
                oldData.TrangThai = 1;
                oldData.LastModificationTime = DateTime.Now;
                oldData.LastModifierUserId = AbpSession.UserId;
                await _hoahongDichVu.UpdateAsync(oldData);
                result.Message = "Cập nhật thành công !";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Detail = ex.Message;
                result.Message = "Cập nhật thất bại !";
                result.Status = "error";
            }
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChietKhauDichVu_Delete)]
        public async Task<ChietKhauDichVuDto> Delete(Guid id)
        {
            var data = await _hoahongDichVu.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _hoahongDichVu.UpdateAsync(data);
                return ObjectMapper.Map<ChietKhauDichVuDto>(data);
            }
            return new ChietKhauDichVuDto();
        }
        public async Task<CreateOrEditChietKhauDichVuDto> GetForEdit(Guid id)
        {
            var data = await _hoahongDichVu.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditChietKhauDichVuDto>(data);
            }
            return new CreateOrEditChietKhauDichVuDto();
        }
        public async Task<PagedResultDto<ChietKhauDichVuDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<ChietKhauDichVuDto> result = new PagedResultDto<ChietKhauDichVuDto>();
            var lstData = await _hoahongDichVu.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<ChietKhauDichVuDto>>(lstData);
            return result;
        }

        public async Task<List<CreateOrEditChietKhauDichVuDto>> GetHoaHongNV_theoDichVu(Guid idNhanVien, Guid idDonViQuyDoi)
        {
            List<NS_ChietKhauDichVu> data = await _hoahongDichVu.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IdNhanVien == idNhanVien && x.IdDonViQuiDoi == idDonViQuyDoi).ToListAsync();
            if (data != null)
            {
                return ObjectMapper.Map<List<CreateOrEditChietKhauDichVuDto>>(data);
            }
            return new List<CreateOrEditChietKhauDichVuDto>();
        }
        public async Task<List<CreateOrEditChietKhauDichVuDto>> GetAllHoaHong_theoNhanVien(Guid idNhanVien)
        {
            var data = await _hoahongDichVu.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IdNhanVien == idNhanVien).ToListAsync();
            if (data != null)
            {
                return ObjectMapper.Map<List<CreateOrEditChietKhauDichVuDto>>(data);
            }
            return new List<CreateOrEditChietKhauDichVuDto>();
        }
        public async Task<List<CreateOrEditChietKhauDichVuDto>> GetAllHoaHong_theoDichVu(Guid idDonViQuyDoi)
        {
            var data = await _hoahongDichVu.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IdDonViQuiDoi == idDonViQuyDoi).ToListAsync();
            if (data != null)
            {
                return ObjectMapper.Map<List<CreateOrEditChietKhauDichVuDto>>(data);
            }
            return new List<CreateOrEditChietKhauDichVuDto>();
        }
        public async Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAccordingByNhanVien(PagedRequestDto input, Guid idNhanVien, Guid idChiNhanh)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chietKhauDichVuRepository.GetAll(input, AbpSession.TenantId ?? 1, idNhanVien, idChiNhanh);
        }
    }
}
