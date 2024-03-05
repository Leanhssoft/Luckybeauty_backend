using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Repository;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.ESMS;
using BanHangBeautify.Storage;
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
        private readonly IChietKhauDichVuRepository _chietKhauDichVuRepository;
        private readonly IExcelBase _excelBase;

        public ChietKhauDichVuAppService(IRepository<NS_ChietKhauDichVu, Guid> repository,
             IChietKhauDichVuRepository chietKhauDichVuRepository,
             IExcelBase excelBase
        )
        {
            _hoahongDichVu = repository;
            _chietKhauDichVuRepository = chietKhauDichVuRepository;
            _excelBase = excelBase;
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
                if (input.IdNhanViens != null && input.IdNhanViens.Count > 0)
                {
                    foreach (var item in input.IdNhanViens)
                    {
                        NS_ChietKhauDichVu data = new NS_ChietKhauDichVu();
                        data.Id = Guid.NewGuid();
                        data.IdNhanVien = item;
                        data.LaPhanTram = input.LaPhanTram;
                        data.LoaiChietKhau = input.LoaiChietKhau;
                        data.IdChiNhanh = input.IdChiNhanh;
                        data.GiaTri = input.GiaTri;
                        data.TrangThai = 1;
                        data.IdDonViQuiDoi = input.IdDonViQuiDoi;
                        data.CreationTime = DateTime.Now;
                        data.CreatorUserId = AbpSession.UserId;
                        data.TenantId = AbpSession.TenantId ?? 1;
                        data.IsDeleted = false;
                        await _hoahongDichVu.InsertAsync(data);
                    }
                    result.Message = "Thêm mới thành công !";
                    result.Status = "success";
                }


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
                oldData.IdNhanVien = input.IdNhanViens[0];
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
        public async Task<bool> UpdateSetup_HoaHongDichVu_ofNhanVien(CreateOrEditChietKhauDichVuDto input)
        {
            // delete & add again
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var lstDelete = await _hoahongDichVu.GetAllListAsync(x => input.IdNhanViens.Contains(x.IdNhanVien)
                  && x.IdDonViQuiDoi == input.IdDonViQuiDoi && x.LoaiChietKhau == input.LoaiChietKhau);
                if (lstDelete != null)
                {
                    _hoahongDichVu.RemoveRange(lstDelete);
                }
            }
            await Create(input);
            return true;
        }


        [HttpPost]
        public async Task<int> AddMultiple_ChietKhauDichVu_toMultipleNhanVien(ChietKhauDichVuDto_AddMultiple param)
        {
            try
            {
                if (param != null)
                {
                    return await _chietKhauDichVuRepository.AddMultiple_ChietKhauDichVu_toMultipleNhanVien(param, AbpSession.TenantId ?? 1, AbpSession.UserId);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        [HttpGet]
        public async Task<int> ApplyAll_SetupHoaHongDV(ChietKhauDichVuDto_AddMultiple param, Guid? idNhanVienChosed,
            byte? loaiApDung = 0)
        {
            try
            {
                if (param != null)
                {
                    return await _chietKhauDichVuRepository.ApplyAll_SetupHoaHongDV(param, idNhanVienChosed, loaiApDung, AbpSession.UserId);
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChietKhauDichVu_Delete)]
        public async Task<ExecuteResultDto> Delete(Guid id)
        {
            var data = await _hoahongDichVu.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _hoahongDichVu.DeleteAsync(data);
                return new ExecuteResultDto()
                {
                    Status = "success",
                    Message = "Xóa dữ liệu thành công!"
                };
            }
            return new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi say ra vui lòng thử lại sau!"
            };
        }

        [HttpGet]
        public async Task<bool> DeleteSetup_DichVu_ofNhanVien(List<Guid> arrIdNhanVien, List<Guid> arrIdDonViQuyDoi)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                // không xóa vĩnh viễn dc?? vì GetAllListAsync luôn enable xóa mềm
                var lstDelete = await _hoahongDichVu.GetAllListAsync(x => arrIdNhanVien.Contains(x.IdNhanVien) && arrIdDonViQuyDoi.Contains(x.IdDonViQuiDoi));
                if (lstDelete != null)
                {
                    _hoahongDichVu.RemoveRange(lstDelete);
                }
            }
            return true;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChietKhauDichVu_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            {
                var finds = await _hoahongDichVu.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (finds != null && finds.Count > 0)
                {
                    _hoahongDichVu.RemoveRange(finds);
                    result.Status = "success";
                    result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                }
                return result;
            }
        }
        public async Task<CreateOrEditChietKhauDichVuDto> GetForEdit(Guid id)
        {
            var data = await _hoahongDichVu.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                var mapData = ObjectMapper.Map<CreateOrEditChietKhauDichVuDto>(data);
                var listIdNhanVien = new List<Guid>();
                listIdNhanVien.Add(data.IdNhanVien);
                mapData.IdNhanViens = listIdNhanVien;
                return mapData;
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
        [HttpGet]
        public async Task<List<CreateOrEditChietKhauDichVuDto>> GetHoaHongNV_theoDichVu(Guid idNhanVien, Guid idDonViQuyDoi, Guid? idChiNhanh = null)
        {
            List<NS_ChietKhauDichVu> data = await _hoahongDichVu.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)
            && x.IdNhanVien == idNhanVien && x.IdDonViQuiDoi == idDonViQuyDoi && x.IdChiNhanh == idChiNhanh).ToListAsync();
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
        public async Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAccordingByNhanVien(PagedRequestDto input, Guid? idNhanVien = null, Guid? idChiNhanh = null)
        {
            if (idNhanVien == null || idNhanVien == Guid.Empty)
            {
                idNhanVien = null;
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chietKhauDichVuRepository.GetAll(input, AbpSession.TenantId ?? 1, idNhanVien, idChiNhanh);
        }

        [HttpGet]
        public async Task<PagedResultDto<ChietKhauDichVuItemDto_TachRiengCot>> GetAllSetup_HoaHongDichVu(PagedRequestDto input, Guid? idNhanVien = null, Guid? idChiNhanh = null)
        {
            if (idNhanVien == null || idNhanVien == Guid.Empty)
            {
                idNhanVien = null;
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chietKhauDichVuRepository.GetAllSetup_HoaHongDichVu(input, AbpSession.TenantId ?? 1, idNhanVien, idChiNhanh);
        }

        [HttpGet]
        public async Task<FileDto> ExportToExcel_CaiDat_HoaHongDV(PagedRequestDto input, Guid? idNhanVien = null, Guid? idChiNhanh = null)
        {
            var data = await GetAllSetup_HoaHongDichVu(input, idNhanVien, idChiNhanh);
            var dataExcel = ObjectMapper.Map<List<ChietKhauDichVuItemDto_TachRiengCot>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.TenNhanVien,
                x.TenDichVu,
                x.TenNhomDichVu,
                x.GiaDichVu,
                HoaHongThucHien = string.Concat(x.HoaHongThucHien, " ", x.LaPhanTram_HoaHongThucHien == true ? "%" : "đ"),
                HoaHongTuVan = string.Concat(x.HoaHongTuVan, " ", x.LaPhanTram_HoaHongTuVan == true ? "%" : "đ"),
            }).ToList();

            return _excelBase.WriteToExcel("CaiDat_HoaHongDichVu", @"CaiDat\CaiDat_HoaHongDV_Template.xlsx", dataNew, 4);
        }
    }
}
