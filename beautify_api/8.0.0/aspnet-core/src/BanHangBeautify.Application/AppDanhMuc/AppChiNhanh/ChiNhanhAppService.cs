using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using BanHangBeautify.AppDanhMuc.AppChiNhanh.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.PhongBan.Dto;
using BanHangBeautify.Suggests.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppChiNhanh
{
    [AbpAuthorize(PermissionNames.Pages_ChiNhanh)]
    public class ChiNhanhAppService : SPAAppServiceBase
    {
        public readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;
        public readonly IRepository<User, long> _userRepository;
        private readonly IRepository<NS_NhanVien, Guid> _nhanSuRepository;
        private readonly IRepository<NS_QuaTrinh_CongTac, Guid> _quaTrinhCongTacRepository;
        public ChiNhanhAppService(IRepository<DM_ChiNhanh, Guid> chiNhanhRepository,IRepository<User, long> userRepository,
            IRepository<NS_NhanVien,Guid> nhanSuRepository,
            IRepository<NS_QuaTrinh_CongTac,Guid> quaTrinhCongTacRepository)
        {
            _chiNhanhRepository = chiNhanhRepository;
            _userRepository = userRepository;
            _nhanSuRepository = nhanSuRepository;
            _quaTrinhCongTacRepository = quaTrinhCongTacRepository;
        }
        [HttpGet]
        public async Task<ListResultDto<ChiNhanhDto>> GetAllChiNhanh(PagedResultRequestDto input, string keyWord)
        {
            ListResultDto<ChiNhanhDto> result = new ListResultDto<ChiNhanhDto>();
            var chiNhanhs = await _chiNhanhRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(keyWord))
            {
                chiNhanhs = chiNhanhs.Where(x => x.MaSoThue.Contains(keyWord) || x.TenChiNhanh.Contains(keyWord) || x.DiaChi.Contains(keyWord) || x.SoDienThoai.Contains(keyWord)).ToList();
            }
            input.MaxResultCount = 10;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount -1) * input.MaxResultCount : 0;
            chiNhanhs = chiNhanhs.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<ChiNhanhDto>>(chiNhanhs);
            return result;
        }
        public async Task<DM_ChiNhanh> GetChiNhanh(Guid id)
        {
            return await _chiNhanhRepository.GetAsync(id);
        }
        public async Task<List<SuggestChiNhanh>> GetByUserId(long userId)
        {
            List<SuggestChiNhanh> result = new List<SuggestChiNhanh>();
            var user =await _userRepository.FirstOrDefaultAsync(x => x.Id == userId && x.TenantId==AbpSession.TenantId);
            var nhanSu =await  _nhanSuRepository.FirstOrDefaultAsync(x => x.Id == user.NhanSuId && x.TenantId == (AbpSession.TenantId??1));
            if (nhanSu==null)
            {
                var chiNhanh = _chiNhanhRepository.GetAll().Where(x => x.IsDeleted == false).ToList();
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
                var chiNhanh = _chiNhanhRepository.FirstOrDefault(x => x.Id == idChiNhanh);
                result.Add(new SuggestChiNhanh()
                {
                    Id = chiNhanh.Id,
                    TenChiNhanh = chiNhanh.TenChiNhanh
                });
            }
            return result;

        }
        public async Task<CreateChiNhanhDto> GetForEdit(Guid id)
        {
            var data = await _chiNhanhRepository.GetAsync(id);
            if (data!=null)
            {
                return ObjectMapper.Map<CreateChiNhanhDto>(data);
            }
            return new CreateChiNhanhDto();
        }
        [HttpPost]
        public async Task<ChiNhanhDto> CreateOrEditChiNhanh(CreateChiNhanhDto dto)
        {
            var exits = await _chiNhanhRepository.FirstOrDefaultAsync(dto.Id);
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
            var chiNhanhCount = _chiNhanhRepository.GetAll().Where(x=>x.TenantId==(AbpSession.TenantId??1)&&x.IdCongTy==dto.IdCongTy).Count() + 1;
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
            await _chiNhanhRepository.InsertAsync(chiNhanh);
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
            await _chiNhanhRepository.UpdateAsync(chiNhanh);
            return result;
        }
        [HttpPost]
        public async Task<bool> DeleteChiNhanh(Guid Id)
        {
            bool result = false;
            var findBranch = await _chiNhanhRepository.FirstOrDefaultAsync(x => x.Id == Id);
            if (findBranch != null)
            {
                findBranch.IsDeleted = true;
                findBranch.DeleterUserId = AbpSession.UserId;
                findBranch.DeletionTime = DateTime.Now;
                _chiNhanhRepository.Update(findBranch);
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
                    var lst = await _chiNhanhRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x=>x.CreationTime).ToListAsync();
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
                    var chiNhanh =await _chiNhanhRepository.FirstOrDefaultAsync(x=>x.Id==qtct.IdChiNhanh);
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
