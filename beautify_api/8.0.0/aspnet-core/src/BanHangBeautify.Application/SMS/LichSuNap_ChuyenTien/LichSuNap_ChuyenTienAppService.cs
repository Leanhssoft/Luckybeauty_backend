using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DatLichOnline.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.LichSuNap_ChuyenTien.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.SMS.LichSuNap_ChuyenTien
{
    public class LichSuNap_ChuyenTienAppService : SPAAppServiceBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<SMS_LichSuNap_ChuyenTien, Guid> _lichSuNapTien;
        private readonly IRepository<HeThong_SMS, Guid> _hethongSMS;
        private readonly ILichSuNap_ChuyenTienRepository _lichSuNapTienRepository;

        public LichSuNap_ChuyenTienAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<SMS_LichSuNap_ChuyenTien, Guid> lichSuNapTien, IRepository<HeThong_SMS, Guid> hethongSMS,
           ILichSuNap_ChuyenTienRepository lichSuNapTienRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _lichSuNapTien = lichSuNapTien;
            _hethongSMS = hethongSMS;
            _lichSuNapTienRepository = lichSuNapTienRepository;
        }
        [HttpGet]
        public async Task<LichSuNap_ChuyenTienDto> GetNhatKyChuyenTien_byId(Guid id)
        {
            SMS_LichSuNap_ChuyenTien data = await _lichSuNapTien.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<LichSuNap_ChuyenTienDto>(data);
            }
            return null;
        }

        [HttpPost]
        public async Task<LichSuNap_ChuyenTienDto> ThemMoiPhieuNapTien(int tenantId, LichSuNap_ChuyenTienDto input)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant != null)
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var adminUserIds = await UserManager.Users.Where(us => us.IsAdmin == true).Select(us => us.Id).ToListAsync();
                    SMS_LichSuNap_ChuyenTien objNew = ObjectMapper.Map<SMS_LichSuNap_ChuyenTien>(input);
                    objNew.Id = Guid.NewGuid(); ;
                    objNew.TenantId = tenantId;
                    objNew.IdNguoiNhanTien = adminUserIds != null ? adminUserIds.FirstOrDefault() : 0;// Nếu nạp tiền: lưu IdTaiKhoan Admin
                    objNew.CreatorUserId = AbpSession.UserId;
                    objNew.CreationTime = DateTime.Now;
                    await _lichSuNapTien.InsertAsync(objNew);
                    var result = ObjectMapper.Map<LichSuNap_ChuyenTienDto>(objNew);
                    return result;
                }
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<LichSuNap_ChuyenTienDto> ThemMoiPhieuChuyenTien(LichSuNap_ChuyenTienDto input)
        {
            SMS_LichSuNap_ChuyenTien objNew = ObjectMapper.Map<SMS_LichSuNap_ChuyenTien>(input);
            objNew.Id = Guid.NewGuid(); ;
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            await _lichSuNapTien.InsertAsync(objNew);
            var result = ObjectMapper.Map<LichSuNap_ChuyenTienDto>(objNew);
            return result;

        }
        [HttpPost]
        public async Task<bool> CapNhatPhieuChuyenTien(LichSuNap_ChuyenTienDto input)
        {
            SMS_LichSuNap_ChuyenTien objUpdate = await _lichSuNapTien.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (objUpdate != null)
            {
                objUpdate.IdNguoiChuyenTien = input.IdNguoiChuyenTien;
                objUpdate.IdNguoiNhanTien = input.IdNguoiNhanTien;
                objUpdate.SoTienChuyen_Nhan = input.SoTienChuyen_Nhan;
                objUpdate.NoiDungChuyen_Nhan = input.NoiDungChuyen_Nhan;
                objUpdate.LastModifierUserId = AbpSession.UserId;
                objUpdate.LastModificationTime = DateTime.Now;
                await _lichSuNapTien.UpdateAsync(objUpdate);
                return false;
            }
            return true;
        }

        [HttpPost]
        public async Task<LichSuNap_ChuyenTienDto> ThemMoi_CapNhatPhieuNapTien(int tenantId, LichSuNap_ChuyenTienDto input)
        {
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId);
            if (tenant != null)
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var lst = _lichSuNapTien.GetAllList(x => x.IdPhieuNapTien == input.IdPhieuNapTien).ToList();
                    if (lst != null && lst.Count > 0)
                    {
                        SMS_LichSuNap_ChuyenTien objUpdate = await _lichSuNapTien.GetAsync(lst.FirstOrDefault().Id);
                        objUpdate.TenantId = tenantId;
                        objUpdate.ThoiGianNap_ChuyenTien = input.ThoiGianNap_ChuyenTien;
                        objUpdate.IdNguoiChuyenTien = input.IdNguoiChuyenTien;
                        objUpdate.IdNguoiNhanTien = input.IdNguoiNhanTien;
                        objUpdate.SoTienChuyen_Nhan = input.SoTienChuyen_Nhan;
                        objUpdate.NoiDungChuyen_Nhan = input.NoiDungChuyen_Nhan;
                        objUpdate.LastModificationTime = DateTime.Now;
                        await _lichSuNapTien.UpdateAsync(objUpdate);
                        var result = ObjectMapper.Map<LichSuNap_ChuyenTienDto>(objUpdate);
                        return result;
                    }
                    else
                    {
                        // neu chua co: insert (do mấy data test ban đầu chưa lưu lịch sử nạp tiền)
                        return await ThemMoiPhieuNapTien(tenantId, input);
                    }
                }
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<bool> XoaLichSuNapTien_byId(Guid id)
        {
            var data = await _lichSuNapTien.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _lichSuNapTien.UpdateAsync(data);
                return true;
            }
            return true;
        }
        [HttpGet]
        public async Task<PagedResultDto<PageNhatKyChuyenTienDto>> GetAllNhatKyChuyenTien(ParamSearch param)
        {
            return await _lichSuNapTienRepository.GetAllNhatKyChuyenTien(param);
        }


        [HttpGet]
        public async Task<double?> GetBrandnameBalance_byUserLogin(long userId)
        {
            try
            {
                // tongnap theo user
                var lstNapTien = await _lichSuNapTien.GetAllListAsync(x => x.IdNguoiNhanTien == userId);
                double? tongNap = lstNapTien.Sum(x => x.SoTienChuyen_Nhan);

                // tonggui
                var lstGuiTien_otherUser = await _lichSuNapTien.GetAllListAsync(x => x.IdNguoiChuyenTien == userId);
                double? tongGui = lstGuiTien_otherUser.Sum(x => x.SoTienChuyen_Nhan);

                // tonggui SMS thanh cong
                var lstGuiTinSMS = await _hethongSMS.GetAllListAsync(x => x.IdNguoiGui == userId && x.TrangThai == 100);
                double? tongGuiSMS = lstGuiTinSMS.Sum(x => x.SoTinGui * x.GiaTienMoiTinNhan);

                double? sodu = tongNap - tongGui - tongGuiSMS;
                return sodu;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
