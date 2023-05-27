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
using System.Runtime.Intrinsics.Arm;
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
        private readonly IRepository<DM_DonViQuiDoi, Guid> _donViQuiDoiRepository;
        public BookingAppService(
            IRepository<Booking, Guid> repository,
            IRepository<BookingNhanVien, Guid> bookingNhanVienRepository,
            IRepository<BookingService, Guid> bookingServiceRepository,
            IRepository<DM_KhachHang, Guid> khachHangRepository,
            IRepository<DM_HangHoa, Guid> dichVuRepository,
            IRepository<DM_DonViQuiDoi, Guid> donViQuiDoiRepository)
        {
            _repository = repository;
            _bookingNhanVienRepository = bookingNhanVienRepository;
            _bookingServiceRepository = bookingServiceRepository;
            _khachHangRepository = khachHangRepository;
            _dichVuRepository = dichVuRepository;
            _donViQuiDoiRepository = donViQuiDoiRepository;
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
            booking.BookingDate = DateTime.Now;
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
                bookingService.CreatorUserId= AbpSession.UserId;
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
                findBooking.SoDienThoai = dto.SoDienThoai;
                findBooking.StartTime = dto.StartTime;
                findBooking.GhiChu = dto.GhiChu;
                findBooking.LoaiBooking = dto.LoaiBooking;
                findBooking.TenKhachHang = dto.TenKhachHang;
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
            var idDichVus = await _bookingServiceRepository.GetAll().Include(x=>x.DM_DonViQuiDoi).Where(x => x.IdBooking == rdo.Id && x.IsDeleted==false).Select(x => x.DM_DonViQuiDoi.IdHangHoa).ToListAsync();
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
        public async Task<List<BookingDto>> GetAll(PagedBookingResultRequestDto input)
        {
            List<BookingDto> result = new List<BookingDto>();
            var bookings = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false&& x.IdChiNhanh==input.IdChiNhanh).ToListAsync();
            foreach (var item in bookings)
            {
                var idDichVus = await _bookingServiceRepository.GetAll().Where(x => x.IdBooking == item.Id).Select(x => x.DM_DonViQuiDoi.IdHangHoa).ToListAsync();
                var dichVus = await _dichVuRepository.GetAll().Where(x => idDichVus.Contains(x.Id)).ToListAsync();
                BookingDto rdo = new BookingDto();
                rdo.Id = item.Id;
                rdo.StartTime = item.StartTime;
                rdo.EndTime = item.EndTime;
                var tenHangHoas = dichVus.Select(x => x.TenHangHoa).ToList();
                rdo.NoiDung = item.TenKhachHang + " - " + string.Join("-", tenHangHoas);
                rdo.Color = dichVus.Count == 1 ? dichVus[0].Color ?? "" : "";
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

        public async Task<UpdateBookingDto> GetForEdit(Guid id)
        {
            UpdateBookingDto result = new UpdateBookingDto();
            var booking = await _repository.FirstOrDefaultAsync(x=>x.Id==id); 
            if (booking != null) {
                result = ObjectMapper.Map<UpdateBookingDto>(booking);
                var bookingService =await _bookingServiceRepository.FirstOrDefaultAsync(x => x.IdBooking == id&&x.IsDeleted==false);
                if (bookingService!=null)
                {
                    result.IdDonViQuiDoi = bookingService.IdDonViQuiDoi;
                }
                var bookingNhanVien =await _bookingNhanVienRepository.FirstOrDefaultAsync(x => x.IdBooking == id && x.IsDeleted == false);
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
}
