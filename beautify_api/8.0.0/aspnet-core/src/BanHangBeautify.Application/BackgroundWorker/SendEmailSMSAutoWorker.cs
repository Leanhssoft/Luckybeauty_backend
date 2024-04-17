using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Bookings.Bookings.Dto;
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
using Castle.Core.Resource;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                                var turnOnSMS = lstSetup.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.SMS && x.TrangThai == 1).Select(x => x);

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
                                        float? totalTime = GetTotalTime(sms_OnLichHen.LoaiThoiGian, sms_OnLichHen.NhacTruocKhoangThoiGian);

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

                            var turnOnZalo = lstSetup.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.Zalo && x.TrangThai == 1).Select(x => x);
                            foreach (var item in turnOnZalo)
                            {
                                await Zalo_SendMes(item.IdLoaiTin, param, inforCommon, item);
                            }

                            var turnOnEmail = lstSetup.Where(x => x.HinhThucGui == ConstSMS.HinhThucGuiTin.Gmail && x.TrangThai == 1).Select(x => x);
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
                byte? idLoaiTinFilter = idLoaiTin;
                // tin liên quan đến lịch hẹn: filter = 3
                switch (idLoaiTinFilter)
                {
                    case ConstSMS.LoaiTin.LichHen:
                    case ConstSMS.LoaiTin.NhacLichHen:
                    case ConstSMS.LoaiTin.XacNhanLichHen:
                        idLoaiTinFilter = ConstSMS.LoaiTin.LichHen;
                        break;
                }
                PagedResultDto<PageKhachHangSMSDto> data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTinFilter);

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
                        await SaveNhatKyGuiTin(idLoaiTin, customer, inforCommon, smsResult.MessageId, smsResult.MessageStatus,
                            ConstSMS.HinhThucGuiTin.SMS, 950, noidung);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task Zalo_SendMes(byte? idLoaiTin, ParamSearchSMS paramSearch, InforAutoWorker inforCommon, CaiDatNhacNhoDto itemSetup)
        {
            try
            {
                paramSearch.HinhThucGuiTins = new List<byte> { ConstSMS.HinhThucGuiTin.Zalo };
                byte? idLoaiTinFilter = idLoaiTin;
                // tin liên quan đến lịch hẹn: filter = 3
                switch (idLoaiTinFilter)
                {
                    case ConstSMS.LoaiTin.LichHen:
                    case ConstSMS.LoaiTin.NhacLichHen:
                    case ConstSMS.LoaiTin.XacNhanLichHen:
                        idLoaiTinFilter = ConstSMS.LoaiTin.LichHen;
                        break;
                }
                PagedResultDto<PageKhachHangSMSDto> data = await _repoSMS.GetListCustomer_byIdLoaiTin(paramSearch, idLoaiTinFilter);

                if (data.TotalCount > 0)
                {
                    IReadOnlyCollection<PageKhachHangSMSDto> lstCustomer = null;
                    float totalTime = 0;
                    if (itemSetup.LoaiThoiGian != 0 && itemSetup.NhacTruocKhoangThoiGian != 0)
                    {
                        totalTime = GetTotalTime(itemSetup.LoaiThoiGian, itemSetup.NhacTruocKhoangThoiGian);
                    }
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
                    if (itemSetup.IdMauTin.Length < 36)
                    {
                        // mẫu tin là mẫu ZNS của zalo (gửi tin qua SĐT)
                        var znsTemp = await _zaloApi.GetZNSTemplateDetails_byId(zaloToken.AccessToken, itemSetup.IdMauTin);
                        if (znsTemp != null)
                        {
                            foreach (var customer in lstCustomer)
                            {
                                if (!string.IsNullOrEmpty(customer.SoDienThoai))
                                {
                                    var dataMes = await _zaloApi.GuiTinZalo_UseZNS(customer, zaloToken.AccessToken, znsTemp);
                                    var statusMes = dataMes.error == 0 ? 200 : dataMes.error;
                                    int priceInt = 0;
                                    try
                                    {
                                        float.TryParse(znsTemp.price, out float price);
                                        priceInt = (int)price;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (dataMes.error == 0 && dataMes.data != null)
                                    {
                                        await SaveNhatKyGuiTin(idLoaiTin, customer, inforCommon, dataMes.data.message_id, statusMes, ConstSMS.HinhThucGuiTin.Zalo, priceInt);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // mẫu tin là mẫu từ Database
                        var objFind = _zaloTemplateRepo.GetZaloTemplate_byId(new Guid(itemSetup.IdMauTin));
                        if (objFind != null)
                        {
                            foreach (var customer in lstCustomer)
                            {
                                // chỉ gửi tin nếu khách hàng chia sẻ thông tin cho OA
                                if (!string.IsNullOrEmpty(customer.ZOAUserId))
                                {
                                    var dataMes = await _zaloApi.GuiTinTruyenThongorGiaoDich_fromDataDB(customer, zaloToken.AccessToken, objFind.Id);
                                    if (dataMes != null && dataMes.data != null)
                                    {
                                        var statusMes = dataMes.error == 0 ? 200 : dataMes.error;
                                        var price = 0;
                                        switch (objFind.TemplateType)
                                        {
                                            case ZaloTemplateType.TRANSACTION:
                                            case ZaloTemplateType.BOOKING:
                                                {
                                                    price = 165;
                                                }
                                                break;
                                            case ZaloTemplateType.MESSAGE:
                                            case ZaloTemplateType.MEDIA:
                                                {
                                                    price = 55;
                                                }
                                                break;
                                        }
                                        await SaveNhatKyGuiTin(idLoaiTin, customer, inforCommon, dataMes.data.message_id, statusMes, ConstSMS.HinhThucGuiTin.Zalo, price);
                                    }
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

        private async Task SaveNhatKyGuiTin(byte? idLoaiTin, PageKhachHangSMSDto customer, InforAutoWorker inforCommon,
            string messageId, int messageStatus, byte? hinhThucGuiTin = 0, int messagePrice = 950, string noidungTin = null)
        {
            try
            {
                string noidung = noidungTin;
                if (string.IsNullOrEmpty(noidung))
                {
                    switch (idLoaiTin)
                    {
                        case ConstSMS.LoaiTin.SinhNhat:
                            noidung = $@"Chúc mừng sinh nhật khách hàng {customer.TenKhachHang},ngày sinh {customer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
                            break;
                        case ConstSMS.LoaiTin.GiaoDich:
                            noidung = $@"Xác nhận thanh toán Hóa đơn: {customer.MaHoaDon}<br/> Ngày lập: {customer.NgayLapHoaDon?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} <br/> Tổng tiền: {customer.TongThanhToan}";
                            break;
                        case ConstSMS.LoaiTin.XacNhanLichHen:
                            noidung = $@"Xác nhận lịch hẹn dịch vụ: {customer.TenHangHoa}<br/> Số điện thoại đặt lịch: {customer.SoDienThoai}<br/> Ngày đặt lịch: {customer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
                            break;
                        case ConstSMS.LoaiTin.NhacLichHen:
                            noidung = $@"Nhắc lịch hẹn khách hàng {customer.TenKhachHang} <br/> Thời gian hẹn: {customer.StartTime} tại {customer.DiaChiChiNhanh} <br/> Sdt đặt lịch: {customer.SoDienThoai}";
                            break;
                        default:// todo noidung
                            noidung = _commonZaloSMS.ReplaceContent(customer, noidung);
                            break;
                    }
                }
                HeThong_SMS hethongSMS = new()
                {
                    Id = Guid.NewGuid(),
                    TenantId = inforCommon.TenantId,
                    IdLoaiTin = (idLoaiTin == ConstSMS.LoaiTin.XacNhanLichHen || idLoaiTin == ConstSMS.LoaiTin.XacNhanLichHen) ? ConstSMS.LoaiTin.LichHen : idLoaiTin ?? 0,
                    IdChiNhanh = customer.IdChiNhanh ?? (Guid)inforCommon.IdChiNhanhFirst,
                    IdKhachHang = customer.IdKhachHang,
                    SoDienThoai = customer.SoDienThoai,
                    SoTinGui = 1,
                    IdTinNhan = messageId,
                    GiaTienMoiTinNhan = messagePrice,
                    TrangThai = messageStatus,
                    NoiDungTin = noidung,
                    IdNguoiGui = inforCommon.UserId,
                    CreatorUserId = inforCommon.UserId,
                    ThoiGianGui = DateTime.Now,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    HinhThucGui = hinhThucGuiTin,
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
            catch (Exception)
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

        private float GetTotalTime(byte? loaiThoiGian, float? nhacTruoc = 0)
        {
            float? totalTime = 0;
            switch (loaiThoiGian)
            {
                case LoaiThoiGian.SECOND:
                    totalTime = nhacTruoc;
                    break;
                case LoaiThoiGian.MiNUTE:
                    totalTime = nhacTruoc * 60;
                    break;
                case LoaiThoiGian.HOUR:
                    totalTime = nhacTruoc * 3600;
                    break;
                case LoaiThoiGian.DAY:
                    totalTime = nhacTruoc * 3600 * 24;
                    break;
                case LoaiThoiGian.MONTH:
                    totalTime = nhacTruoc * 3600 * 24 * 30;
                    break;
                default:
                    break;
            }
            return totalTime ?? 0;
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
