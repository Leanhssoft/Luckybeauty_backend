using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien_DichVu.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu
{
    [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu)]
    public class NhanVienDichVuAppService : SPAAppServiceBase
    {
        IRepository<DichVu_NhanVien, Guid> _repository;
        public NhanVienDichVuAppService(IRepository<DichVu_NhanVien, Guid> repository)
        {
            _repository = repository;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Create)]
        public async Task<ExecuteResultDto> Create(CreateOrUpdateDichVuNhanVienDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                DichVu_NhanVien rdo = new DichVu_NhanVien();
                rdo.Id = Guid.NewGuid();
                rdo.IdNhanVien = input.IdNhanVien;
                rdo.IdDonViQuyDoi = input.IdHangHoa;
                rdo.TenantId = AbpSession.TenantId ?? 1;
                rdo.CreationTime = DateTime.Now;
                rdo.CreatorUserId = AbpSession.UserId;
                rdo.LastModificationTime = DateTime.Now;
                rdo.LastModifierUserId = AbpSession.UserId;
                rdo.IsDeleted = false;
                await _repository.InsertAsync(rdo);
                result.Message = "Thêm mới thành công!";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = "error";
            }
            return result;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Create)]
        public async Task<ExecuteResultDto> CreateByServiceMany(CreateManyEmployeeDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                List<DichVu_NhanVien> lstDichVuNhanVien = new List<DichVu_NhanVien>();
                if (input.IdNhanViens != null && input.IdNhanViens.Count > 0)
                {
                    foreach (var item in input.IdNhanViens)
                    {
                        var checkExits = _repository.GetAll().Where(x => x.IdDonViQuyDoi == input.IdDonViQuiDoi && x.IdNhanVien == item && x.IsDeleted == false).FirstOrDefault();
                        if (checkExits != null)
                        {
                            continue;
                        }
                        DichVu_NhanVien rdo = new DichVu_NhanVien();
                        rdo.Id = Guid.NewGuid();
                        rdo.IdNhanVien = item;
                        rdo.IdDonViQuyDoi = input.IdDonViQuiDoi;
                        rdo.TenantId = AbpSession.TenantId ?? 1;
                        rdo.CreationTime = DateTime.Now;
                        rdo.CreatorUserId = AbpSession.UserId;
                        rdo.LastModificationTime = DateTime.Now;
                        rdo.LastModifierUserId = AbpSession.UserId;
                        rdo.IsDeleted = false;
                        lstDichVuNhanVien.Add(rdo);
                    }

                }
                await _repository.InsertRangeAsync(lstDichVuNhanVien);
                result.Message = "Thêm mới thành công!";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = "error";
            }
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Create)]
        public async Task<ExecuteResultDto> CreateByEmployeeMany(CreateServiceManyDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                List<DichVu_NhanVien> lstDichVuNhanVien = new List<DichVu_NhanVien>();
                if (input.IdDonViQuiDois != null && input.IdDonViQuiDois.Count > 0)
                {
                    foreach (var item in input.IdDonViQuiDois)
                    {
                        var checkExits = _repository.GetAll().Where(x => x.IdNhanVien == input.IdNhanVien && x.IdDonViQuyDoi == item && x.IsDeleted == false).FirstOrDefault();
                        if (checkExits != null)
                        {
                            continue;
                        }
                        DichVu_NhanVien rdo = new DichVu_NhanVien();
                        rdo.Id = Guid.NewGuid();
                        rdo.IdNhanVien = input.IdNhanVien;
                        rdo.IdDonViQuyDoi = item;
                        rdo.TenantId = AbpSession.TenantId ?? 1;
                        rdo.CreationTime = DateTime.Now;
                        rdo.CreatorUserId = AbpSession.UserId;
                        rdo.LastModificationTime = DateTime.Now;
                        rdo.LastModifierUserId = AbpSession.UserId;
                        rdo.IsDeleted = false;
                        lstDichVuNhanVien.Add(rdo);
                    }
                }
                await _repository.InsertRangeAsync(lstDichVuNhanVien);
                result.Message = "Thêm mới thành công!";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = "error";
            }
            return result;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Delete)]
        public async Task<ExecuteResultDto> DeleteAsync(EntityDto<Guid> input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                await _repository.DeleteAsync(input.Id);
                result.Message = "Xóa dữ liệu thành công!";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = "error";
            }
            return result;
        }
        [HttpPost]
        public async Task<PagedResultDto<DichVuNhanVienDto>> GetAllAsync(PagedDichVuNhanVienResultRequestDto input)
        {
            PagedResultDto<DichVuNhanVienDto> result = new PagedResultDto<DichVuNhanVienDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var lstData = _repository.GetAllList().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToList();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var items = ObjectMapper.Map<List<DichVuNhanVienDto>>(lstData);
            result.Items = items;
            return result;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Edit)]
        public async Task<ExecuteResultDto> UpdateAsync(CreateOrUpdateDichVuNhanVienDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                var find = await _repository.GetAsync(input.Id);
                find.IdNhanVien = input.IdNhanVien;
                find.IdDonViQuyDoi = input.IdHangHoa;
                find.LastModificationTime = DateTime.Now;
                find.LastModifierUserId = AbpSession.UserId;
                await _repository.UpdateAsync(find);
                result.Message = "Cập nhật dữ liệu thành công!";
                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = "error";
            }
            return result;
        }
        [HttpGet]
        protected async Task<CreateOrUpdateDichVuNhanVienDto> GetForUpdate(Guid id)
        {
            var find = _repository.FirstOrDefault(x => x.Id == id);
            if (find != null)
            {
                return ObjectMapper.Map<CreateOrUpdateDichVuNhanVienDto>(find);
            }
            return new CreateOrUpdateDichVuNhanVienDto();
        }
    }
}
