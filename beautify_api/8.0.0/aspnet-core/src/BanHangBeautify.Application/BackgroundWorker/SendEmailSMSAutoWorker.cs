using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.SMS.Brandname.Repository;
using BanHangBeautify.SMS.CaiDatNhacNho;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.ESMS;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using BanHangBeautify.SMS.MauTinSMS;
using BanHangBeautify.Zalo.GuiTinNhan;
using BanHangBeautify.Zalo.ZaloTemplate;
using BanHangBeautify.ZaloSMS_Common;

namespace BanHangBeautify.BackgroundWorker
{
    public class SendEmailSMSAutoWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public readonly IESMS _eSMS;
        public readonly ICaiDatNhacNhoRepository _repoCaiDatNhacNho;
        public readonly IMauTinSMSRepository _repoMauTinSMS;
        public readonly IRepository<DM_ChiNhanh, Guid> _dmChiNhanh;
        public readonly IRepository<HeThong_SMS, Guid> _hethongSMS;
        public readonly IRepository<SMS_NhatKy_GuiTin, Guid> _smsNhatKyGuiTin;
        public readonly IRepository<User, long> _user;
        private readonly IRepository<Tenant> _tenantRepository;
        public readonly IHeThongSMSRepository _repoSMS;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IEmailSender _emailSender;
        private readonly IBrandnameRepository _repoBrandname;
        public readonly IZalo_TemplateRepository _zaloTemplateRepo;
        public readonly IZaloSendMes _zaloApi;
        public readonly ICommonZaloSMS _commonZaloSMS;
        public readonly IRepository<ZaloAuthorization, Guid> _zaloAuthorization;
        readonly DateTime _dtNow = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

