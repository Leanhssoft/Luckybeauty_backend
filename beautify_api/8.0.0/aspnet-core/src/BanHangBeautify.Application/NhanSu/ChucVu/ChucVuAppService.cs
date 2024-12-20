﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.ChucVu.Dto;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.ChucVu
{
    [AbpAuthorize(PermissionNames.Pages_ChucVu)]
    public class ChucVuAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_ChucVu, Guid> _repository;
        private readonly INhatKyThaoTacAppService _audiLogService;
        public ChucVuAppService(IRepository<NS_ChucVu, Guid> repository, INhatKyThaoTacAppService audiLogService)
        {
            _repository = repository;
            _audiLogService = audiLogService;
        }
        [AbpAuthorize(PermissionNames.Pages_ChucVu_Create, PermissionNames.Pages_ChucVu_Edit)]
        public async Task<ChucVuDto> CreateOrEdit(CreateOrEditChucVuDto dto)
        {
            try
            {
                var find = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (find == null)
                {
                    return await Create(dto);
                }
                else
                {
                    return await Edit(dto, find);
                }
            }
            catch (Exception)
            {
                return new ChucVuDto();
            }
        }
        [NonAction]
        public async Task<ChucVuDto> Create(CreateOrEditChucVuDto dto)
        {
            NS_ChucVu chucVu = new NS_ChucVu();

            chucVu.Id = Guid.NewGuid();
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var countIdChucVu = _repository.Count() + 1;
                if (countIdChucVu.ToString().Length >= 3)
                {
                    chucVu.MaChucVu = "CV" + (countIdChucVu + 1).ToString();
                }
                else if (countIdChucVu.ToString().Length == 2)
                {
                    chucVu.MaChucVu = "CV0" + (countIdChucVu + 1).ToString();
                }
                else
                {
                    chucVu.MaChucVu = "CV00" + (countIdChucVu + 1).ToString();
                }

            }
            chucVu.TenChucVu = dto.TenChucVu;
            chucVu.TrangThai = 1;
            chucVu.MoTa = dto.MoTa;
            chucVu.TenantId = AbpSession.TenantId ?? 1;
            chucVu.CreatorUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<ChucVuDto>(chucVu);
            await _repository.InsertAsync(chucVu);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Chức vụ";
            nhatKyThaoTacDto.NoiDung = "Thêm mới chức vụ: " + chucVu.TenChucVu + "(" + chucVu.MaChucVu + ")";
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới chức vụ: " + chucVu.TenChucVu + "(" + chucVu.MaChucVu + ")";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<ChucVuDto> Edit(CreateOrEditChucVuDto dto, NS_ChucVu chucVu)
        {
            chucVu.MaChucVu = dto.MaChucVu;
            chucVu.TenChucVu = dto.TenChucVu;
            chucVu.TrangThai = dto.TrangThai;
            chucVu.MoTa = dto.MoTa;
            chucVu.LastModifierUserId = AbpSession.UserId;
            chucVu.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<ChucVuDto>(chucVu);
            await _repository.UpdateAsync(chucVu);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Chức vụ";
            nhatKyThaoTacDto.NoiDung = "Sửa thông tin chức vụ: " + chucVu.TenChucVu + "(" + chucVu.MaChucVu + ")";
            nhatKyThaoTacDto.NoiDungChiTiet = "Sửa thông tin chức vụ: " + chucVu.TenChucVu + "(" + chucVu.MaChucVu + ")";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Pages_ChucVu_Delete)]
        public async Task<ChucVuDto> Delete(Guid Id)
        {
            var find = await _repository.FirstOrDefaultAsync(x => x.Id == Id);
            if (find != null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                find.TrangThai = 0;
                var result = ObjectMapper.Map<ChucVuDto>(find);
                await _repository.UpdateAsync(find);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Chức vụ";
                nhatKyThaoTacDto.NoiDung = "Xóa chức vụ: " + find.TenChucVu + "(" + find.MaChucVu + ")";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa chức vụ: " + find.TenChucVu + "(" + find.MaChucVu + ")";
                await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return result;
            }
            return new ChucVuDto();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChucVu_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            {
                var finds = await _repository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (finds != null && finds.Count > 0)
                {
                    _repository.RemoveRange(finds);
                    result.Status = "success";
                    result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                    var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                    nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                    nhatKyThaoTacDto.ChucNang = "Chức vụ";
                    nhatKyThaoTacDto.NoiDung = "Xóa nhiều chức vụ: " + string.Join(",", finds.SelectMany(x => x.TenChucVu).ToList());
                    nhatKyThaoTacDto.NoiDungChiTiet = "Xóa nhiều chức vụ: " + string.Join(",", finds.SelectMany(x => x.TenChucVu).ToList());
                }
                return result;
            }
        }
        public async Task<NS_ChucVu> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<PagedResultDto<NS_ChucVu>> GetAll(PagedResultRequestDto input, string keyWord)
        {
            PagedResultDto<NS_ChucVu> result = new PagedResultDto<NS_ChucVu>();
            try
            {
                var lstChucVu = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
                result.TotalCount = lstChucVu.Count;
                if (!string.IsNullOrEmpty(keyWord))
                {
                    lstChucVu = lstChucVu.Where(x => x.TenChucVu.Contains(keyWord) || x.MaChucVu.Contains(keyWord)).ToList();
                }
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                result.Items = lstChucVu.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }
            catch (Exception)
            {
                result.Items = null;
                result.TotalCount = 0;
            }

            return result;
        }
    }
}
