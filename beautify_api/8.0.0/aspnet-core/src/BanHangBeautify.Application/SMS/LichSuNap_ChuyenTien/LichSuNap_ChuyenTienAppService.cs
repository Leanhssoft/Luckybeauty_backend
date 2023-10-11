using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DatLichOnline.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.SMS.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.LichSuNap_ChuyenTien
{
    public class LichSuNap_ChuyenTienAppService : SPAAppServiceBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<SMS_LichSuNap_ChuyenTien, Guid> _lichSuNapTien;

        public LichSuNap_ChuyenTienAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<SMS_LichSuNap_ChuyenTien, Guid> lichSuNapTien)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _lichSuNapTien = lichSuNapTien;
        }

        public async Task<LichSuNap_ChuyenTienDto> ThemMoiPhieuNapTien(int tenantId, LichSuNap_ChuyenTienDto input)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant != null)
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    //await CurrentUnitOfWork.SaveChangesAsync();
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
    }
}
