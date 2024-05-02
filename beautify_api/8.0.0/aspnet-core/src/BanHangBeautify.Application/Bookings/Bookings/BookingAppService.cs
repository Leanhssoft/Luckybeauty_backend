using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Notifications;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Bookings.Bookings.BookingRepository;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.NhanSu.NhanVien.Responsitory;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.Notifications;
using BanHangBeautify.Roles.Repository;
using BanHangBeautify.SignalR.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Atp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings
{
    [AbpAuthorize(PermissionNames.Pages_Booking)]
    public class BookingAppService : SPAAppServiceBase, IBookingAppService
    {
        private readonly IRepository<Booking, Guid> _repository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRepository<BookingNhanVien, Guid> _bookingNhanVienRepository;
        private readonly IRepository<BookingService, Guid> _bookingServiceRepository;
        private readonly IRepository<DM_KhachHang, Guid> _khachHangRepository;
        private readonly IRepository<DM_HangHoa, Guid> _dichVuRepository;
        private readonly IRepository<NS_NhanVien, Guid> _nhanVienService;
        private readonly INhanSuRepository _nhanSuRepository;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _donViQuiDoiRepository;
        private readonly IAppNotifier _appNotifier;
        IUserRoleRepository _userRoleRepository;
        INhatKyThaoTacAppService _audilogService;
        public BookingAppService(
            IRepository<Booking, Guid> repository,
            IBookingRepository bookingRepository,
            IRepository<BookingNhanVien, Guid> bookingNhanVienRepository,
            IRepository<BookingService, Guid> bookingServiceRepository,
            IRepository<DM_KhachHang, Guid> khachHangRepository,
            IRepository<DM_HangHoa, Guid> dichVuRepository,
            IRepository<DM_DonViQuiDoi, Guid> donViQuiDoiRepository,
             IRepository<NS_NhanVien, Guid> nhanVienService,
             INhanSuRepository nhanSuRepository,
            IAppNotifier appNotifier,
              IUserRoleRepository userRoleRepository,
            INhatKyThaoTacAppService audilogService
            )
        {
            _repository = repository;
            _bookingRepository = bookingRepository;
            _bookingNhanVienRepository = bookingNhanVienRepository;
            _bookingServiceRepository = bookingServiceRepository;
            _khachHangRepository = khachHangRepository;
            _dichVuRepository = dichVuRepository;
            _donViQuiDoiRepository = donViQuiDoiRepository;
            _nhanVienService = nhanVienService;
            _nhanSuRepository = nhanSuRepository;
            _appNotifier = appNotifier;
            _userRoleRepository = userRoleRepository;
            _audilogService = audilogService;
        }

        private async Task<List<UserIdentifier>> GetUserAdmin(Guid idChiNhanh)
        {
            var nhanViens = (await _nhanSuRepository.GetAllNhanSu(new PagedNhanSuRequestDto
            {
                Filter = "",
                SkipCount = 0,
                MaxResultCount = int.MaxValue,
                IdChiNhanh = idChiNhanh,
                TenantId = AbpSession.TenantId ?? 1
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
        [HttpPost]
        public async Task<Booking> CreateOrEditBooking(CreateOrEditBookingDto data)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (checkExist != null)
            {
                return await UpdateBooking(data, checkExist);
            }
            return await CreateBooking(data);
        }
        [NonAction]
        public async Task<Booking> CreateBooking(CreateOrEditBookingDto dto)
        {
            var startTime = dto.StartTime + " " + dto.StartHours;
            var booking = ObjectMapper.Map<Booking>(dto);
            var khachHang = await _khachHangRepository.FirstOrDefaultAsync(dto.IdKhachHang);
            booking.Id = Guid.NewGuid();
            if (khachHang != null)
            {
                booking.TenKhachHang = khachHang.TenKhachHang;
                booking.SoDienThoai = khachHang.SoDienThoai;
            }
            booking.BookingCode = await _bookingRepository.FnGetBookingCode(dto.IdChiNhanh, LoaiChungTuConst.Booking);
            booking.IdChiNhanh = dto.IdChiNhanh;
            booking.StartTime = DateTime.Parse(startTime);
            booking.BookingDate = DateTime.Parse(dto.StartTime);
            booking.CreationTime = DateTime.Now;
            booking.TenantId = AbpSession.TenantId ?? 1;
            booking.CreatorUserId = AbpSession.UserId;
            booking.LoaiBooking = LoaiBookingConst.CuaHangDatChoKhach;
            booking.IsDeleted = false;
            var idDichVu = _donViQuiDoiRepository.FirstOrDefault(x => x.Id == dto.IdDonViQuiDoi).IdHangHoa;
            var dichVu = _dichVuRepository.FirstOrDefault(x => x.Id == idDichVu);
            booking.EndTime = booking.StartTime.AddMinutes(dichVu.SoPhutThucHien.Value);
            var bookingService = CreateBookingService(booking.Id, dto.IdDonViQuiDoi);
            if (dto.IdNhanVien != null && dto.IdNhanVien != Guid.Empty)
            {
                var bookingNhanVien = CreateBookingNhanVien(booking.Id, dto.IdNhanVien ?? Guid.Empty);
                _bookingNhanVienRepository.Insert(bookingNhanVien);
                var nhanVien = _nhanVienService.FirstOrDefault(x => x.Id == dto.IdNhanVien && x.IsDeleted == false && x.TrangThai == TrangThaiNhanVienConst.Ranh);
                nhanVien.TrangThai = TrangThaiNhanVienConst.Ban;
                _nhanVienService.Update(nhanVien);
            }
            _repository.Insert(booking);
            _bookingServiceRepository.Insert(bookingService);
            var listUserRole = await _userRoleRepository.GetListUser_havePermission(booking.TenantId, booking.IdChiNhanh ?? Guid.Empty, "Pages.Notifications.Booking");
            var listUser = ObjectMapper.Map<List<UserIdentifier>>(listUserRole);
            string mess = "Khách hàng: " + booking.TenKhachHang + "(" + booking.SoDienThoai + ")" + " đã đặt lịch hẹn làm dịch vụ : " + dichVu.TenHangHoa + " vào " + booking.BookingDate.ToString("dd/MM/yyyy") + " " + booking.StartTime.ToString("HH:mm");
            var notificationData = NewMessageNotification(mess);
            await _appNotifier.SendMessageAsync(TrangThaiBookingConst.AddNewBooking, notificationData, listUser, severity: NotificationSeverity.Info);

            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Lịch hẹn";
            nhatKyThaoTacDto.NoiDung = "Tạo mới lịch hẹn";
            nhatKyThaoTacDto.NoiDungChiTiet = "<div>Tạo mới lịch hẹn: " + mess + "</div>";
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);

            return booking;
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
        [NonAction]
        public async Task<Booking> UpdateBooking(CreateOrEditBookingDto dto, Booking findBooking)
        {
            var startTime = dto.StartTime + " " + dto.StartHours;
            var khachHang = await _khachHangRepository.FirstOrDefaultAsync(dto.IdKhachHang);
            findBooking.IdKhachHang = dto.IdKhachHang;
            if (khachHang != null)
            {
                findBooking.TenKhachHang = khachHang.TenKhachHang;
                findBooking.SoDienThoai = khachHang.SoDienThoai;
            }

            if (string.IsNullOrEmpty(dto.BookingCode))
            {
                findBooking.BookingCode = await _bookingRepository.FnGetBookingCode(dto.IdChiNhanh, LoaiChungTuConst.Booking);
            }
            else
            {
                findBooking.BookingCode = dto.BookingCode;
            }
            findBooking.StartTime = DateTime.Parse(startTime);
            findBooking.BookingDate = DateTime.Parse(dto.StartTime);
            findBooking.GhiChu = dto.GhiChu;
            findBooking.TrangThai = dto.TrangThai;
            findBooking.LastModificationTime = DateTime.Now;
            findBooking.LastModifierUserId = AbpSession.UserId;
            var dichVu = await _donViQuiDoiRepository.GetAllIncluding(x => x.DM_HangHoa).Where(x => x.Id == dto.IdDonViQuiDoi).FirstOrDefaultAsync();
            string tenDichVu = "";
            if (dichVu != null && dichVu.DM_HangHoa != null)
            {
                tenDichVu = dichVu.DM_HangHoa.TenHangHoa;
                findBooking.EndTime = findBooking.StartTime.AddMinutes(dichVu.DM_HangHoa.SoPhutThucHien ?? 0);
            }

            await _repository.UpdateAsync(findBooking);
            if (dto.IdNhanVien != null && dto.IdNhanVien != Guid.Empty)
            {
                await UpdateBookingNhanVien(dto.Id, dto.IdNhanVien ?? Guid.Empty);
            }
            await UpdateBookingService(dto.Id, dto.IdDonViQuiDoi);
            var listUserRole = await _userRoleRepository.GetListUser_havePermission(findBooking.TenantId, findBooking.IdChiNhanh ?? Guid.Empty, "Pages.Notifications.Booking");
            var listUser = ObjectMapper.Map<List<UserIdentifier>>(listUserRole);
            string mess = "Khách hàng: " + findBooking.TenKhachHang + "(" + findBooking.SoDienThoai + ")" + " đã thay đổi thông tin lịch hẹn thành làm dịch vụ : " + tenDichVu + " vào " + findBooking.BookingDate.ToString("dd/MM/yyyy") + " " + findBooking.StartTime.ToString("HH:mm");
            var notificationData = NewMessageNotification(mess);
            await _appNotifier.SendMessageAsync(TrangThaiBookingConst.AddNewBooking, notificationData, listUser, severity: NotificationSeverity.Info);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Lịch hẹn";
            nhatKyThaoTacDto.NoiDung = "Cập nhật lịch hẹn";
            nhatKyThaoTacDto.NoiDungChiTiet = "<div> Cập nhật lịch hẹn: " + mess + "</div>";
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return findBooking;

        }
        [NonAction]
        public async Task UpdateBookingService(Guid idBooking, Guid idDichVuQuiDoi)
        {
            var bookingService = await _bookingServiceRepository.FirstOrDefaultAsync(x => x.IdBooking == idBooking && x.IsDeleted == false);

            bookingService.IdDonViQuiDoi = idDichVuQuiDoi;
            bookingService.LastModificationTime = DateTime.Now;
            bookingService.LastModifierUserId = AbpSession.UserId;
            await _bookingServiceRepository.UpdateAsync(bookingService);

        }
        [NonAction]
        public async Task<BookingNhanVien> UpdateBookingNhanVien(Guid idBooking, Guid idNhanVien)
        {
            var result = await _bookingNhanVienRepository.FirstOrDefaultAsync(x => x.IdBooking == idBooking && x.IsDeleted == false);
            try
            {
                result.IdNhanVien = idNhanVien;
                result.LastModificationTime = DateTime.Now;
                result.LastModifierUserId = AbpSession.UserId;
                await _bookingNhanVienRepository.UpdateAsync(result);
            }
            catch (Exception)
            {
                result = new BookingNhanVien();
            }
            return result;
        }

        [HttpPost]
        public async Task<ExecuteResultDto> UpdateTrangThaiBooking(Guid idBooking, int trangThai = 1)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                var objUp = await _repository.FirstOrDefaultAsync(idBooking);
                if (objUp == null)
                {
                    result.Status = "error";
                    result.Message = "Cập nhật trạng thái lịch hẹn thất bại";
                }
                objUp.TrangThai = trangThai;
                objUp.LastModificationTime = DateTime.Now;
                objUp.LastModifierUserId = AbpSession.UserId;
                if (trangThai == TrangThaiBookingConst.Huy || trangThai == TrangThaiBookingConst.HoanThanh)
                {
                    var bookingNhanVien = _bookingNhanVienRepository.FirstOrDefault(x => x.IdBooking == objUp.Id);
                    if (bookingNhanVien != null)
                    {
                        var nhanVien = _nhanVienService.FirstOrDefault(x => x.Id == bookingNhanVien.IdNhanVien);
                        nhanVien.TrangThai = TrangThaiNhanVienConst.Ranh;
                        _nhanVienService.Update(nhanVien);
                    }
                }
                await _repository.UpdateAsync(objUp);
                result.Status = "success";
                result.Message = "Cập nhật trạng thái lịch hẹn thành công";
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Đặt lịch";
                nhatKyThaoTacDto.NoiDung = "Cập nhật trạng thái lịch hẹn";
                nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật trạng thái lịch hẹn " + objUp.TenKhachHang + " - " + objUp.SoDienThoai + " Ngày: " + objUp.BookingDate.ToString("dd/MM/yyyy HH:mm"); ;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            }
            catch (Exception ex)
            {
                result.Status = "error";
                result.Message = "Cập nhật trạng thái lịch hẹn thất bại";
                result.Detail = ex.Message;
            }
            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_Booking_Delete)]
        [HttpPost]
        public async Task<bool> DeleteBooking(Guid id)
        {
            bool result = false;
            var findBooking = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (findBooking != null)
            {
                //delete all bookingservice
                var bookingService = await _bookingServiceRepository.GetAll().Where(x => x.IsDeleted == false && x.IdBooking == id).ToListAsync();
                if (bookingService != null && bookingService.Count > 0)
                {
                    foreach (var item in bookingService)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _bookingServiceRepository.UpdateAsync(item);
                    }
                }
                //delete all bookingNhanVien
                var bookingNhanVien = await _bookingNhanVienRepository.GetAll().Where(x => x.IsDeleted == false && x.IdBooking == id).ToListAsync();
                if (bookingNhanVien != null && bookingNhanVien.Count > 0)
                {
                    foreach (var item in bookingNhanVien)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _bookingNhanVienRepository.UpdateAsync(item);
                    }
                }
                //findBooking.TrangThai = TrangThaiBookingConst.Huy;// khônmg cập nhật trạng thái = Hủy: chỉ dùng khi khách Hủy lịch
                findBooking.DeletionTime = DateTime.Now;
                findBooking.DeleterUserId = AbpSession.UserId;
                await _repository.DeleteAsync(findBooking);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Đặt lịch";
                nhatKyThaoTacDto.NoiDung = "Xóa lịch hẹn";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa lịch hẹn " + findBooking.TenKhachHang + " - " + findBooking.SoDienThoai + " Ngày: " + findBooking.BookingDate.ToString("dd/MM/yyyy HH:mm");
                result = true;
            }
            return result;
        }
        public async Task<List<BookingGetAllItemDto>> GetAll(PagedBookingResultRequestDto input)
        {
            List<BookingGetAllItemDto> result = new List<BookingGetAllItemDto>();
            int tenantId = AbpSession.TenantId ?? 1;
            DateTime date = input.DateSelected;
            if (input.TypeView.ToLower() == "week")
            {
                DateTime firstDayOfWeek = date.AddDays(DayOfWeek.Monday - date.DayOfWeek);
                DateTime lastDayOfWeek = date.AddDays(DayOfWeek.Sunday - date.DayOfWeek + 7);
                DateTime timeFrom = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0);
                DateTime timeTo = new DateTime(lastDayOfWeek.Year, lastDayOfWeek.Month, lastDayOfWeek.Day, 23, 59, 59);
                result = await _bookingRepository.GetAllBooking(input, tenantId, timeFrom, timeTo);
            }
            else if (input.TypeView.ToLower() == "day")
            {
                DateTime timeFrom = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                DateTime timeTo = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                result = await _bookingRepository.GetAllBooking(input, tenantId, timeFrom, timeTo);
            }
            else
            {
                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                result = await _bookingRepository.GetAllBooking(input, tenantId, firstDayOfMonth, lastDayOfMonth);
            }

            return result;
        }

        public async Task<List<BookingDetailOfCustometDto>> GetKhachHang_Booking(BookingRequestDto input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            List<BookingDetailDto> data = await _bookingRepository.GetKhachHang_Booking(input);
            var dtGr = data.GroupBy(x => new
            {
                x.IdBooking,
                x.IdKhachHang,
                x.MaKhachHang,
                x.TenKhachHang,
                x.SoDienThoai,
                x.Avatar,
                x.BookingDate,
                x.StartTime,
                x.EndTime,
                x.TrangThai,
                x.TxtTrangThaiBook
            }).Select(x => new BookingDetailOfCustometDto
            {
                IdBooking = x.Key.IdBooking,
                IdKhachHang = x.Key.IdKhachHang,
                MaKhachHang = x.Key.MaKhachHang,
                TenKhachHang = x.Key.TenKhachHang,
                SoDienThoai = x.Key.SoDienThoai,
                Avatar = x.Key.Avatar,
                BookingDate = x.Key.BookingDate,
                StartTime = x.Key.StartTime,
                EndTime = x.Key.EndTime,
                TrangThai = x.Key.TrangThai,
                TxtTrangThaiBook = x.Key.TxtTrangThaiBook,
                Details = x.ToList(),
            }).ToList();
            return dtGr;
        }
        [HttpPost]
        public async Task<List<BookingDetailOfCustometDto>> GetInforBooking_byID(List<Guid> arrIdBooking)
        {
            List<BookingDetailDto> data = await _bookingRepository.GetInforBooking_byID(arrIdBooking);
            var dtGr = data.GroupBy(x => new
            {
                x.IdBooking,
                x.IdKhachHang,
                x.MaKhachHang,
                x.TenKhachHang,
                x.SoDienThoai,
                x.BookingDate,
                x.StartTime,
                x.EndTime,
                x.TrangThai,
                x.TxtTrangThaiBook
            }).Select(x => new BookingDetailOfCustometDto
            {
                IdBooking = x.Key.IdBooking,
                IdKhachHang = x.Key.IdKhachHang,
                MaKhachHang = x.Key.MaKhachHang,
                TenKhachHang = x.Key.TenKhachHang,
                SoDienThoai = x.Key.SoDienThoai,
                BookingDate = x.Key.BookingDate,
                StartTime = x.Key.StartTime,
                EndTime = x.Key.EndTime,
                TrangThai = x.Key.TrangThai,
                TxtTrangThaiBook = x.Key.TxtTrangThaiBook,
                Details = x.ToList(),
            }).ToList();
            return dtGr;
        }
        public async Task<Booking> GetDetail(Guid id)
        {
            Booking result = new Booking();
            var booking = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (booking != null)
            {
                result = booking;
            }
            return result;
        }
        public async Task<BookingInfoDto> GetBookingInfo(Guid id)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            return await _bookingRepository.GetBookingInfo(id, tenantId);
        }
        public async Task<CreateOrEditBookingDto> GetForEdit(Guid id)
        {
            CreateOrEditBookingDto result = new CreateOrEditBookingDto();
            var booking = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (booking != null)
            {
                result = ObjectMapper.Map<CreateOrEditBookingDto>(booking);
                result.StartHours = booking.StartTime.ToString("HH:mm");
                result.StartTime = DateTime.Parse(result.StartTime).ToString("yyyy-MM-dd");
                var bookingService = await _bookingServiceRepository.FirstOrDefaultAsync(x => x.IdBooking == id && x.IsDeleted == false);
                if (bookingService != null)
                {
                    result.IdDonViQuiDoi = bookingService.IdDonViQuiDoi;
                }
                var bookingNhanVien = await _bookingNhanVienRepository.FirstOrDefaultAsync(x => x.IdBooking == id && x.IsDeleted == false);
                if (bookingNhanVien != null)
                {
                    result.IdNhanVien = bookingNhanVien.IdNhanVien;
                }
            }
            return result;
        }
        public async Task<bool> CancelBooking(Guid id)
        {
            bool result = false;
            var findBooking = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (findBooking != null)
            {
                findBooking.LastModificationTime = DateTime.Now;
                findBooking.LastModifierUserId = AbpSession.UserId;
                findBooking.TrangThai = TrangThaiBookingConst.Huy;
                await _repository.UpdateAsync(findBooking);
                var bookingNhanVien = _bookingNhanVienRepository.FirstOrDefault(x => x.IdBooking == findBooking.Id);
                if (bookingNhanVien != null)
                {
                    var nhanVien = _nhanVienService.FirstOrDefault(x => x.Id == bookingNhanVien.IdNhanVien);
                    nhanVien.TrangThai = TrangThaiNhanVienConst.Ranh;
                    _nhanVienService.Update(nhanVien);
                }

                result = true;
            }
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Đặt lịch";
            nhatKyThaoTacDto.NoiDung = "Hủy lịch hẹn";
            nhatKyThaoTacDto.NoiDungChiTiet = "Hủy lịch hẹn " + findBooking.TenKhachHang + " - " + findBooking.SoDienThoai + " Ngày: " + findBooking.BookingDate.ToString("dd/MM/yyyy HH:mm"); ;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        private LocalizableMessageNotificationData NewMessageNotification(string mess)
        {
            return new LocalizableMessageNotificationData(
                        new LocalizableString(
                            mess,
                            "LuckyBeauty"
                        )
                    );
        }
    }
}
