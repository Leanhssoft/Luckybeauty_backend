using Abp.Domain.Repositories;
using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.Checkin.Repository;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin
{
    //[AbpAuthorize(PermissionNames.Pages_CheckIn)]
    public class CheckInAppService : SPAAppServiceBase
    {
        private readonly IRepository<KH_CheckIn, Guid> _khCheckIn;
        private readonly IRepository<Booking_CheckIn_HoaDon, Guid> _checkInHoaDon;
        private readonly IKHCheckInRespository _repository;
        private readonly IRepository<Booking, Guid> _bookingRespository;
        public CheckInAppService(IRepository<KH_CheckIn, Guid> khCheckIn,
            IRepository<Booking_CheckIn_HoaDon, Guid> checkInHoaDon,
            IRepository<Booking, Guid> bookingRespository,
           IKHCheckInRespository checkInRepo
           )
        {
            _khCheckIn = khCheckIn;
            _checkInHoaDon = checkInHoaDon;
            _repository = checkInRepo;
            _bookingRespository = bookingRespository;
        }

        public async Task<bool> CheckExistCusCheckin(Guid idCus, Guid? idCheckIn = null)
        {
            if (idCheckIn != null && idCheckIn != Guid.Empty)
            {
                var lst = await _khCheckIn.GetAllListAsync(x => x.Id != idCheckIn && x.IdKhachHang == idCus
                && (x.TrangThai == TrangThaiCheckin.WAITING || x.TrangThai == TrangThaiCheckin.DOING));
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _khCheckIn.GetAllListAsync(x => x.IdKhachHang == idCus
                && (x.TrangThai == TrangThaiCheckin.WAITING || x.TrangThai == TrangThaiCheckin.DOING));
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public KHCheckInDto InsertCustomerCheckIn(KHCheckInDto dto)
        {
            if (dto == null) { return new KHCheckInDto(); };
            KH_CheckIn objNew = ObjectMapper.Map<KH_CheckIn>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            _khCheckIn.InsertAsync(objNew);
            var result = ObjectMapper.Map<KHCheckInDto>(objNew);
            return result;
        }
        [HttpPost]
        public async Task<string> UpdateCustomerCheckIn(KHCheckInDto dto)
        {
            if (dto == null) { return "Data null"; };
            KH_CheckIn objUp = await _khCheckIn.FirstOrDefaultAsync(dto.Id);
            if (objUp == null)
            {
                return "object null";
            }
            objUp.IdChiNhanh = dto.IdChiNhanh;
            objUp.IdKhachHang = dto.IdKhachHang;
            objUp.DateTimeCheckIn = dto.DateTimeCheckIn;
            objUp.GhiChu = dto.GhiChu;
            objUp.LastModifierUserId = AbpSession.UserId;
            objUp.LastModificationTime = DateTime.Now;
            await _khCheckIn.UpdateAsync(objUp);
            return string.Empty;
        }
        [HttpPost]
        public async Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking(PagedKhachHangResultRequestDto input)
        {
            try
            {
                return await _repository.GetListCustomerChecking(input, AbpSession.TenantId ?? 1);
            }
            catch (Exception)
            {
                return new List<PageKhachHangCheckingDto>();
            }
        }

        [HttpPost]
        public async Task<string> UpdateTrangThaiCheckin(Guid idCheckIn, int trangThai = 1)
        {
            try
            {
                var objUp = await _khCheckIn.FirstOrDefaultAsync(idCheckIn);
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

                    // xóa checkin --> xóa luon trong bảng _checkInHoaDon
                    _checkInHoaDon.GetAll().Where(x => x.IdCheckIn == idCheckIn).ToList().ForEach(x => x.IsDeleted = true);
                }
                await _khCheckIn.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException;
            }
        }

        [HttpGet]
        public async Task<KHCheckInDto> GetInforCheckIn_byId(Guid idCheckIn)
        {
            try
            {
                KH_CheckIn obj = await _khCheckIn.FirstOrDefaultAsync(idCheckIn);
                return ObjectMapper.Map<KHCheckInDto>(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<IEnumerable<Guid>> GetArrIdChecking_fromIdBooking(Guid idBooking)
        {
            try
            {
                var arrIdCheckin = _checkInHoaDon.GetAllList().Where(x => x.IdBooking == idBooking).Select(x => x.IdCheckIn);
                // only get if khachhang is checking
                var lstCheckin = _khCheckIn.GetAllList(x => arrIdCheckin.Contains(x.Id)
                  && (x.TrangThai == TrangThaiCheckin.DOING || x.TrangThai == TrangThaiCheckin.WAITING)).Select(x => x.Id);
                return lstCheckin;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region checkin + hoadon
        public async Task<CheckInHoaDonDto> InsertCheckInHoaDon(CheckInHoaDonDto dto)
        {
            if (dto == null) { return new CheckInHoaDonDto(); };
            Booking_CheckIn_HoaDon objNew = ObjectMapper.Map<Booking_CheckIn_HoaDon>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            await _checkInHoaDon.InsertAsync(objNew);
            var booking = await _bookingRespository.FirstOrDefaultAsync(x => x.Id == dto.IdBooking);
            if (booking != null)
            {
                booking.TrangThai = TrangThaiBookingConst.CheckIn;
                await _bookingRespository.UpdateAsync(booking);
            }
            var result = ObjectMapper.Map<CheckInHoaDonDto>(objNew);
            return result;
        }
        /// <summary>
        ///  used to save hoadon
        /// </summary>
        /// <param name="idCheckIn"></param>
        /// <param name="idHoaDon"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> Update_IdHoaDon_toCheckInHoaDon(Guid idCheckIn, Guid idHoaDon)
        {
            var listUp = await _checkInHoaDon.GetAll().Where(x => x.IdCheckIn == idCheckIn).ToListAsync();

            if (listUp != null && listUp.Count > 0)
            {

                var objUp = listUp.FirstOrDefault();
                var booking = await _bookingRespository.FirstOrDefaultAsync(x => x.Id == objUp.IdBooking);
                if (booking != null)
                {
                    booking.TrangThai = TrangThaiBookingConst.HoanThanh;
                    await _bookingRespository.UpdateAsync(booking);
                }
                objUp.IdHoaDon = idHoaDon;
                await _checkInHoaDon.UpdateAsync(objUp);
                return true;
            }
            return false;
        }

        #endregion
    }
}
