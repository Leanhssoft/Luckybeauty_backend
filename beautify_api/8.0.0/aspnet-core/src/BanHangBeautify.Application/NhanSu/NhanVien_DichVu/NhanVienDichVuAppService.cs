using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien_DichVu.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        IRepository<NS_NhanVien, Guid> _nhanSuRepository;
        IRepository<NS_ChucVu, Guid> _chucVuRepository;
        IRepository<DM_DonViQuiDoi, Guid> _donViQuyDoiRepository;
        IRepository<DM_HangHoa, Guid> _hangHoaRepository;
        public NhanVienDichVuAppService(
            IRepository<DichVu_NhanVien, Guid> repository,
            IRepository<NS_NhanVien, Guid> nhanSuRepository,
            IRepository<NS_ChucVu, Guid> chucVuRepository,
            IRepository<DM_DonViQuiDoi, Guid> donViQuyDoiRepository,
            IRepository<DM_HangHoa, Guid> hangHoaRepository)
        {
            _repository = repository;
            _nhanSuRepository = nhanSuRepository;
            _chucVuRepository = chucVuRepository;
            _donViQuyDoiRepository = donViQuyDoiRepository;
            _hangHoaRepository = hangHoaRepository;
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
        public async Task<ExecuteResultDto> CreateOrUpdateEmployeeByService(CreateManyEmployeeDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                List<DichVu_NhanVien> lstDichVuNhanVien = new List<DichVu_NhanVien>();
                var checkExist = _repository.GetAll().Where(x => x.IdDonViQuyDoi == input.IdDonViQuiDoi).ToList();
                if (checkExist != null && checkExist.Count > 0)
                {
                    foreach (var item in input.IdNhanViens)
                    {
                        if (!checkExist.Select(x => x.IdNhanVien).ToList().Contains(item))
                        {
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
                            await _repository.InsertAsync(rdo);
                        }
                        else
                        {
                            var dvnv = checkExist.Where(x => x.IdDonViQuyDoi == item).FirstOrDefault();
                            if (dvnv.IsDeleted == true)
                            {
                                dvnv.IsDeleted = false;
                                dvnv.DeleterUserId = null;
                                dvnv.DeletionTime = null;
                                await _repository.UpdateAsync(dvnv);
                            }
                        }
                    }

                    result.Message = "Cập nhật thành công!";
                }
                else
                {
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
                }

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
        public async Task<ExecuteResultDto> CreateOrUpdateServicesByEmployee(CreateServiceManyDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                List<DichVu_NhanVien> lstDichVuNhanVien = new List<DichVu_NhanVien>();
                var checkExist = _repository.GetAll().Where(x => x.IdNhanVien == input.IdNhanVien).ToList();
                if (checkExist != null && checkExist.Count > 0)
                {
                    foreach (var item in input.IdDonViQuiDois)
                    {
                        if (!checkExist.Select(x => x.IdDonViQuyDoi).ToList().Contains(item))
                        {
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
                            await _repository.InsertAsync(rdo);
                        }
                        
                    }
                    foreach (var item in checkExist)
                    {
                        if (input.IdDonViQuiDois.Contains(item.IdDonViQuyDoi)==false)
                        {
                            item.IsDeleted = true;
                            item.DeletionTime = DateTime.Now;
                            item.DeleterUserId = AbpSession.UserId;
                            await _repository.UpdateAsync(item);
                        }
                        
                    }
                    result.Message = "Cập nhật thành công!";
                }
                else
                {
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
                }

                result.Status = "success";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = "error";
            }
            return result;
        }

        public async Task<List<SuggestDichVuBookingDto>> GetAllDichVu(Guid? idNhomDichVu)
        {
            List<SuggestDichVuBookingDto> result = new List<SuggestDichVuBookingDto>();
            var lst = await _donViQuyDoiRepository
                .GetAll()
                .Include(x => x.DM_HangHoa)
                .Where(
                    x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false &&
                    x.DM_HangHoa.IdLoaiHangHoa != LoaiHangHoaConst.HangHoa
                ).ToListAsync();
            if (idNhomDichVu.HasValue)
            {
                lst = lst.Where(x => x.DM_HangHoa.IdNhomHangHoa == idNhomDichVu).ToList();
            }

            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestDichVuBookingDto rdo = new SuggestDichVuBookingDto();
                    rdo.Id = item.Id;
                    rdo.TenDichVu = item.DM_HangHoa.TenHangHoa;
                    rdo.DonGia = decimal.Parse(item.GiaBan.ToString() ?? "0");
                    rdo.Image = item.DM_HangHoa.Image;
                    rdo.SoPhutThucHien = item.DM_HangHoa.SoPhutThucHien.HasValue ? float.Parse(item.DM_HangHoa.SoPhutThucHien.ToString()) : 0;
                    result.Add(rdo);
                }
            }
            return result;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu)]
        public async Task<NhanVienDichVuDetailDto> GetDetail(Guid idNhanVien)
        {
            NhanVienDichVuDetailDto result = new NhanVienDichVuDetailDto();
            var data = _nhanSuRepository.GetAll().Where(x => x.Id == idNhanVien && x.IsDeleted == false).ToList();
            if (data != null && data.Count > 0)
            {
                var nhanVien = await _nhanSuRepository.GetAllIncluding().Where(x => x.Id == idNhanVien).Include(x => x.NS_ChucVu).FirstOrDefaultAsync();
                if (nhanVien != null)
                {
                    var chucVu = _chucVuRepository.FirstOrDefault(x => x.Id == nhanVien.IdChucVu);
                    var dichVuNhanViens = _repository.GetAll().Where(x => x.IdNhanVien == idNhanVien).ToList();
                    result.ChucVu = nhanVien.NS_ChucVu != null ? chucVu.TenChucVu : "";
                    result.TenNhanVien = nhanVien.TenNhanVien;
                    result.SoDienThoai = nhanVien.SoDienThoai;
                    result.Avatar = nhanVien.Avatar;
                    result.Email = "";
                    result.Rate = 0;
                    var idDichVus = dichVuNhanViens.Select(x => x.IdDonViQuyDoi).ToList();
                    var dichVus = _donViQuyDoiRepository.GetAllIncluding().Where(x => idDichVus.Contains(x.Id)).ToList();
                    foreach (var item in dichVus)
                    {
                        DichVuNhanTheoNhanVienDto service = new DichVuNhanTheoNhanVienDto();
                        var dichVu = _hangHoaRepository.FirstOrDefault(x => x.Id == item.IdHangHoa);
                        service.IdDichVu = item.Id;
                        service.Image = dichVu.Image;
                        service.SoPhutThucHien = dichVu.SoPhutThucHien.ToString();
                        service.TenDichVu = dichVu.TenHangHoa;
                        service.DonGia = (decimal)item.GiaBan;
                        result.DichVuThucHiens.Add(service);
                    }
                }
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
