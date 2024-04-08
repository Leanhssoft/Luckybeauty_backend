using Abp.EntityFrameworkCore;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.SMS.CaiDatNhacNho;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.MauTinSMS
{
    public class MauTinSMSRepository : SPARepositoryBase<SMS_Template, Guid>, IMauTinSMSRepository
    {
        public MauTinSMSRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<MauTinSMSDto> GetMauTinSMS_byId(Guid id)
        {
            try
            {
                var dbContext = GetDbContext();
                var data = dbContext.Set<SMS_Template>().Where(x => x.Id == id).Select(x => new MauTinSMSDto
                {
                    Id = x.Id,
                    IdLoaiTin = x.IdLoaiTin ?? 1,
                    TenantId = x.TenantId,
                    TenMauTin = x.TenMauTin,
                    NoiDungTinMau = x.NoiDungTinMau,
                    LaMacDinh = x.LaMacDinh,
                    TrangThai = x.TrangThai
                }).ToList();
                return data.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        } 
    }
}