        public SendEmailSMSAutoWorker(AbpTimer timer,
            IESMS eSMS,
            ICaiDatNhacNhoRepository repoCaiDatNhacNho,
            IMauTinSMSRepository repoMauTinSMS,
            IRepository<DM_ChiNhanh, Guid> dmChiNhanh,
           IRepository<HeThong_SMS, Guid> hethongSMS,
             IRepository<SMS_NhatKy_GuiTin, Guid> smsNhatKyGuiTin,
             IRepository<User, long> user,
             IRepository<Tenant> tenantRepository,
            IHeThongSMSRepository repoSMS,
            IUnitOfWorkManager unitOfWorkManager,
            IEmailSender emailSender,
            IBrandnameRepository repoBrandname,
            IZalo_TemplateRepository zaloTemplateRepo,
            IRepository<ZaloAuthorization, Guid> zaloAuthorization,
              IZaloSendMes zaloApi,
              ICommonZaloSMS commonZaloSMS
            ) : base(timer)
        {
            Timer.Period = 10000;
            _eSMS = eSMS;
            _repoCaiDatNhacNho = repoCaiDatNhacNho;
            _repoMauTinSMS = repoMauTinSMS;
            _dmChiNhanh = dmChiNhanh;
            _hethongSMS = hethongSMS;
            _smsNhatKyGuiTin = smsNhatKyGuiTin;
            _user = user;
            _tenantRepository = tenantRepository;
            _repoSMS = repoSMS;
            _unitOfWorkManager = unitOfWorkManager;
            _emailSender = emailSender;
            _repoBrandname = repoBrandname;
            _zaloTemplateRepo = zaloTemplateRepo;
            _zaloApi = zaloApi;
            _zaloAuthorization = zaloAuthorization;
            _commonZaloSMS = commonZaloSMS;
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
                        List<CaiDatNhacNhoDto> lstSetup = await _repoCaiDatNhacNho.GetAllCaiDatNhacNho();

                        if (lstSetup != null && lstSetup.Count > 0)
                        {
                            var chiNhanh = _dmChiNhanh.GetAll().Select(x => x.Id.ToString()).ToList();
                            var userAdmin = _user.GetAllList(x => x.IsAdmin).Select(x => x.Id).FirstOrDefault();
                            var lstIdSetup = lstSetup.Select(x => x.Id);
                            var brandName_ofTenant = lstBrandname.Items.Where(x => x.TenantId == tenantId);

                            InforAutoWorker inforCommon = new()
                            {
                                TenantId = tenantId,
                                UserId = userAdmin
                            };
                            if (chiNhanh != null && chiNhanh.Count > 0)
                            {
                                inforCommon.IdChiNhanhFirst = new Guid(chiNhanh.FirstOrDefault().ToString());
                            }

                            ParamSearchSMS param = new()
                            {
                                FromDate = _dtNow,
                                ToDate = _dtNow,
                                IdChiNhanhs = chiNhanh,// all chinhanh
                                TrangThais = new List<string> { "0" }, // chi get tin chua gui (trangthai = 0)
                                CurrentPage = 1,
                                PageSize = 10000,
                            };

                            if (brandName_ofTenant.Any())
                            {
                                inforCommon.BrandNameFirst = brandName_ofTenant.FirstOrDefault().Brandname;

                                var oclock = DateTime.Now.Hour;
                                var minutes = DateTime.Now.Minute;

                                var turnOnSMS = lstSetup.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.SMS).Select(x => x);

                                var sms_OnBirthday = turnOnSMS.Where(x => x.IdLoaiTin == ConstSMS.LoaiTin.SinhNhat).FirstOrDefault();
                                if (sms_OnBirthday != null)
                                {
                                    var itemMauTin = await _repoMauTinSMS.GetMauTinSMS_byId(sms_OnBirthday.Id);
                                    if (itemMauTin != null)
                                    {
                                        inforCommon.NoiDungTinNhan = itemMauTin.NoiDungTinMau;

                                        if (oclock > 7)
                                        {
                                            // tin sinhnhat: chi gui từ 7h sáng
                                            await SendSMS(ConstSMS.LoaiTin.SinhNhat, param, inforCommon);
                                        }
                                    }
                                }

                                var sms_OnLichHen = turnOnSMS.Where(x => x.IdLoaiTin == ConstSMS.LoaiTin.LichHen).FirstOrDefault();
                                if (sms_OnLichHen != null)
                                {
                                    var itemMauTin = await _repoMauTinSMS.GetMauTinSMS_byId(sms_OnLichHen.Id);
                                    if (itemMauTin != null)
                                    {
                                        inforCommon.NoiDungTinNhan = itemMauTin.NoiDungTinMau;

                                        float? totalTime = 0;
                                        switch (sms_OnLichHen.LoaiThoiGian)
                                        {
                                            case LoaiThoiGian.SECOND:
                                                totalTime = sms_OnLichHen.NhacTruocKhoangThoiGian;
                                                break;
                                            case LoaiThoiGian.MiNUTE:
                                                totalTime = sms_OnLichHen.NhacTruocKhoangThoiGian * 60;
                                                break;
                                            case LoaiThoiGian.HOUR:
                                                totalTime = sms_OnLichHen.NhacTruocKhoangThoiGian * 3600;
                                                break;
                                            case LoaiThoiGian.DAY:
                                                totalTime = sms_OnLichHen.NhacTruocKhoangThoiGian * 3600 * 24;
                                                break;
                                            case LoaiThoiGian.MONTH:
                                                totalTime = sms_OnLichHen.NhacTruocKhoangThoiGian * 3600 * 24 * 30;
                                                break;
                                            default:
                                                break;
                                        }
                                        if (oclock > 7)
                                        {
                                            await SendSMS(ConstSMS.LoaiTin.LichHen, param, inforCommon, totalTime);
                                        }
                                    }
                                }

                                var sms_OnGiaoDich = turnOnSMS.Where(x => x.IdLoaiTin == ConstSMS.LoaiTin.GiaoDich).FirstOrDefault();
                                if (sms_OnGiaoDich != null)
                                {
                                    await SendSMS(ConstSMS.LoaiTin.GiaoDich, param, inforCommon, 0);
                                }
                            }

                            var turnOnZalo = lstSetup.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.Zalo).Select(x => x);
                            foreach (var item in turnOnZalo)
                            {
                                await Zalo_SendMes(item.IdLoaiTin, param, inforCommon);
                            }

