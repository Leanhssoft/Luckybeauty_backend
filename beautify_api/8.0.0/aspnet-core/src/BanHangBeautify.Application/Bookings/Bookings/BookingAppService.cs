using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Entities;
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
        public BookingAppService(IRepository<Booking, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Booking> CreateBooking(CreateBookingDto dto)
        {
            var booking = ObjectMapper.Map<Booking>(dto);
            booking.Id = Guid.NewGuid();
            booking.CreationTime = DateTime.Now;
            booking.TenantId = AbpSession.TenantId??1;
            booking.CreatorUserId = AbpSession.UserId;
            booking.IsDeleted = false;
            await _repository.InsertAsync(booking);
            return booking;
        }
        public async Task<Booking> UpdateBooking(UpdateBookingDto dto)
        {
            var findBooking =await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (findBooking != null)
            {
                var booking = ObjectMapper.Map<Booking>(dto);
                booking.LastModificationTime= DateTime.Now;
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
            var findBooking =await _repository.FirstOrDefaultAsync(x => x.Id == id);
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
            var bookings = await _repository.GetAll().Where(x=>x.TenantId==(AbpSession.TenantId??1) && x.IsDeleted==false).ToListAsync();
            foreach (var item in bookings)
            {
                BookingDto rdo = new BookingDto();
                rdo.StartTime = item.StartTime; rdo.EndTime = item.StartTime.AddMinutes(60);
                rdo.NoiDung = item.TenKhachHang;
                rdo.Color = "";
                result.Add(rdo);
            }
            return result;
        }
        public async Task<Booking> GetDetail(Guid id)
        {
            Booking result = new Booking();
            var booking =await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (booking!=null)
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
