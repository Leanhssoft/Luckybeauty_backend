using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Bookings.BookingColor.Dto;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.BookingColor
{
    [AbpAuthorize(PermissionNames.Pages_Booking_Color)]
    public class BookingColorAppService : SPAAppServiceBase
    {
        private readonly IRepository<Booking_Color, Guid> _repository;
        public BookingColorAppService(IRepository<Booking_Color, Guid> repository)
        {
            this._repository = repository;
        }
        public async Task<BookingColorDto> CreateOrEdit(CreateOrEditBookingColor input) {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<BookingColorDto> Create(CreateOrEditBookingColor input)
        {
            BookingColorDto result = new BookingColorDto();
            Booking_Color data = new Booking_Color();
            data.Id = Guid.NewGuid();
            data.TrangThai = input.TrangThai;
            data.MaMau = input.MaMau;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 0;
            await _repository.InsertAsync(data);
            result.MaMau = input.MaMau;
            result.TrangThaiBooking = input.TrangThai;
            return result;
        }
        [NonAction]
        public async Task<BookingColorDto> Update(CreateOrEditBookingColor input, Booking_Color oldData)
        {
            BookingColorDto result = new BookingColorDto();
            oldData.TrangThai = input.TrangThai;
            oldData.MaMau = input.MaMau;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result.MaMau = input.MaMau;
            result.TrangThaiBooking = input.TrangThai;
            return result;
        }
        [HttpPost]
        public async Task<BookingColorDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(data);
                return new BookingColorDto()
                {
                    MaMau = data.MaMau,
                    TrangThaiBooking = data.TrangThai
                };
            }
            return new BookingColorDto();
        }
        public async Task<CreateOrEditBookingColor> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return new CreateOrEditBookingColor()
                {
                    MaMau = data.MaMau,
                    TrangThai = data.TrangThai
                };
            }
            return new CreateOrEditBookingColor();

        }
        public async Task<PagedResultDto<Booking_Color>> GetAll(PagedRequestDto input){
            PagedResultDto<Booking_Color> result = new PagedResultDto<Booking_Color>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "": input.Keyword ;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var data = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x=>x.CreationTime).ToListAsync();
            result.TotalCount = data.Count;
            result.Items = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }

    }
}
