﻿using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DataExporting.Checkin.Dto;
using BanHangBeautify.DataExporting.Checkin.Repository;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DataExporting.Checkin
{
    //[AbpAuthorize(PermissionNames.Pages_CheckIn)]
    public class CheckInAppService : SPAAppServiceBase
    {
        private readonly IRepository<KH_CheckIn, Guid> _khCheckIn;
        private readonly IRepository<Booking_CheckIn_HoaDon, Guid> _checkInHoaDon;
        private readonly IKHCheckInRespository _repository;

        public CheckInAppService(IRepository<KH_CheckIn, Guid> khCheckIn,
            IRepository<Booking_CheckIn_HoaDon, Guid> checkInHoaDon,
           IKHCheckInRespository checkInRepo
           )
        {
            _khCheckIn = khCheckIn;
            _checkInHoaDon = checkInHoaDon;
            _repository = checkInRepo;
        }

        public async Task<bool> CheckExistCusCheckin(Guid idCus, Guid? idCheckIn = null)
        {
            if (idCheckIn != null && idCheckIn != Guid.Empty)
            {
                var lst = await _khCheckIn.GetAllListAsync(x => x.Id != idCheckIn && x.IdKhachHang == idCus && x.TrangThai == 1);
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _khCheckIn.GetAllListAsync(x => x.IdKhachHang == idCus && x.TrangThai == 1);
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
                await _khCheckIn.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException;
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
            var result = ObjectMapper.Map<CheckInHoaDonDto>(objNew);
            return result;
        }

        #endregion
    }
}