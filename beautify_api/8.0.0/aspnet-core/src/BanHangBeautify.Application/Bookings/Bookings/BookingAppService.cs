using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings
{
    [AbpAuthorize(PermissionNames.Pages_Booking)]
    public class BookingAppService : SPAAppServiceBase
    {
        private readonly IRepository<Booking, Guid> _repository;    
        private readonly IRepository<BookingNhanVien, Guid> _bookingNhanVienRepository;
        private readonly IRepository<BookingService, Guid> _bookingServiceRepository;
        private readonly IRepository<DM_KhachHang, Guid> _khachHangRepository;
        private readonly IRepository<DM_HangHoa, Guid> _dichVuRepository;
        public BookingAppService(
            IRepository<Booking, Guid> repository, 
            IRepository<BookingNhanVien,Guid> bookingNhanVienRepository,
            IRepository<BookingService, Guid> bookingServiceRepository, 
            IRepository<DM_KhachHang, Guid> khachHangRepository,
            IRepository<DM_HangHoa,Guid> dichVuRepository)
        {
            _repository = repository;
            _bookingNhanVienRepository = bookingNhanVienRepository;
            _bookingServiceRepository = bookingServiceRepository;
            _khachHangRepository = khachHangRepository;
            _dichVuRepository = dichVuRepository;
        }
        public async Task<Booking> CreateBooking(CreateBookingDto dto)
        {
            var startTime = dto.StartTime + " " + dto.StartHours;
            var booking = ObjectMapper.Map<Booking>(dto);
            var khachHang =await _khachHangRepository.FirstOrDefaultAsync(dto.IdKhachHang);
            booking.Id = Guid.NewGuid();
            if(khachHang!=null)
            {
                booking.TenKhachHang = khachHang.TenKhachHang;
                booking.SoDienThoai = khachHang.SoDienThoai;
            }
            booking.StartTime = DateTime.Parse(startTime);
            booking.EndTime = booking.StartTime;
            booking.BookingDate = DateTime.Now;
            booking.CreationTime = DateTime.Now;
            booking.TenantId = AbpSession.TenantId ?? 1;
            booking.CreatorUserId = AbpSession.UserId;
            booking.IsDeleted = false;
            var bookingService = CreateBookingService(booking.Id, dto.IdDonViQuiDoi);
            var bookingNhanVien = CreateBookingNhanVien(booking.Id, dto.IdNhanVien);
            await _repository.InsertAsync(booking);
            await _bookingNhanVienRepository.InsertAsync(bookingNhanVien);
            await _bookingServiceRepository.InsertAsync(bookingService);
            return booking;
        }
        [NonAction]
        public BookingService CreateBookingService(Guid idBooking,Guid idDichVuQuiDoi)
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
                result.CreationTime = DateTime.Now;
            }
            catch (Exception)
            {
                result = new BookingNhanVien();
            }
            return result;
        }
        public async Task<Booking> UpdateBooking(UpdateBookingDto dto)
        {
            var findBooking = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findBooking != null)
            {
                var booking = ObjectMapper.Map<Booking>(dto);
                booking.LastModificationTime = DateTime.Now;
                booking.LastModifierUserId = AbpSession.UserId;
                await _repository.UpdateAsync(booking);
                return booking;
            }
            return new Booking();

        }
        [AbpAuthorize(PermissionNames.Pages_Booking_Delete)]
        public async Task<bool> DeleteBooking(Guid id)
        {
            bool result = false;
            var findBooking = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (findBooking != null)
            {
                findBooking.DeletionTime = DateTime.Now;
                findBooking.DeleterUserId = AbpSession.UserId;
                await _repository.DeleteAsync(findBooking);
                result = true;
            }
            return result;
        }
        public async Task<List<BookingDto>> GetAll(PagedBookingResultRequestDto input)
        {
            List<BookingDto> result = new List<BookingDto>();
            var bookings = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            foreach (var item in bookings)
            {
                var idDichVus = await _bookingServiceRepository.GetAll().Where(x=>x.IdBooking==item.Id).Select(x=>x.DM_DonViQuiDoi.IdHangHoa).ToListAsync();
                var dichVus = await _dichVuRepository.GetAll().Where(x => idDichVus.Contains(x.Id)).ToListAsync();
                BookingDto rdo = new BookingDto();
                rdo.StartTime = item.StartTime; rdo.EndTime = item.StartTime.AddMinutes(60);
                var tenHangHoas = dichVus.Select(x => x.TenHangHoa).ToList();
                rdo.NoiDung = item.TenKhachHang + " - " + string.Join("-", tenHangHoas);
                var time = dichVus.Select(x=>x.SoPhutThucHien).ToList();
                float totalTimeService = 0;
                foreach (var i in time)
                {
                    totalTimeService += i.Value;
                }
                rdo.EndTime = rdo.StartTime.AddMinutes(totalTimeService);
                rdo.Color = dichVus.Count==1 ? dichVus[0].Color??"" : "";
                result.Add(rdo);
            }
            return result;
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
}
