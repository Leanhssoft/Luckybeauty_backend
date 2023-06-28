using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Repository;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.PhongBan.Dto;
using BanHangBeautify.Suggests.Dto;
using Microsoft.AspNetCore.Authorization;
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
        public ChiNhanhAppService(IRepository<DM_ChiNhanh, Guid> chiNhanhService,IRepository<User, long> userRepository,
            IRepository<NS_NhanVien,Guid> nhanSuRepository,
            IRepository<NS_QuaTrinh_CongTac,Guid> quaTrinhCongTacRepository,
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
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount -1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chiNhanhReponsitory.GetAll(input,AbpSession.TenantId??1);
        }
        public async Task<DM_ChiNhanh> GetChiNhanh(Guid id)
        {
            return await _chiNhanhService.GetAsync(id);
        }
        public async Task<List<SuggestChiNhanh>> GetByUserId(long userId)
        {
            List<SuggestChiNhanh> result = new List<SuggestChiNhanh>();
            var user =await _userRepository.FirstOrDefaultAsync(x => x.Id == userId && x.TenantId==AbpSession.TenantId);
            var nhanSu =await  _nhanSuRepository.FirstOrDefaultAsync(x => x.Id == user.NhanSuId && x.TenantId == (AbpSession.TenantId??1));
            if (nhanSu==null)
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
                var idChiNhanh = _quaTrinhCongTacRepository.GetAll().Where(x=>x.IsDeleted==false&& x.TenantId==(AbpSession.TenantId??1)).OrderByDescending(x=>x.CreationTime).FirstOrDefault(x => x.IdNhanVien == nhanSu.Id).IdChiNhanh;
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
            if (data!=null)
            {
                return ObjectMapper.Map<CreateChiNhanhDto>(data);
            }
            return new CreateChiNhanhDto();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChiNhanh_Edit,PermissionNames.Pages_ChiNhanh_Create)]
        public async Task<ChiNhanhDto> CreateOrEditChiNhanh(CreateChiNhanhDto dto)
        {
            var exits = await _chiNhanhService.FirstOrDefaultAsync(dto.Id);
            if (exits == null)
            {
                return await Create(dto);
            }
            else return await Edit(dto, exits);
        }
        [NonAction]
        public async Task<ChiNhanhDto> Create(CreateChiNhanhDto dto)
        {
            DM_ChiNhanh chiNhanh = new DM_ChiNhanh();
            chiNhanh.Id = Guid.NewGuid();
            var chiNhanhCount = _chiNhanhService.GetAll().Where(x=>x.TenantId==(AbpSession.TenantId??1)&&x.IdCongTy==dto.IdCongTy).Count() + 1;
            chiNhanh.MaChiNhanh =string.IsNullOrEmpty(dto.MaChiNhanh)? "CN_0" + chiNhanhCount.ToString(): dto.MaChiNhanh;
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
            chiNhanh.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<ChiNhanhDto>(chiNhanh);
            await _chiNhanhService.InsertAsync(chiNhanh);
            return result;
            //return ObjectMapper.Map<ChiNhanhDto>(dto);
        }
        [NonAction]
        public async Task<ChiNhanhDto> Edit(CreateChiNhanhDto dto, DM_ChiNhanh chiNhanh)
        {
            chiNhanh.TenChiNhanh = dto.TenChiNhanh;
            chiNhanh.MaSoThue = dto.MaSoThue;
            chiNhanh.DiaChi = dto.DiaChi;
            chiNhanh.GhiChu = dto.GhiChu;
            chiNhanh.Logo = dto.Logo;
            chiNhanh.NgayApDung = dto.NgayApDung;
            chiNhanh.NgayHetHan = dto.NgayHetHan;
            chiNhanh.TenantId = AbpSession.TenantId ?? 1;
            chiNhanh.LastModifierUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<ChiNhanhDto>(chiNhanh);
            await _chiNhanhService.UpdateAsync(chiNhanh);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChiNhanh_Delete)]
        public async Task<bool> DeleteChiNhanh(Guid Id)
        {
            bool result = false;
            var findBranch = await _chiNhanhService.FirstOrDefaultAsync(x => x.Id == Id);
            if (findBranch != null)
            {
                findBranch.IsDeleted = true;
                findBranch.DeleterUserId = AbpSession.UserId;
                findBranch.DeletionTime = DateTime.Now;
                _chiNhanhService.Update(findBranch);
                result = true;
            }
            return result;
        }
        public async Task<List<SuggestChiNhanh>> GetChiNhanhByUser()
        {
            List<SuggestChiNhanh> result = new List<SuggestChiNhanh>();
            var user =await _userRepository.FirstOrDefaultAsync(x=>x.Id==AbpSession.UserId&&x.TenantId==AbpSession.TenantId);
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    var lst = await _chiNhanhService.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x=>x.CreationTime).ToListAsync();
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
                        Where(x=>x.NS_NhanVien.Id==user.NhanSuId &&
                            x.IsDeleted==false && x.TenantId == (AbpSession.TenantId??1)
                            ).
                        OrderByDescending(x => x.CreationTime).Take(1).ToList().FirstOrDefault();
                    var chiNhanh =await _chiNhanhService.FirstOrDefaultAsync(x=>x.Id==qtct.IdChiNhanh);
                    if (chiNhanh!=null)
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
