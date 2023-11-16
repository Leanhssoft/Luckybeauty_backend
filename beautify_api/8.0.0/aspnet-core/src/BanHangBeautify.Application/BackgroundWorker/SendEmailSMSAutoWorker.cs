using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using BanHangBeautify.SMS.ESMS;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Configuration.Common.Consts;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.Authorization.Users;

namespace BanHangBeautify.BackgroundWorker
{
    public class SendEmailSMSAutoWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public readonly IESMS _eSMS;
        public readonly IRepository<SMS_CaiDat_NhacNho, Guid> _caiDatNhacNho;
        public readonly IRepository<DM_ChiNhanh, Guid> _dmChiNhanh;
        public readonly IRepository<HeThong_SMS, Guid> _hethongSMS;
        public readonly IRepository<SMS_NhatKy_GuiTin, Guid> _smsNhatKyGuiTin;
        public readonly IRepository<User, long> _user;
        private readonly IRepository<Tenant> _tenantRepository;
        public readonly IHeThongSMSRepository _repoSMS;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        readonly DateTime _dtNow = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

        public SendEmailSMSAutoWorker(AbpTimer timer,
            IESMS eSMS,
            IRepository<SMS_CaiDat_NhacNho, Guid> caiDatNhacNho,
            IRepository<DM_ChiNhanh, Guid> dmChiNhanh,
           IRepository<HeThong_SMS, Guid> hethongSMS,
             IRepository<SMS_NhatKy_GuiTin, Guid> smsNhatKyGuiTin,
             IRepository<User, long> user,
             IRepository<Tenant> tenantRepository,
            IHeThongSMSRepository repoSMS,
            IUnitOfWorkManager unitOfWorkManager
            ) : base(timer)
        {
            Timer.Period = 10000;
            _eSMS = eSMS;
            _caiDatNhacNho = caiDatNhacNho;
            _dmChiNhanh = dmChiNhanh;
            _hethongSMS = hethongSMS;
            _smsNhatKyGuiTin = smsNhatKyGuiTin;
            _user = user;
            _tenantRepository = tenantRepository;
            _repoSMS = repoSMS;
            _unitOfWorkManager = unitOfWorkManager;
        }

