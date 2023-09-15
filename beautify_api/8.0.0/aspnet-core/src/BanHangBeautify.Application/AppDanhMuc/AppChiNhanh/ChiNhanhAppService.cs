using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Repository;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.Suggests.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh
{
    [AbpAuthorize]
    public class ChiNhanhAppService : SPAAppServiceBase
    {
        public readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhService;
        private readonly IChiNhanhRepository _chiNhanhReponsitory;
        public readonly IRepository<User, long> _userRepository;
        private readonly IRepository<NS_NhanVien, Guid> _nhanSuRepository;
        private readonly IRepository<NS_QuaTrinh_CongTac, Guid> _quaTrinhCongTacRepository;
        public ChiNhanhAppService(IRepository<DM_ChiNhanh, Guid> chiNhanhService, IRepository<User, long> userRepository,
            IRepository<NS_NhanVien, Guid> nhanSuRepository,
            IRepository<NS_QuaTrinh_CongTac, Guid> quaTrinhCongTacRepository,
            IChiNhanhRepository chiNhanhRepository)
        {
            _chiNhanhService = chiNhanhService;
            _userRepository = userRepository;
            _nhanSuRepository = nhanSuRepository;
            _quaTrinhCongTacRepository = quaTrinhCongTacRepository;
            _chiNhanhReponsitory = chiNhanhRepository;
        }
        [HttpGet]
        public async Task<PagedResultDto<ChiNhanhDto>> GetAllChiNhanh(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chiNhanhReponsitory.GetAll(input, AbpSession.TenantId ?? 1);
        }
        public async Task<DM_ChiNhanh> GetChiNhanh(Guid id)
        {
            return await _chiNhanhService.GetAsync(id);
        }
        public async Task<List<SuggestChiNhanh>> GetByUserId(long userId)
        {
            List<SuggestChiNhanh> result = new List<SuggestChiNhanh>();
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId && x.TenantId == AbpSession.TenantId);
            var nhanSu = await _nhanSuRepository.FirstOrDefaultAsync(x => x.Id == user.NhanSuId && x.TenantId == (AbpSession.TenantId ?? 1));
            if (nhanSu == null)
            {
                var chiNhanh = _chiNhanhService.GetAll().Where(x => x.IsDeleted == false).ToList();
                foreach (var item in chiNhanh)
                {
                    SuggestChiNhanh rdo = new SuggestChiNhanh();
                    rdo.Id = item.Id;
                    rdo.TenChiNhanh = item.TenChiNhanh;
                    result.Add(rdo);
                }
            }
            else
            {
                var idChiNhanh = _quaTrinhCongTacRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).FirstOrDefault(x => x.IdNhanVien == nhanSu.Id).IdChiNhanh;
                var chiNhanh = _chiNhanhService.FirstOrDefault(x => x.Id == idChiNhanh);
                result.Add(new SuggestChiNhanh()
                {
                    Id = chiNhanh.Id,
                    TenChiNhanh = chiNhanh.TenChiNhanh
                });
            }
            return result;

        }
        [AbpAuthorize(PermissionNames.Pages_ChiNhanh_Edit)]
        public async Task<CreateChiNhanhDto> GetForEdit(Guid id)
        {
            var data = await _chiNhanhService.GetAsync(id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateChiNhanhDto>(data);
            }
            return new CreateChiNhanhDto();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChiNhanh_Edit, PermissionNames.Pages_ChiNhanh_Create)]
        public async Task<ExecuteResultDto> CreateOrEditChiNhanh(CreateChiNhanhDto dto)
        {
            var exits = await _chiNhanhService.FirstOrDefaultAsync(dto.Id);
            if (exits == null)
            {
                return await Create(dto);
            }
            else return await Edit(dto, exits);
        }
        [NonAction]
        public async Task<ExecuteResultDto> Create(CreateChiNhanhDto dto)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                DM_ChiNhanh chiNhanh = new DM_ChiNhanh();
                var checkTenChiNhanh = await _chiNhanhService.FirstOrDefaultAsync(x => x.TenChiNhanh == dto.TenChiNhanh);
                if (checkTenChiNhanh == null)
                {
                    chiNhanh.Id = Guid.NewGuid();
                    var chiNhanhCount = _chiNhanhService.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IdCongTy == dto.IdCongTy).Count() + 1;
                    chiNhanh.MaChiNhanh = string.IsNullOrEmpty(dto.MaChiNhanh) ? "CN_0" + chiNhanhCount.ToString() : dto.MaChiNhanh;
                    chiNhanh.SoDienThoai = dto.SoDienThoai;
                    chiNhanh.TenChiNhanh = dto.TenChiNhanh;
                    chiNhanh.MaSoThue = dto.MaSoThue;
                    chiNhanh.DiaChi = dto.DiaChi;
                    chiNhanh.GhiChu = dto.GhiChu;
                    chiNhanh.Logo = dto.Logo;
                    chiNhanh.NgayApDung = dto.NgayApDung;
                    chiNhanh.NgayHetHan = dto.NgayHetHan;
                    chiNhanh.TenantId = AbpSession.TenantId ?? 1;
                    chiNhanh.CreatorUserId = AbpSession.UserId;
                    chiNhanh.IdCongTy = dto.IdCongTy;
                    chiNhanh.TrangThai = dto.TrangThai;
                    chiNhanh.CreationTime = DateTime.Now;
                    await _chiNhanhService.InsertAsync(chiNhanh);
                    result.Message = "Thêm mới chi nhánh thành công!";
                    result.Status = "success";
                }
                else
                {
                    result.Message = "Tên chi nhánh đã tồn tại!";
                    result.Status = "error";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Thêm mới chi nhánh thất bại!";
                result.Status = "error";
                result.Detail = ex.Message;
            }
            return result;
        }
        [NonAction]
        public async Task<ExecuteResultDto> Edit(CreateChiNhanhDto dto, DM_ChiNhanh chiNhanh)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                var checkTenChiNhanh = await _chiNhanhService.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (checkTenChiNhanh != null)
                {
                    chiNhanh.TenChiNhanh = dto.TenChiNhanh;
                    chiNhanh.MaSoThue = dto.MaSoThue;
                    chiNhanh.SoDienThoai = dto.SoDienThoai;
                    chiNhanh.DiaChi = dto.DiaChi;
                    chiNhanh.GhiChu = dto.GhiChu;
                    chiNhanh.Logo = dto.Logo;
                    chiNhanh.NgayApDung = dto.NgayApDung;
                    chiNhanh.NgayHetHan = dto.NgayHetHan;
                    chiNhanh.TenantId = AbpSession.TenantId ?? 1;
                    chiNhanh.TrangThai = dto.TrangThai;
                    chiNhanh.LastModifierUserId = AbpSession.UserId;
                    await _chiNhanhService.UpdateAsync(chiNhanh);
                    result.Message = "Cập nhật thông tin chi nhánh thành công!";
                    result.Status = "success";
                }
                else
                {
                    result.Message = "Tên chi nhánh đã tồn tại!";
                    result.Status = "error";
                }

            }
            catch (Exception ex)
            {
                result.Message = "Thêm mới chi nhánh thất bại!";
                result.Status = "error";
                result.Detail = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChiNhanh_Delete)]
        public async Task<ExecuteResultDto> DeleteChiNhanh(Guid Id)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            var findBranch = await _chiNhanhService.FirstOrDefaultAsync(x => x.Id == Id);
            if (findBranch != null)
            {
                findBranch.IsDeleted = true;
                findBranch.DeleterUserId = AbpSession.UserId;
                findBranch.DeletionTime = DateTime.Now;
                _chiNhanhService.Update(findBranch);
                return new ExecuteResultDto()
                {
                    Status = "success",
                    Message = "Xóa chi nhánh thành công!",
                    Detail = ""
                };
            }
            return new ExecuteResultDto()
            {
                Status = "error",
                Message = "Xóa chi nhánh thất bại!",
                Detail = ""
            };
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChiNhanh_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi sảy ra vui lòng thử lại sau!"
            };
            {
                var finds = await _chiNhanhService.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (finds != null && finds.Count > 0)
                {
                    _chiNhanhService.RemoveRange(finds);
                    result.Status = "success";
                    result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                }
                return result;
            }
        }

        public async Task<List<SuggestChiNhanh>> GetChiNhanhByUser()
        {
            List<SuggestChiNhanh> result = new List<SuggestChiNhanh>();
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == AbpSession.UserId && x.TenantId == AbpSession.TenantId);
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    var lst = await _chiNhanhService.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
                    if (lst != null || lst.Count > 0)
                    {
                        foreach (var item in lst)
                        {
                            SuggestChiNhanh rdo = new SuggestChiNhanh();
                            rdo.Id = item.Id;
                            rdo.TenChiNhanh = item.TenChiNhanh;
                            result.Add(rdo);
                        }
                    }
                }
                else
                {
                    var qtct = _quaTrinhCongTacRepository.GetAll().
                        Include(x => x.NS_NhanVien).
                        Where(x => x.NS_NhanVien.Id == user.NhanSuId &&
                            x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)
                            ).
                        OrderByDescending(x => x.CreationTime).Take(1).ToList().FirstOrDefault();
                    var chiNhanh = await _chiNhanhService.FirstOrDefaultAsync(x => x.Id == qtct.IdChiNhanh);
                    if (chiNhanh != null)
                    {
                        result.Add(new SuggestChiNhanh()
                        {
                            Id = chiNhanh.Id,
                            TenChiNhanh = chiNhanh.TenChiNhanh
                        });
                    }
                }
            }
            result.Reverse();
            return result;
        }
    }
}
