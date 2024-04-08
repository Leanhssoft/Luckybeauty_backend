using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.CaiDatNhacNho
{
    internal class CaiDatNhacNhoRepository : SPARepositoryBase<SMS_CaiDat_NhacNho, Guid>, ICaiDatNhacNhoRepository
    {
        public CaiDatNhacNhoRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<List<CaiDatNhacNhoDto>> GetAllCaiDatNhacNho()
        {
            try
            {
                var dbContext = GetDbContext();
                var data = dbContext.Set<SMS_CaiDat_NhacNho>().Select(x => new CaiDatNhacNhoDto
                {
                    Id = x.Id,
                    IdLoaiTin = x.IdLoaiTin ?? 1,
                    IdMauTin = x.IdMauTin,
                    HinhThucGui = x.HinhThucGui,
                    LoaiThoiGian = x.HinhThucGui,
                    NhacTruocKhoangThoiGian = x.HinhThucGui,
                    TrangThai = x.TrangThai
                }).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CaiDatNhacNho_GroupLoaiTinDto>> GetAllCaiDatNhacNho_GroupLoaiTin()
        {
            try
            {
                List<CaiDatNhacNhoDto> data = await GetAllCaiDatNhacNho();
                var dtGr = data.GroupBy(x => x.IdLoaiTin).Select(x => new CaiDatNhacNho_GroupLoaiTinDto
                {
                    IdLoaiTin = x.Key,
                    LstDetail = x.ToList()
                }).ToList();
                return dtGr;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
