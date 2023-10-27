using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Repository
{
    public class TaiKhoanNganHangRepository : SPARepositoryBase<DM_TaiKhoanNganHang, Guid>
    {
        public TaiKhoanNganHangRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<TaiKhoanNganHangDto>> GetAllBankAccount(Guid? idChiNhanh = null)
        {
            var dbContext = GetDbContext();
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var data = await (from bank in dbContext.Set<DM_NganHang>()
                                  join acc in dbContext.Set<DM_TaiKhoanNganHang>()
                                  on bank.Id equals acc.IdNganHang
                                  where (idChiNhanh == null || idChiNhanh == Guid.Empty || acc.IdChiNhanh == idChiNhanh)
                                  orderby acc.CreationTime descending // ưu tiên lấy tài khoản dc tạo cuối cùng
                                  select new TaiKhoanNganHangDto
                                  {
                                      Id = acc.Id,
                                      IdNganHang = acc.IdNganHang,
                                      IdChiNhanh = acc.IdChiNhanh,
                                      SoTaiKhoan = acc.SoTaiKhoan,
                                      TenChuThe = acc.TenChuThe,
                                      TenNganHang = bank.TenNganHang,
                                      MaNganHang = bank.MaNganHang,
                                      TenRutGon = bank.TenRutGon,
                                      MaPinNganHang = bank.BIN,
                                      TrangThai = acc.TrangThai,
                                      GhiChu = acc.GhiChu,
                                      LogoNganHang = bank.Logo
                                  }).ToListAsync();
                return data;
            }
        }
    }
}
