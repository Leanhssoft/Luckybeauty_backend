using Abp.EntityFrameworkCore;
using BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Repository
{
    public class TaiKhoanNganHangRepository:SPARepositoryBase<DM_TaiKhoanNganHang, Guid>
    {
        public TaiKhoanNganHangRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<TaiKhoanNganHangDto>> GetAllBankAccount()
        {
            var dbContext = GetDbContext();
            var data = await (from bank in dbContext.Set<DM_NganHang>()
                              join acc in dbContext.Set<DM_TaiKhoanNganHang>()
                              on bank.Id equals acc.IdNganHang
                              select new TaiKhoanNganHangDto
                              {
                                  Id = acc.Id,
                                  IdNganHang = acc.IdNganHang,
                                  IdChiNhanh = acc.IdChiNhanh,
                                  SoTaiKhoan = acc.SoTaiKhoan,
                                  TenChuThe = acc.TenChuThe,
                                  TenNganHang = bank.TenNganHang,
                                  MaNganHang = bank.MaNganHang,
                                  TrangThai = acc.TrangThai,
                                  GhiChu = acc.GhiChu,
                                  LogoNganHang = ""
                              }).ToListAsync();
            return data;
        }
    }
}
