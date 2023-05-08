using Abp.Domain.Repositories;
using BanHangBeautify.Checkin.Repository;
using BanHangBeautify.CheckIn.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin
{
    public class KHCheckInService : SPAAppServiceBase
    {
        private readonly IRepository<KH_CheckIn, Guid> _khCheckIn;
        private readonly IKHCheckInRespository _repository;

        public KHCheckInService(IRepository<KH_CheckIn, Guid> khCheckIn,
           IKHCheckInRespository checkInRepo
           )
        {
            _khCheckIn = khCheckIn;
            _repository = checkInRepo;
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
        public async Task<string> UpdatetCustomerCheckIn(KHCheckInDto dto)
        {
            if (dto == null) { return "Data null"; };
            KH_CheckIn objUp = await _khCheckIn.FirstOrDefaultAsync(dto.Id);
            if (objUp == null)
            {
                return "object null";
            }
            objUp.IdChiNhanh = dto.IdChiNhanh;
            objUp.IdKhachHang = dto.IdKhachHang;
            objUp.IdBooking = dto.IdBooking;
            objUp.DateTimeCheckIn = dto.DateTimeCheckIn;
            objUp.GhiChu = dto.GhiChu;
            objUp.LastModifierUserId = AbpSession.UserId;
            objUp.LastModificationTime = DateTime.Now;
            await _khCheckIn.UpdateAsync(objUp);
            return string.Empty;
        }
    }
}
