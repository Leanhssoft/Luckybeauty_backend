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
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.SMS.Brandname.Repository;
using Abp.Net.Mail;
using BanHangBeautify.Configuration.Dto;
using Abp.Configuration;
using NPOI.XWPF.UserModel;
using Abp.Application.Services.Dto;
using BanHangBeautify.Consts;

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
            var tenants = _tenantRepository.GetAllList();
            var lstBrandname = await _repoBrandname.GetListBandname(new ParamSearchBrandname { Keyword = string.Empty, SkipCount = 0 }, 1);  // get all brand name at host

            for (int i = 0; i < tenants.Count; i++)
            {
                var tenantId = tenants[i].Id;

                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    try // phải try catch: vì khi tạo tenant mới, bảng _caiDatNhacNho chưa dc tạo, background chạy liên tục --> lỗi
                    {
                        // check has settup
                        var lstSetup = _caiDatNhacNho.GetAllList(x => x.TrangThai == 1 &&
                           (x.IdLoaiTin == ConstSMS.LoaiTin.SinhNhat
                           || x.IdLoaiTin == ConstSMS.LoaiTin.GiaoDich
                           || x.IdLoaiTin == ConstSMS.LoaiTin.LichHen)
                           ).Select(x => new { x.Id, x.IdLoaiTin, x.NoiDungTin, x.NhacTruocKhoangThoiGian, x.LoaiThoiGian });

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

                            ParamSearchSMS param = new()
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
                                               setup.NoiDungTin,
                                               setup.NhacTruocKhoangThoiGian,
                                               setup.LoaiThoiGian
                                           }).ToList();

                            if (brandName_ofTenant.Any())
                            {
                                inforCommon.BrandNameFirst = brandName_ofTenant.FirstOrDefault().Brandname;

                                var oclock = DateTime.Now.Hour;
                                var minutes = DateTime.Now.Minute;

                                var turnOnSMS = tblJoin.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.SMS).Select(x => x);
                                foreach (var item in turnOnSMS)
                                {
                                    if (!string.IsNullOrEmpty(item.NoiDungTin))
                                    {
                                        inforCommon.NoiDungTinNhan = item.NoiDungTin;
                                        switch (item.IdLoaiTin)
                                        {
                                            case ConstSMS.LoaiTin.SinhNhat:
                                                {
                                                    if (oclock > 7)
                                                    {
                                                        // tin sinhnhat: chi gui từ 7h sáng
                                                        await SendSMS(item.IdLoaiTin, param, inforCommon);
                                                    }
                                                }
                                                break;
                                            case ConstSMS.LoaiTin.LichHen:
                                                {
                                                    float? totalTime = 0;
                                                    switch (item.LoaiThoiGian)
                                                    {
                                                        case LoaiThoiGian.SECOND:
                                                            totalTime = item.NhacTruocKhoangThoiGian;
                                                            break;
                                                        case LoaiThoiGian.MiNUTE:
                                                            totalTime = item.NhacTruocKhoangThoiGian * 60;
                                                            break;
                                                        case LoaiThoiGian.HOUR:
                                                            totalTime = item.NhacTruocKhoangThoiGian * 3600;
                                                            break;
                                                        case LoaiThoiGian.DAY:
                                                            totalTime = item.NhacTruocKhoangThoiGian * 3600 * 24;
                                                            break;
                                                        case LoaiThoiGian.MONTH:
                                                            totalTime = item.NhacTruocKhoangThoiGian * 3600 * 24 * 30;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    if (oclock > 7)
                                                    {
                                                        await SendSMS(item.IdLoaiTin, param, inforCommon, totalTime);
                                                    }
                                                }
                                                break;
                                            default:// giaodich: luon gui
                                                await SendSMS(item.IdLoaiTin, param, inforCommon, 0);
                                                break;
                                        }
                                    }
                                }
                            }

                            var turnOnZalo = tblJoin.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.Zalo).Select(x => x);
                            foreach (var item in turnOnZalo)
                            {
                                if (!string.IsNullOrEmpty(item.NoiDungTin))
                                {
                                    inforCommon.NoiDungTinNhan = item.NoiDungTin;
                                    //await SendSMS(item.IdLoaiTin, param, inforCommon); // todo
                                }
                            }

                            var turnOnEmail = tblJoin.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.Gmail).Select(x => x);
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
                    }
                    catch (Exception ex)
                    {

                    }
                });
                }
            }
            unitOfWork.Complete();
        }

        protected string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin)
        {
            var ss = noiDungTin.Replace("{TenKhachHang}", cutomer.TenKhachHang);
            ss = ss.Replace("{NgaySinh}", cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("{BookingDate}", cutomer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("{ThoiGianHen}", cutomer.ThoiGianHen);
            ss = ss.Replace("{TenHangHoa}", cutomer.TenHangHoa);// dichvuhen
            ss = ss.Replace("{MaGiaoDich}", cutomer.MaHoaDon);
            ss = ss.Replace("{NgayGiaoDich}", cutomer.NgayLapHoaDon?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
            return ss;
        }

        protected async Task SendSMS(byte? idLoaiTin, ParamSearchSMS paramSearch, InforAutoWorker inforCommon, float? totalTime = 0)
        {
            try
            {
                paramSearch.HinhThucGuiTins = new List<byte> { ConstSMS.HinhThucGuiTin.SMS };
                PagedResultDto<PageKhachHangSMSDto> data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTin);
               
                if (data.TotalCount > 0)
                {
                    IReadOnlyCollection<PageKhachHangSMSDto> lstCustomer = null;
                    if(totalTime > 0)
                    {
                        // Nếu gửi trước: check start time ...: có thể có sai số (chênh lệch xx)
                        float? saisoTruoc = totalTime - 10;
                        float? saisoSau = totalTime + 10;
                        lstCustomer = data.Items.Where(x => x.ChenhLech > saisoTruoc && x.ChenhLech < saisoSau).ToList();
                    }
                    else
                    {
                        lstCustomer = data.Items;
                    }

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
                            HinhThucGui = ConstSMS.HinhThucGuiTin.SMS,
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
        protected async Task SendEmail(byte? idLoaiTin, ParamSearchSMS paramSearch, InforAutoWorker inforCommon)
        {
            try
            {
                paramSearch.HinhThucGuiTins = new List<byte> { ConstSMS.HinhThucGuiTin.Gmail };
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
                            await _emailSender.SendAsync(customer.Email, "Test send email", noidung, true);// todo TieuDe email
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
                            HinhThucGui = ConstSMS.HinhThucGuiTin.Gmail
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