                            var turnOnEmail = lstSetup.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.Gmail).Select(x => x);
                            foreach (var item in turnOnEmail)
                            {
                                //if (!string.IsNullOrEmpty(item.NoiDungTin))
                                //{
                                //    inforCommon.NoiDungTinNhan = item.NoiDungTin;
                                //    await SendEmail(item.IdLoaiTin, param, inforCommon);
                                //}
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
        protected async Task SendSMS(byte? idLoaiTin, ParamSearchSMS paramSearch, InforAutoWorker inforCommon, float? totalTime = 0)
        {
            try
            {
                paramSearch.HinhThucGuiTins = new List<byte> { ConstSMS.HinhThucGuiTin.SMS };
                PagedResultDto<PageKhachHangSMSDto> data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTin);

                if (data.TotalCount > 0)
                {
                    IReadOnlyCollection<PageKhachHangSMSDto> lstCustomer = null;
                    if (totalTime > 0)
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
                        var noidung = _commonZaloSMS.ReplaceContent(customer, inforCommon.NoiDungTinNhan);
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
                            IdChiNhanh = customer.IdChiNhanh ?? (Guid)inforCommon.IdChiNhanhFirst,
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
        protected async Task Zalo_SendMes(byte? idLoaiTin, ParamSearchSMS paramSearch, InforAutoWorker inforCommon, float? totalTime = 0)
        {
            try
            {
                paramSearch.HinhThucGuiTins = new List<byte> { ConstSMS.HinhThucGuiTin.Zalo };
                PagedResultDto<PageKhachHangSMSDto> data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTin);

                if (data.TotalCount > 0)
                {
                    IReadOnlyCollection<PageKhachHangSMSDto> lstCustomer = null;
                    if (totalTime > 0)
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

                    //// get token zalo
                    var zaloToken = _zaloAuthorization.GetAllList().OrderByDescending(x => x.CreationTime).FirstOrDefault();
                    // get temp default: todo get from db
                    var objFind = await _zaloTemplateRepo.FindTempDefault_ByIdLoaiTin(idLoaiTin ?? 0);
                    if (objFind != null)
                    {
                        foreach (var customer in lstCustomer)
                        {
                            // chỉ gửi tin nếu khách hàng chia sẻ thông tin cho OA
                            if (!string.IsNullOrEmpty(customer.ZOAUserId))
                            {
                                var noidung = string.Empty;

                                switch (idLoaiTin)
                                {
                                    case ConstSMS.LoaiTin.SinhNhat:
                                        noidung = $@"Chúc mừng sinh nhật khách hàng {customer.TenKhachHang},ngày sinh {customer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
                                        break;
                                    case ConstSMS.LoaiTin.GiaoDich:
                                        noidung = $@"Xác nhận thanh toán Hóa đơn: {customer.MaHoaDon}<br/> Ngày lập: {customer.NgayLapHoaDon?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} <br/> Tổng tiền: {customer.TongThanhToan}";
                                        break;
                                    case ConstSMS.LoaiTin.LichHen:
                                        noidung = $@"Xác nhận lịch hẹn dịch vụ: {customer.TenHangHoa}<br/> Số điện thoại đặt lịch: {customer.SoDienThoai}<br/> Ngày đặt lịch: {customer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
                                        break;
                                    default:
                                        noidung = _commonZaloSMS.ReplaceContent(customer, inforCommon.NoiDungTinNhan);
                                        break;
                                }

                                // send mes zalo
                                var smsResult = await _zaloApi.GuiTinTruyenThongorGiaoDich_fromDataDB(customer, zaloToken.AccessToken, objFind.Id);
                                if (smsResult != null && smsResult.data != null)
                                {
                                    HeThong_SMS hethongSMS = new()
                                    {
                                        Id = Guid.NewGuid(),
                                        TenantId = inforCommon.TenantId,
                                        IdLoaiTin = idLoaiTin ?? 0,
                                        IdChiNhanh = customer.IdChiNhanh ?? (Guid)inforCommon.IdChiNhanhFirst,
                                        IdKhachHang = customer.IdKhachHang,
                                        SoDienThoai = customer.SoDienThoai,
                                        SoTinGui = 1,
                                        IdTinNhan = smsResult.data.message_id,
                                        GiaTienMoiTinNhan = 200,
                                        TrangThai = 200,
                                        NoiDungTin = noidung,
                                        IdNguoiGui = inforCommon.UserId,
                                        CreatorUserId = inforCommon.UserId,
                                        ThoiGianGui = DateTime.Now,
                                        CreationTime = DateTime.Now,
                                        IsDeleted = false,
                                        HinhThucGui = ConstSMS.HinhThucGuiTin.Zalo,
                                    };
                                    await _hethongSMS.InsertAsync(hethongSMS);

                                    SMS_NhatKy_GuiTin nkyGuiSMS = new()
                                    {
                                        Id = Guid.NewGuid(),
                                        TenantId = inforCommon.TenantId,
                                        IdHeThongSMS = hethongSMS.Id,
                                        ThoiGianTu = _dtNow,
                                        ThoiGianDen = _dtNow,
                                        IdBooking = idLoaiTin == ConstSMS.LoaiTin.LichHen ? customer.IdBooking : null,
                                        IdHoaDon = idLoaiTin == ConstSMS.LoaiTin.GiaoDich ? customer.IdHoaDon : null,
                                        CreationTime = DateTime.Now,
                                        CreatorUserId = inforCommon.UserId,
                                        IsDeleted = false,
                                    };
                                    await _smsNhatKyGuiTin.InsertAsync(nkyGuiSMS);
                                }
                            }
                        }
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
                        var noidung = _commonZaloSMS.ReplaceContent(customer, inforCommon.NoiDungTinNhan);

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
