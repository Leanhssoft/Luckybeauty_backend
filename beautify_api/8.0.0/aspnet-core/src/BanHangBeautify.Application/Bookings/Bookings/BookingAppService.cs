using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Bookings.Bookings.BookingRepository;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<DM_DonViQuiDoi, Guid> _donViQuiDoiRepository;
        private readonly IHubContext<BookingHub> _bookingHubContext;
        public BookingAppService(
            IRepository<Booking, Guid> repository,
            IBookingRepository bookingRepository,
            IRepository<BookingNhanVien, Guid> bookingNhanVienRepository,
            IRepository<BookingService, Guid> bookingServiceRepository,
            IRepository<DM_KhachHang, Guid> khachHangRepository,
            IRepository<DM_HangHoa, Guid> dichVuRepository,
            IRepository<DM_DonViQuiDoi, Guid> donViQuiDoiRepository,
            IHubContext<BookingHub> bookingHubContext)
        {
            _repository = repository;
            _bookingRepository = bookingRepository;
            _bookingNhanVienRepository = bookingNhanVienRepository;
            _bookingServiceRepository = bookingServiceRepository;
            _khachHangRepository = khachHangRepository;
            _dichVuRepository = dichVuRepository;
            _donViQuiDoiRepository = donViQuiDoiRepository;
            _bookingHubContext = bookingHubContext;
        }

        public async Task<Booking> CreateBooking(CreateBookingDto dto)
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
            booking.IdChiNhanh = dto.IdChiNhanh;
            booking.StartTime = DateTime.Parse(startTime);
            booking.BookingDate = DateTime.Parse(dto.StartTime);
            booking.CreationTime = DateTime.Now;
            booking.TenantId = AbpSession.TenantId ?? 1;
            booking.CreatorUserId = AbpSession.UserId;
            booking.IsDeleted = false;
            var idDichVu = _donViQuiDoiRepository.FirstOrDefault(x => x.Id == dto.IdDonViQuiDoi).IdHangHoa;
            var dichVu = _dichVuRepository.FirstOrDefault(x => x.Id == idDichVu);
            booking.EndTime = booking.StartTime.AddMinutes(dichVu.SoPhutThucHien.Value);
            var bookingService = CreateBookingService(booking.Id, dto.IdDonViQuiDoi);
            var bookingNhanVien = CreateBookingNhanVien(booking.Id, dto.IdNhanVien);
            _repository.Insert(booking);
            _bookingNhanVienRepository.Insert(bookingNhanVien);
            _bookingServiceRepository.Insert(bookingService);
            //await UpdateEndTimeBooking(booking);
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
        [HttpPost]
        public async Task<Booking> UpdateBooking(UpdateBookingDto dto)
        {
            var findBooking = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findBooking != null)
            {
                findBooking.SoDienThoai = string.IsNullOrEmpty(dto.SoDienThoai) ? findBooking.SoDienThoai : dto.SoDienThoai;
                findBooking.StartTime = dto.StartTime;
                findBooking.GhiChu = dto.GhiChu;
                findBooking.LoaiBooking = dto.LoaiBooking;
                findBooking.TenKhachHang = string.IsNullOrEmpty(dto.TenKhachHang) ? findBooking.TenKhachHang : dto.TenKhachHang;
                findBooking.TrangThai = dto.TrangThai;
                findBooking.LastModificationTime = DateTime.Now;
                findBooking.LastModifierUserId = AbpSession.UserId;
                await _repository.UpdateAsync(findBooking);
                await UpdateBookingNhanVien(dto.Id, dto.IdNhanVien);
                await UpdateBookingService(dto.Id, dto.IdDonViQuiDoi);
                await UpdateEndTimeBooking(findBooking);
                return findBooking;
            }
            return new Booking();

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
        [NonAction]
        public async Task UpdateEndTimeBooking(Booking rdo)
        {
            var idDichVus = await _bookingServiceRepository.GetAll().Include(x => x.DM_DonViQuiDoi).Where(x => x.IdBooking == rdo.Id && x.IsDeleted == false).Select(x => x.DM_DonViQuiDoi.IdHangHoa).ToListAsync();
            var dichVus = await _dichVuRepository.GetAll().Where(x => idDichVus.Contains(x.Id)).ToListAsync();
            var time = dichVus.Select(x => x.SoPhutThucHien).ToList();
            float totalTimeService = 0;
            foreach (var i in time)
            {
                totalTimeService += i.Value;
            }
            rdo.EndTime = rdo.StartTime.AddMinutes(totalTimeService);
            await _repository.UpdateAsync(rdo);
        }

        [HttpPost]
        public async Task<string> UpdateTrangThaiBooking(Guid idBooking, int trangThai = 1)
        {
            try
            {
                var objUp = await _repository.FirstOrDefaultAsync(idBooking);
                if (objUp == null)
                {
                    return "data null";
                }
                objUp.TrangThai = trangThai;
                if (trangThai == 0)
                {
                    objUp.DeleterUserId = AbpSession.UserId;
                    objUp.DeletionTime = DateTime.Now;
                    objUp.IsDeleted = true;
                }
                await _repository.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException;
            }
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
                findBooking.DeletionTime = DateTime.Now;
                findBooking.DeleterUserId = AbpSession.UserId;
                await _repository.DeleteAsync(findBooking);

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

            await _bookingHubContext.Clients.All.SendAsync("BookingDataUpdated", result);
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
        public async Task<List<BookingDetailOfCustometDto>> GetInforBooking_byID(Guid idBooking)
        {
            List<BookingDetailDto> data = await _bookingRepository.GetInforBooking_byID(idBooking);
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

        public async Task<UpdateBookingDto> GetForEdit(Guid id)
        {
            UpdateBookingDto result = new UpdateBookingDto();
            var booking = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (booking != null)
            {
                result = ObjectMapper.Map<UpdateBookingDto>(booking);
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
                findBooking.TrangThai = 0;
                await _repository.UpdateAsync(findBooking);
                result = true;
            }
            return result;
        }
    }
    public class BookingHub : Hub
    {

    }
}