        protected async override void DoWork()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (int i = 0; i < tenants.Count; i++)
            {
                var tenantId = tenants[i].Id;

                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    //    // your code
                    await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    // get allchinhanh of tenantId
                    var chiNhanh = _dmChiNhanh.GetAll().Select(x => x.Id.ToString()).ToList();
                    var userAdmin = _user.GetAllList(x => x.IsAdmin).Select(x => x.Id).FirstOrDefault();
                    var lstSetup = _caiDatNhacNho.GetAllList(x => x.TrangThai == 1 &&
                       (x.IdLoaiTin == ConstSMS.LoaiTin.SinhNhat
                       || x.IdLoaiTin == ConstSMS.LoaiTin.GiaoDich
                       || x.IdLoaiTin == ConstSMS.LoaiTin.LichHen)
                       ).Select(x => new { x.IdLoaiTin, x.NoiDungTin });
                    var xx = _unitOfWorkManager.Current.GetTenantId();

                    Guid? idChiNhanhFirst = null;
                    if (chiNhanh != null && chiNhanh.Count > 0)
                    {
                        idChiNhanhFirst = new Guid(chiNhanh.FirstOrDefault().ToString());
                    }

                    CommonClass.ParamSearch param = new()
                    {
                        FromDate = _dtNow,
                        ToDate = _dtNow,
                        IdChiNhanhs = chiNhanh,
                        TrangThais = new List<string> { "0" }, // chi get tin chua gui (trangthai = 0)
                        CurrentPage = 1,
                        PageSize = 10000,
                    };
                    if (lstSetup.Any())
                    {
                        var lstLoaiTin = lstSetup.Select(x => x.IdLoaiTin);
                        ////// todo check CaiDat_NhacTuDong_ChiTiet: HinhThucGui(sms / zalo / gmail)
                        if (lstLoaiTin.Contains(ConstSMS.LoaiTin.SinhNhat))
                        {
                            string noidungTinMau = lstSetup.Where(x => x.IdLoaiTin == ConstSMS.LoaiTin.SinhNhat).Select(x => x.NoiDungTin)?.FirstOrDefault()?.ToString();
                            await SaveData(tenantId, idChiNhanhFirst, ConstSMS.LoaiTin.SinhNhat, param, noidungTinMau, userAdmin);
                        }
                        if (lstLoaiTin.Contains(ConstSMS.LoaiTin.LichHen))
                        {
                            string noidungTinMau = lstSetup.Where(x => x.IdLoaiTin == ConstSMS.LoaiTin.LichHen).Select(x => x.NoiDungTin)?.FirstOrDefault()?.ToString();
                            await SaveData(tenantId, idChiNhanhFirst, ConstSMS.LoaiTin.LichHen, param, noidungTinMau, userAdmin);
                        }
                        if (lstLoaiTin.Contains(ConstSMS.LoaiTin.GiaoDich))
                        {
                            string noidungTinMau = lstSetup.Where(x => x.IdLoaiTin == ConstSMS.LoaiTin.GiaoDich).Select(x => x.NoiDungTin)?.FirstOrDefault()?.ToString();
                            await SaveData(tenantId, idChiNhanhFirst, ConstSMS.LoaiTin.GiaoDich, param, noidungTinMau, userAdmin);
                        }
                    }
                });
                }
            }
            unitOfWork.Complete();
        }

        protected string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin)
        {
            var ss = noiDungTin.Replace("{TenKhachHang}", cutomer.TenKhachHang);
            _ = noiDungTin.Replace("{NgaySinh}", cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            _ = noiDungTin.Replace("{BookingDate}", cutomer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            _ = noiDungTin.Replace("{ThoiGianHen}", cutomer.ThoiGianHen);
            _ = noiDungTin.Replace("{TenHangHoa}", cutomer.TenHangHoa);// dichvuhen
            _ = noiDungTin.Replace("{MaGiaoDich}", cutomer.MaHoaDon);
            _ = noiDungTin.Replace("{NgayGiaoDich}", cutomer.NgayLapHoaDon?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
            return ss;
        }

        protected async Task SaveData(int tenantId, Guid? idChiNhanhFirst, byte? idLoaiTin, CommonClass.ParamSearch param, string noidungTinMau, long? userId)
        {
            try
            {
                var data = await _repoSMS.GetListCustomer_byIdLoaiTin(param, idLoaiTin);
                if (data.TotalCount > 0)
                {
                    var lstCustomer = data.Items;

                    foreach (var customer in lstCustomer)
                    {
                        #region Bitrhday - send SMS
                        var noidung = ReplaceContent(customer, noidungTinMau);
                        ESMSDto obj = new()
                        {
                            Phone = customer.SoDienThoai,
                            Brandname = "brandname",// todo brandname
                            Content = noidung,
                        };
                        // send SMS
                        var smsResult = await _eSMS.SendSMS_Json(obj);

                        // save to hethong sms
                        HeThong_SMS hethongSMS = new()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = tenantId,
                            IdLoaiTin = idLoaiTin ?? 0,
                            IdChiNhanh = (Guid)idChiNhanhFirst,
                            IdKhachHang = customer.IdKhachHang,
                            SoDienThoai = customer.SoDienThoai,
                            SoTinGui = (int)Math.Ceiling(noidung.Length / 160.0),
                            IdTinNhan = smsResult.MessageId,
                            GiaTienMoiTinNhan = 950,
                            TrangThai = smsResult.MessageStatus,
                            NoiDungTin = noidung,
                            IdNguoiGui = userId,
                            CreatorUserId = userId,
                            ThoiGianGui = DateTime.Now,
                            CreationTime = DateTime.Now,
                            IsDeleted = false,
                        };
                        await _hethongSMS.InsertAsync(hethongSMS);

                        SMS_NhatKy_GuiTin nkyGuiSMS = new()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = tenantId,
                            IdHeThongSMS = hethongSMS.Id,
                            ThoiGianTu = _dtNow,
                            ThoiGianDen = _dtNow,
                            IdBooking = idLoaiTin == ConstSMS.LoaiTin.LichHen ? customer.Id : null,
                            IdHoaDon = idLoaiTin == ConstSMS.LoaiTin.GiaoDich ? customer.Id : null,
                            CreationTime = DateTime.Now,
                            CreatorUserId = userId,
                            IsDeleted = false,
                        };
                        await _smsNhatKyGuiTin.InsertAsync(nkyGuiSMS);
                        #endregion

                        #region birthday - send email
                        // todo
                        #endregion
                        CurrentUnitOfWork.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
