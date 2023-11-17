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
using BanHangBeautify.SMS.Brandname.Repository;
using Abp.Net.Mail;
using BanHangBeautify.Configuration.Dto;
using Abp.Configuration;
using NPOI.XWPF.UserModel;

namespace BanHangBeautify.BackgroundWorker
{
    public class SendEmailSMSAutoWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public readonly IESMS _eSMS;
        public readonly IRepository<SMS_CaiDat_NhacNho, Guid> _caiDatNhacNho;
        public readonly IRepository<CaiDat_NhacNho_ChiTiet, Guid> _caiDatNhacNhoChiTiet;
        public readonly IRepository<DM_ChiNhanh, Guid> _dmChiNhanh;
        public readonly IRepository<HeThong_SMS, Guid> _hethongSMS;
        public readonly IRepository<SMS_NhatKy_GuiTin, Guid> _smsNhatKyGuiTin;
        public readonly IRepository<User, long> _user;
        private readonly IRepository<Tenant> _tenantRepository;
        public readonly IHeThongSMSRepository _repoSMS;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly SettingManager _settingManager;
        private readonly IEmailSender _emailSender;
        private readonly IBrandnameRepository _repoBrandname;
        readonly DateTime _dtNow = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

        public SendEmailSMSAutoWorker(AbpTimer timer,
            IESMS eSMS,
            IRepository<SMS_CaiDat_NhacNho, Guid> caiDatNhacNho,
            IRepository<CaiDat_NhacNho_ChiTiet, Guid> caiDatNhacNhoChiTiet,
            IRepository<DM_ChiNhanh, Guid> dmChiNhanh,
           IRepository<HeThong_SMS, Guid> hethongSMS,
             IRepository<SMS_NhatKy_GuiTin, Guid> smsNhatKyGuiTin,
             IRepository<User, long> user,
             IRepository<Tenant> tenantRepository,
            IHeThongSMSRepository repoSMS,
            IUnitOfWorkManager unitOfWorkManager,
            SettingManager settingManager,
            IEmailSender emailSender,
            IBrandnameRepository repoBrandname
            ) : base(timer)
        {
            Timer.Period = 10000;
            _eSMS = eSMS;
            _caiDatNhacNho = caiDatNhacNho;
            _caiDatNhacNhoChiTiet = caiDatNhacNhoChiTiet;
            _dmChiNhanh = dmChiNhanh;
            _hethongSMS = hethongSMS;
            _smsNhatKyGuiTin = smsNhatKyGuiTin;
            _user = user;
            _tenantRepository = tenantRepository;
            _repoSMS = repoSMS;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _emailSender = emailSender;
            _repoBrandname = repoBrandname;
        }

        protected async override void DoWork()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            var lstBrandname = await _repoBrandname.GetListBandname(new PagedRequestDto { Keyword = string.Empty, SkipCount = 0 }, 1);  // get all brand name at host

