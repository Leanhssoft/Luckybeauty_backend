﻿using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Notifications;
using Abp.Runtime.Security;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DatLichOnline.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.NhanSu.NhanVien.Responsitory;
using BanHangBeautify.Notifications;
using BanHangBeautify.Roles.Repository;
using BanHangBeautify.Suggests.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.DatLichOnline
{
    public class OnlineBookingAppService : SPAAppServiceBase
    {
        IRepository<Tenant, int> _tenantRepository;
        IRepository<Booking, Guid> _bookingRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<DM_HangHoa, Guid> _dichVuRepository;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _donViQuiDoiRepository;
        IRepository<BookingNhanVien, Guid> _bookingNhanVienRepository;
        IRepository<BookingService, Guid> _bookingServiceRepository;
        IRepository<NS_LichLamViec, Guid> _lichLamViecRepository;
        IRepository<NS_LichLamViec_Ca, Guid> _lichLamViecCaRepository;
        IRepository<NS_CaLamViec, Guid> _caLamViecRepository;
        IRepository<DM_NgayNghiLe, Guid> _nghiLeRepository;
        IRepository<HT_CongTy, Guid> _congTyRepository;
        IRepository<DM_KhachHang, Guid> _dmKhachHangRepository;
        IRepository<NS_NhanVien, Guid> _nhanVienResponsitory;
        INhanSuRepository _nhanSuService;
        private readonly IAppNotifier _appNotifier;
        INotificationAppService _notificationAppService;
        IUserRoleRepository _userRoleRepository;
        private readonly IConfiguration _configuration;

        public OnlineBookingAppService(
            IRepository<Tenant, int> tenantRepository,
            IRepository<Booking, Guid> bookingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<DM_HangHoa, Guid> dichVuRepository,
            IRepository<DM_DonViQuiDoi, Guid> donViQuiDoiRepository,
            IRepository<BookingNhanVien, Guid> bookingNhanVienRepository,
            IRepository<BookingService, Guid> bookingServiceRepository,
            IRepository<NS_LichLamViec, Guid> lichLamViecRepository,
            IRepository<NS_LichLamViec_Ca, Guid> lichLamViecCaRepository,
            IRepository<NS_CaLamViec, Guid> caLamViecRepository,
            IRepository<DM_NgayNghiLe, Guid> nghiLeRepository,
        IRepository<HT_CongTy, Guid> congTyRepository,
            IAppNotifier appNotifier,
            INotificationAppService notificationAppService,
            IRepository<DM_KhachHang, Guid> khachHangRepository,
            INhanSuRepository nhanSuService,
            IRepository<NS_NhanVien, Guid> nhanVienResponsitory,
              IUserRoleRepository userRoleRepository,
              IConfiguration configuration
            )
        {
            _tenantRepository = tenantRepository;
            _bookingRepository = bookingRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _dichVuRepository = dichVuRepository;
            _donViQuiDoiRepository = donViQuiDoiRepository;
            _bookingNhanVienRepository = bookingNhanVienRepository;
            _bookingServiceRepository = bookingServiceRepository;
            _lichLamViecCaRepository = lichLamViecCaRepository;
            _lichLamViecRepository = lichLamViecRepository;
            _caLamViecRepository = caLamViecRepository;
            _nghiLeRepository = nghiLeRepository;
            _congTyRepository = congTyRepository;
            _appNotifier = appNotifier;
            _notificationAppService = notificationAppService;
            _dmKhachHangRepository = khachHangRepository;
            _nhanSuService = nhanSuService;
            _nhanVienResponsitory = nhanVienResponsitory;
            _userRoleRepository = userRoleRepository;
            _configuration = configuration;
        }
        public List<string> GetAllTenant()
        {
            List<string> result = new List<string>();
            result = _tenantRepository.GetAll().Where(x => x.IsDeleted == false && x.IsActive == true).Select(x => x.TenancyName.ToLower()).ToList();
            return result;
        }
        public async Task<StoreInfoDto> GetThongTinCuaHang(string tenantName)
        {
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName);
            if (tenant == null)
            {
                return null;
            }
            using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
            {
                await CurrentUnitOfWork.SaveChangesAsync();
                StoreInfoDto store = new StoreInfoDto();
                var congTy = await _congTyRepository.GetAll().FirstOrDefaultAsync();
                store.TenCuaHang = congTy.TenCongTy;
                store.MaSoThue = congTy.MaSoThue;
                store.DiaChi = congTy.DiaChi;
                store.SoDienThoai = congTy.SoDienThoai;
                store.Logo = congTy.Logo;
                store.Website = congTy.Website;
                return store;
            }
        }
        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVien(PagedRequestSuggestNhanVien input)
        {
            List<SuggestEmpolyeeExecuteServiceDto> result = new List<SuggestEmpolyeeExecuteServiceDto>();
            input.TenNhanVien = string.IsNullOrEmpty(input.TenNhanVien) ? "" : input.TenNhanVien;
            string connecStringInServer = _configuration.GetConnectionString("Default");
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == input.TenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            var conn = new SqlConnection(@connectionString);
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand("prc_bookingOnline_SuggestNhanVien", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TenantId", tenant.Id));
                        cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                        cmd.Parameters.Add(new SqlParameter("@IdDichVu", input.IdDichVu));
                        cmd.Parameters.Add(new SqlParameter("@TenNhanVien", input.TenNhanVien ?? ""));
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            string[] array = { "Data" };
                            var ds = new DataSet();
                            ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                            var ddd = ds.Tables;

                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                var data = ObjectHelper.FillCollection<SuggestEmpolyeeExecuteServiceDto>(ds.Tables[0]);
                                return data;
                            }
                        }
                        conn.Close();
                        return new List<SuggestEmpolyeeExecuteServiceDto>();
                    }
                }

            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Open)
                {
                    result = new List<SuggestEmpolyeeExecuteServiceDto>();
                }
                conn.Close();
            }
            return result;
        }
        public async Task<List<SuggestDichVuBookingOnlineDto>> SuggestDichVu(PagedRequestSuggestDichVu input)
        {

            List<SuggestDichVuBookingOnlineDto> result = new List<SuggestDichVuBookingOnlineDto>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == input.TenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            string connecStringInServer = _configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            string sqlQuery = "";
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    sqlQuery += "SELECT ISNULL(nhh.TenNhomHang,N'Chưa phân loại') as TenNhomHangHoa,nhh.Color,dvqd.Id,hh.TenHangHoa as TenDichVu,dvqd.GiaBan as DonGia,hh.SoPhutThucHien,hh.Image FROM DM_DonViQuiDoi dvqd \n";
                    sqlQuery += "JOIN DM_HangHoa hh on hh.Id = dvqd.IdHangHoa \n";
                    sqlQuery += "LEFT JOIN DM_NhomHangHoa nhh on nhh.Id = hh.IdNhomHangHoa \n";
                    sqlQuery += string.Format($"WHERE hh.IdLoaiHangHoa != {LoaiHangHoaConst.HangHoa} \n");
                    sqlQuery += string.Format($"AND hh.TenantId = {tenant.Id} AND hh.IsDeleted = 0 \n");
                    if (!string.IsNullOrEmpty(input.TenNhomDichVu))
                    {
                        sqlQuery += string.Format($"AND LOWER(ISNULL(nhh.TenNhomHang,N'Chưa phân loại')) = LOWER(N'{input.TenNhomDichVu.ToLower()}') \n");
                    }
                    if (!string.IsNullOrEmpty(input.Keyword))
                    {
                        sqlQuery += string.Format($"AND (LOWER(nhh.TenNhomHang) LIKE N'%{input.Keyword.ToLower()}%' OR LOWER(hh.TenHangHoa) LIKE N'%{input.Keyword.ToLower()}%') \n");
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        // Insert into the database
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlQuery;
                            await cmd.ExecuteNonQueryAsync();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                string[] array = { "Data" };
                                var ds = new DataSet();
                                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    var data = ObjectHelper.FillCollection<SuggestDichVuBookingDto>(ds.Tables[0]);
                                    for (int i = 0; i < data.Count; i++)
                                    {
                                        var donGia = ds.Tables[0].Rows[i]["DonGia"].ToString();
                                        data[i].DonGia = decimal.Parse(string.IsNullOrEmpty(donGia) ? "0" : donGia);
                                    }
                                    var group = data.ToList().GroupBy(x => new { x.TenNhomHangHoa, x.Color }).ToList();
                                    foreach (var item in group)
                                    {
                                        SuggestDichVuBookingOnlineDto dto = new SuggestDichVuBookingOnlineDto();
                                        dto.TenNhomHangHoa = item.Key.TenNhomHangHoa;
                                        dto.Color = item.Key.Color;
                                        dto.DanhSachDichVu = item.ToList();
                                        result.Add(dto);
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        result = new List<SuggestDichVuBookingOnlineDto>();
                    }
                }
            }

            return result;
        }
        public async Task<List<SuggestChiNhanhBooking>> SuggestChiNhanh(string tenantName)
        {
            List<SuggestChiNhanhBooking> result = new List<SuggestChiNhanhBooking>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            string connecStringInServer = _configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            string sqlQuery = "";
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    sqlQuery += "SELECT Id,TenChiNhanh,DiaChi,SoDienThoai,Logo FROM DM_ChiNhanh \n";
                    sqlQuery += string.Format($"WHERE TenantId = {tenant.Id} AND IsDeleted = 0 \n");
                    if (conn.State == ConnectionState.Open)
                    {
                        // Insert into the database
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlQuery;
                            await cmd.ExecuteNonQueryAsync();
                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                // Create a dataset to hold the retrieved data
                                DataSet dataSet = new DataSet();
                                dataAdapter.Fill(dataSet, "ChiNhanh");
                                DataTable dataTable = dataSet.Tables["ChiNhanh"];
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    SuggestChiNhanhBooking rdo = new SuggestChiNhanhBooking();
                                    rdo.Id = Guid.Parse(row["Id"].ToString());
                                    rdo.TenChiNhanh = row["TenChiNhanh"].ToString();
                                    rdo.Logo = row["Logo"].ToString();
                                    rdo.DiaChi = row["DiaChi"].ToString();
                                    rdo.SoDienThoai = row["SoDienThoai"].ToString();
                                    result.Add(rdo);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    result = new List<SuggestChiNhanhBooking>();
                }

            }
            return result;
        }
        public async Task<List<SuggestNhomHangHoaBookingOnlineDto>> SuggestNhomDichVu(string tenantName)
        {
            List<SuggestNhomHangHoaBookingOnlineDto> result = new List<SuggestNhomHangHoaBookingOnlineDto>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            string connecStringInServer = _configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            string sqlQuery = "";
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    sqlQuery += "SELECT ISNULL(nhh.Color,'#FFF') as Color, ISNULL(nhh.TenNhomHang,N'Chưa phân loại') as TenNhomHangHoa FROM DM_DonViQuiDoi dvqd \n";
                    sqlQuery += "JOIN DM_HangHoa hh on hh.Id = dvqd.IdHangHoa \n";
                    sqlQuery += "LEFT JOIN DM_NhomHangHoa nhh on nhh.Id = hh.IdNhomHangHoa \n";
                    sqlQuery += string.Format($"WHERE hh.IdLoaiHangHoa != {LoaiHangHoaConst.HangHoa} \n");
                    sqlQuery += string.Format($"AND hh.TenantId = {tenant.Id} AND hh.IsDeleted = 0 \n");
                    sqlQuery += "GROUP BY nhh.Color,nhh.TenNhomHang";
                    if (conn.State == ConnectionState.Open)
                    {
                        // Insert into the database
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlQuery;
                            await cmd.ExecuteNonQueryAsync();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                string[] array = { "Data" };
                                var ds = new DataSet();
                                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    var data = ObjectHelper.FillCollection<SuggestNhomHangHoaBookingOnlineDto>(ds.Tables[0]);
                                    result = data;

                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        result = new List<SuggestNhomHangHoaBookingOnlineDto>();
                    }
                }
            }

            return result;
        }
        [UnitOfWork]
        public async Task<ExecuteResultDto> CreateBooking(string tenantName, DatLichDto data)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName);
            if (tenant == null)
            {
                return null;
            }
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
                {
                    await CurrentUnitOfWork.SaveChangesAsync();
                    DM_KhachHang kh = new DM_KhachHang();
                    var checkKhachHang = await _dmKhachHangRepository.GetAll().Where(x => x.SoDienThoai == data.SoDienThoai).FirstOrDefaultAsync();
                    if (checkKhachHang != null)
                    {
                        kh.Id = checkKhachHang.Id;
                    }
                    else
                    {
                        kh.Id = Guid.NewGuid();
                        var checkMa = _dmKhachHangRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).ToList();
                        kh.MaKhachHang = "KH00" + (checkMa.Count + 1).ToString();
                        kh.TenantId = tenant.Id;
                        kh.TenKhachHang = data.TenKhachHang;
                        kh.SoDienThoai = data.SoDienThoai;
                        kh.IsDeleted = false;
                        kh.CreationTime = DateTime.Now;
                        await _dmKhachHangRepository.InsertAsync(kh);

                    }
                    Booking bk = new Booking();
                    bk.Id = Guid.NewGuid();
                    bk.IdChiNhanh = data.IdChiNhanh;
                    bk.BookingDate = DateTime.Parse(data.BookingDate);
                    bk.IdKhachHang = kh.Id;
                    var startTime = bk.BookingDate.ToString("yyyy-MM-dd") + " " + data.StartTime;
                    bk.StartTime = DateTime.Parse(startTime);
                    bk.EndTime = bk.StartTime.AddMinutes(data.SoPhutThucHien);
                    bk.GhiChu = data.GhiChu;
                    bk.LoaiBooking = LoaiBookingConst.BookingOnline;
                    bk.TrangThai = TrangThaiBookingConst.DatLich;
                    bk.SoDienThoai = data.SoDienThoai;
                    bk.TenKhachHang = data.TenKhachHang;
                    bk.TenantId = tenant.Id;
                    bk.CreationTime = DateTime.Now;
                    bk.IsDeleted = false;

                    _bookingRepository.Insert(bk);
                    var idDichVu = _donViQuiDoiRepository.FirstOrDefault(x => x.Id == data.IdDichVu).IdHangHoa;
                    var dichVu = _dichVuRepository.FirstOrDefault(x => x.Id == idDichVu);
                    var bookingService = CreateBookingService(bk.Id, data.IdDichVu);
                    var bookingNhanVien = CreateBookingNhanVien(bk.Id, data.IdNhanVien);
                    _bookingNhanVienRepository.Insert(bookingNhanVien);
                    _bookingServiceRepository.Insert(bookingService);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var listUserRole = await _userRoleRepository.GetListUser_havePermission(tenant.Id, data.IdChiNhanh, "Pages.Notifications.Booking");
                    var listUser = ObjectMapper.Map<List<UserIdentifier>>(listUserRole);
                    string mess = "Khách hàng: " + bk.TenKhachHang + "(" + bk.SoDienThoai + ")" + " đã đặt lịch hẹn làm dịch vụ : " + dichVu.TenHangHoa + " vào " + bk.BookingDate.ToString("dd/MM/yyyy") + " " + bk.StartTime.ToString("HH:mm");
                    var notificationData = new LocalizableMessageNotificationData(
                        new LocalizableString(
                            mess,
                            "LuckyBeauty"
                        )
                    );
                    await _appNotifier.SendMessageAsync(TrangThaiBookingConst.AddNewBooking, notificationData, listUser, severity: NotificationSeverity.Info);
                    result.Message = "Đặt lịch thành công!";
                    result.Status = "success";
                }

            }
            catch (Exception ex)
            {

                result.Message = "Đặt lịch thất bại!";
                result.Status = "error";
                result.Detail = ex.Message;
            }


            return result;
        }
        [NonAction]
        private async Task<List<UserIdentifier>> getUserAdmin(Guid idChiNhanh, int tenantId)
        {
            var nhanViens = (await _nhanSuService.GetAllNhanSu(new PagedNhanSuRequestDto
            {
                Filter = "",
                SkipCount = 0,
                MaxResultCount = int.MaxValue,
                IdChiNhanh = idChiNhanh,
                TenantId = tenantId
            })).Items;
            var idNhanViens = nhanViens.Select(x => x.Id).ToList();
            var users = await UserManager.Users
                .Where(us => us.NhanSuId != null && idNhanViens.Contains((Guid)us.NhanSuId))
                .Select(us => new UserIdentifier(us.TenantId, us.Id))
                .ToListAsync();

            var adminUserIds = await UserManager.Users
                .Where(us => us.IsAdmin == true)
                .Select(us => us.Id)
                .ToListAsync();

            users.AddRange(adminUserIds.Select(id => new UserIdentifier(1, id)));

            return users.Distinct().ToList();
        }
        [NonAction]
        public BookingService CreateBookingService(Guid idBooking, Guid idDichVuQuiDoi)
        {
            BookingService bookingService = new BookingService();
            try
            {
                bookingService.Id = Guid.NewGuid();
                bookingService.TenantId = AbpSession.TenantId ?? 1;
                bookingService.IdBooking = idBooking;
                bookingService.IdDonViQuiDoi = idDichVuQuiDoi;
                bookingService.IsDeleted = false;
                bookingService.CreationTime = DateTime.Now;
                bookingService.CreatorUserId = AbpSession.UserId;
            }
            catch (Exception)
            {
                bookingService = new BookingService();
            }
            return bookingService;
        }
        [NonAction]
        public BookingNhanVien CreateBookingNhanVien(Guid idBooking, Guid idNhanVien)
        {
            BookingNhanVien result = new BookingNhanVien();
            try
            {
                result.Id = Guid.NewGuid();
                result.TenantId = AbpSession.TenantId ?? 1;
                result.IdBooking = idBooking;
                result.IdNhanVien = idNhanVien;
                result.IsDeleted = false;
                result.CreatorUserId = AbpSession.UserId;
                result.CreationTime = DateTime.Now;
            }
            catch (Exception)
            {
                result = new BookingNhanVien();
            }
            return result;
        }
        [HttpPost]
        public async Task<List<AvailableTime>> GetAviableTime(PagedRequestAvailableTime input)
        {
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == input.TenantName);

            List<AvailableTime> times = new List<AvailableTime>();
            using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
            {
                var nhanVien = _bookingNhanVienRepository.GetAll().Where(
                        x => x.IdNhanVien == input.IdNhanVien &&
                        x.TenantId == tenant.Id && x.IsDeleted == false).ToList();
                var appointments = _bookingRepository.GetAll().Where(x => nhanVien.Select(z => z.IdBooking).Contains(x.Id) && x.BookingDate.Date == input.DateBooking.Date && x.IsDeleted == false).ToList();
                var lichLamViec = _lichLamViecRepository.GetAll().Where(x => x.IdNhanVien == input.IdNhanVien && x.IsDeleted == false).ToList();
                var lichLamViecCa = _lichLamViecCaRepository.GetAll().Where(x => lichLamViec.Select(z => z.Id).ToList().Contains(x.IdLichLamViec) && x.NgayLamViec.Date == input.DateBooking.Date && x.IsDeleted == false).ToList();
                var caLamViec = _caLamViecRepository.GetAll().Where(x => lichLamViecCa.Select(y => y.IdCaLamViec).Contains(x.Id) && x.IsDeleted == false).ToList();
                var nghiLe = _nghiLeRepository.GetAll().Where(x => x.DenNgay >= input.DateBooking && x.TuNgay <= input.DateBooking).ToList();
                if (nghiLe != null && nghiLe.Count > 0)
                {
                    return new List<AvailableTime>();
                }
                if (caLamViec != null && caLamViec.Count > 0)
                {
                    foreach (var x in caLamViec)
                    {
                        var gioVaoStr = input.DateBooking.ToString("yyyy/MM/dd") + " " + x.GioVao.ToString("HH:mm");
                        var startTime = DateTime.Parse(gioVaoStr);
                        var gioRaStr = input.DateBooking.ToString("yyyy/MM/dd") + " " + x.GioRa.ToString("HH:mm");
                        var endTime = DateTime.Parse(gioRaStr);
                        var currentTime = startTime;
                        while (currentTime.AddMinutes(input.ServiceTime) <= endTime)
                        {
                            AvailableTime time = new AvailableTime();
                            bool isAvailableTime = true;
                            time.Time = currentTime.ToString("HH:mm");

                            if (x.LaNghiGiuaCa == true)
                            {
                                var nghiTuStr = input.DateBooking.ToString("yyyy/MM/dd") + " " + x.GioNghiTu.Value.ToString("HH:mm");
                                var nghiTu = DateTime.Parse(nghiTuStr);
                                var nghiDenStr = input.DateBooking.ToString("yyyy/MM/dd") + " " + x.GioNghiDen.Value.ToString("HH:mm");
                                var nghiDen = DateTime.Parse(nghiDenStr);
                                if (currentTime < DateTime.Now)
                                {
                                    isAvailableTime = false;
                                }
                                else if (currentTime >= nghiTu && currentTime <= nghiDen)
                                {
                                    isAvailableTime = false;
                                }
                                else
                                {
                                    foreach (var appointment in appointments)
                                    {
                                        if ((currentTime >= appointment.StartTime && currentTime.AddMinutes(input.ServiceTime) <= appointment.EndTime))
                                        {
                                            isAvailableTime = false;
                                            break;
                                        }
                                    }
                                }
                                time.IsAvailableTime = isAvailableTime;
                                times.Add(time);
                                currentTime = currentTime.AddMinutes(input.ServiceTime);
                            }
                            else
                            {
                                if (currentTime < DateTime.Now)
                                {
                                    isAvailableTime = false;
                                }
                                else
                                {
                                    foreach (var appointment in appointments)
                                    {
                                        if ((currentTime >= appointment.StartTime && currentTime.AddMinutes(input.ServiceTime) <= appointment.EndTime))
                                        {
                                            isAvailableTime = false;
                                            break;
                                        }
                                    }
                                }
                                time.IsAvailableTime = isAvailableTime;
                                times.Add(time);
                                currentTime = currentTime.AddMinutes(input.ServiceTime);
                            }
                        }

                    }
                }
                else
                {
                    var currentTime = new DateTime(input.DateBooking.Year, input.DateBooking.Month, input.DateBooking.Day, 07, 00, 00);
                    var endTime = new DateTime(input.DateBooking.Year, input.DateBooking.Month, input.DateBooking.Day, 20, 00, 00);
                    while (currentTime.AddMinutes(input.ServiceTime) <= endTime)
                    {
                        AvailableTime time = new AvailableTime();
                        bool isAvailableTime = true;
                        time.Time = currentTime.ToString("HH:mm");

                        if (currentTime < DateTime.Now)
                        {
                            isAvailableTime = false;
                        }
                        else
                        {
                            foreach (var appointment in appointments)
                            {
                                if ((currentTime >= appointment.StartTime && currentTime.AddMinutes(input.ServiceTime) <= appointment.EndTime))
                                {
                                    isAvailableTime = false;
                                    break;
                                }
                            }
                        }
                        time.IsAvailableTime = isAvailableTime;
                        times.Add(time);
                        currentTime = currentTime.AddMinutes(input.ServiceTime);
                    }
                }

            }
            return times.OrderByDescending(x => x.Time).Reverse().ToList();
        }

    }
}
public class PagedRequestAvailableTime
{
    public Guid IdNhanVien { get; set; }
    public string TenantName { get; set; }
    public DateTime DateBooking { get; set; }
    public int ServiceTime { get; set; }
}
public class AvailableTime
{
    public bool IsAvailableTime { get; set; }
    public string Time { get; set; }
}
public class PagedRequestSuggestDichVu
{
    [Required]
    public string TenantName { get; set; }
    public string TenNhomDichVu { get; set; }
    public string Keyword { get; set; }

}
public class PagedRequestSuggestNhanVien
{
    [Required]
    public string TenantName { get; set; }
    public Guid IdChiNhanh { get; set; }
    public Guid IdDichVu { get; set; }
    public string TenNhanVien { get; set; }

}

public class SuggestChiNhanhBooking : SuggestChiNhanh
{
    public string Logo { get; set; }
    public string DiaChi { get; set; }
    public string SoDienThoai { get; set; }
}
public class SuggestDichVuBookingDto : SuggestDichVuDto
{
    public string TenNhomHangHoa { set; get; }
    public string Image { get; set; }
    public string Color { get; set; }
    public float SoPhutThucHien { get; set; }
}
public class SuggestDichVuBookingOnlineDto
{
    public string TenNhomHangHoa { set; get; }
    public string Color { get; set; }
    public List<SuggestDichVuBookingDto> DanhSachDichVu { set; get; }
}
public class SuggestNhomHangHoaBookingOnlineDto
{
    public string Color { set; get; }
    public string TenNhomHangHoa { get; set; }
}