            for (int i = 0; i < tenants.Count; i++)
            {
                var tenantId = tenants[i].Id;

                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    // check has settup
                    var lstSetup = _caiDatNhacNho.GetAllList(x => x.TrangThai == 1 &&
                       (x.IdLoaiTin == ConstSMS.LoaiTin.SinhNhat
                       || x.IdLoaiTin == ConstSMS.LoaiTin.GiaoDich
                       || x.IdLoaiTin == ConstSMS.LoaiTin.LichHen)
                       ).Select(x => new { x.Id, x.IdLoaiTin, x.NoiDungTin });

                    if (lstSetup.Any())
                    {
                        var chiNhanh = _dmChiNhanh.GetAll().Select(x => x.Id.ToString()).ToList();
                        var userAdmin = _user.GetAllList(x => x.IsAdmin).Select(x => x.Id).FirstOrDefault();
                        var lstIdSetup = lstSetup.Select(x => x.Id);
                        var brandName_ofTenant = lstBrandname.Items.Where(x => x.TenantId == tenantId);

                        InforAutoWorker inforCommon = new()
                        {
                            TenantId = tenantId,
                            UserId = userAdmin,
                        };
                        if (chiNhanh != null && chiNhanh.Count > 0)
                        {
                            inforCommon.IdChiNhanhFirst = new Guid(chiNhanh.FirstOrDefault().ToString());
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

                        var lstSetupDetail = _caiDatNhacNhoChiTiet.GetAllList(x => lstIdSetup.Contains(x.IdCaiDatNhacNho) && x.TrangThai == 1)
                        .Select(x => new { x.IdCaiDatNhacNho, x.HinhThucGui });

                        var tblJoin = (from setup in lstSetup
                                       join setupDetail in lstSetupDetail on setup.Id equals setupDetail.IdCaiDatNhacNho
                                       select new
                                       {
                                           setupDetail.IdCaiDatNhacNho,
                                           setupDetail.HinhThucGui,
                                           setup.IdLoaiTin,
                                           setup.NoiDungTin
                                       }).ToList();

                        if (brandName_ofTenant.Any())
                        {
                            inforCommon.BrandNameFirst = brandName_ofTenant.FirstOrDefault().Brandname;

                            var turnOnSMS = tblJoin.Where(x => x.HinhThucGui == 1).Select(x => x);
                            foreach (var item in turnOnSMS)
                            {
                                if (!string.IsNullOrEmpty(item.NoiDungTin))
                                {
                                    inforCommon.NoiDungTinNhan = item.NoiDungTin;
                                    await SendSMS(item.IdLoaiTin, param, inforCommon);
                                }
                            }
                        }

                        var turnOnZalo = tblJoin.Where(x => x.HinhThucGui == 3).Select(x => x);
                        foreach (var item in turnOnZalo)
                        {
                            inforCommon.NoiDungTinNhan = item.NoiDungTin;
                            await SendSMS(item.IdLoaiTin, param, inforCommon);
                        }

                        var turnOnEmail = tblJoin.Where(x => x.HinhThucGui == 2).Select(x => x);
                        foreach (var item in turnOnEmail)
                        {
                            if (!string.IsNullOrEmpty(item.NoiDungTin))
                            {
                                inforCommon.NoiDungTinNhan = item.NoiDungTin;
                                await SendEmail(item.IdLoaiTin, param, inforCommon);
                            }
                        }
                        CurrentUnitOfWork.SaveChanges();
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

        protected async Task SendSMS(byte? idLoaiTin, CommonClass.ParamSearch paramSearch, InforAutoWorker inforCommon)
        {
            try
            {
                var data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTin);
                if (data.TotalCount > 0)
                {
                    var lstCustomer = data.Items;
                    foreach (var customer in lstCustomer)
                    {
                        var noidung = ReplaceContent(customer, inforCommon.NoiDungTinNhan);
                        ESMSDto obj = new()
                        {
                            Phone = customer.SoDienThoai,
                            Brandname = inforCommon.BrandNameFirst,
                            Content = noidung,
                        };
                        // send SMS
                        var smsResult = await _eSMS.SendSMS_Json(obj);

                        // save to hethong sms
                        HeThong_SMS hethongSMS = new()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = inforCommon.TenantId,
                            IdLoaiTin = idLoaiTin ?? 0,
                            IdChiNhanh = (Guid)inforCommon.IdChiNhanhFirst,
                            IdKhachHang = customer.IdKhachHang,
                            SoDienThoai = customer.SoDienThoai,
                            SoTinGui = (int)Math.Ceiling(noidung.Length / 160.0),
                            IdTinNhan = smsResult.MessageId,
                            GiaTienMoiTinNhan = 950,
                            TrangThai = smsResult.MessageStatus,
                            NoiDungTin = noidung,
                            IdNguoiGui = inforCommon.UserId,
                            CreatorUserId = inforCommon.UserId,
                            ThoiGianGui = DateTime.Now,
                            CreationTime = DateTime.Now,
                            IsDeleted = false,
                        };
                        await _hethongSMS.InsertAsync(hethongSMS);

                        SMS_NhatKy_GuiTin nkyGuiSMS = new()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = inforCommon.TenantId,
                            IdHeThongSMS = hethongSMS.Id,
                            ThoiGianTu = _dtNow,
                            ThoiGianDen = _dtNow,
                            IdBooking = idLoaiTin == ConstSMS.LoaiTin.LichHen ? customer.Id : null,
                            IdHoaDon = idLoaiTin == ConstSMS.LoaiTin.GiaoDich ? customer.Id : null,
                            CreationTime = DateTime.Now,
                            CreatorUserId = inforCommon.UserId,
                            IsDeleted = false,
                        };
                        await _smsNhatKyGuiTin.InsertAsync(nkyGuiSMS);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task SendEmail(byte? idLoaiTin, CommonClass.ParamSearch paramSearch, InforAutoWorker inforCommon)
        {
            try
            {
                var data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTin);
                if (data.TotalCount > 0)
                {
                    var lstCustomer = data.Items;
                    foreach (var customer in lstCustomer)
                    {
                        int sendStatus = 99;
                        var noidung = ReplaceContent(customer, inforCommon.NoiDungTinNhan);

                        try
                        {
                            await _emailSender.SendAsync(customer.Email, "Test send email", noidung, true);
                            sendStatus = 100;
                        }
                        catch (Exception ex)
                        {

                        }

                        // save to hethong sms
                        HeThong_SMS hethongSMS = new()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = inforCommon.TenantId,
                            IdLoaiTin = idLoaiTin ?? 0,
                            IdChiNhanh = (Guid)inforCommon.IdChiNhanhFirst,
                            IdKhachHang = customer.IdKhachHang,
                            SoDienThoai = customer.SoDienThoai,
                            SoTinGui = 0,
                            GiaTienMoiTinNhan = 0,
                            TrangThai = sendStatus,
                            NoiDungTin = noidung,
                            IdNguoiGui = inforCommon.UserId,
                            CreatorUserId = inforCommon.UserId,
                            ThoiGianGui = DateTime.Now,
                            CreationTime = DateTime.Now,
                            IsDeleted = false,
                        };
                        await _hethongSMS.InsertAsync(hethongSMS);

                        SMS_NhatKy_GuiTin nkyGuiSMS = new()
                        {
                            Id = Guid.NewGuid(),
                            TenantId = inforCommon.TenantId,
                            IdHeThongSMS = hethongSMS.Id,
                            ThoiGianTu = _dtNow,
                            ThoiGianDen = _dtNow,
                            IdBooking = idLoaiTin == ConstSMS.LoaiTin.LichHen ? customer.Id : null,
                            IdHoaDon = idLoaiTin == ConstSMS.LoaiTin.GiaoDich ? customer.Id : null,
                            CreationTime = DateTime.Now,
                            CreatorUserId = inforCommon.UserId,
                            IsDeleted = false,
                        };
                        await _smsNhatKyGuiTin.InsertAsync(nkyGuiSMS);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class InforAutoWorker
    {
        public int TenantId { get; set; }
        public long? UserId { get; set; } = 1;
        public Guid? IdChiNhanhFirst { get; set; }
        public string BrandNameFirst { get; set; }
        public string NoiDungTinNhan { get; set; }
    }
}
