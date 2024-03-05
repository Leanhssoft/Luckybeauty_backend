using Abp.EntityFrameworkCore;
using AutoMapper.Internal.Mappers;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.NhanVienThucHien.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien.Repository
{
    public class NhanVienThucHienRepository : SPARepositoryBase<BH_NhanVienThucHien, Guid>, INhanVienThucHienRepository
    {
        public NhanVienThucHienRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<List<CreateOrEditNhanVienThucHienDto>> GetNhanVienThucHien_byIdHoaDon(Guid idHoaDon, Guid? idQuyHoaDon = null)
        {
            var dbContext = GetDbContext();
            var data = await (from nvth in dbContext.Set<BH_NhanVienThucHien>()
                              join nv in dbContext.Set<NS_NhanVien>()
                              on nvth.IdNhanVien equals nv.Id
                              where nvth.IdHoaDon == idHoaDon
                               && (idQuyHoaDon == null || (nvth.LoaiChietKhau == 1 && nvth.IdQuyHoaDon == idQuyHoaDon))
                              select new CreateOrEditNhanVienThucHienDto
                              {
                                  Id = nvth.Id,
                                  IdNhanVien = nvth.IdNhanVien,
                                  IdHoaDon = nvth.IdHoaDon,
                                  IdHoaDonChiTiet = nvth.IdHoaDonChiTiet,
                                  IdQuyHoaDon = nvth.IdQuyHoaDon,
                                  LoaiChietKhau = nvth.LoaiChietKhau,
                                  PTChietKhau = nvth.PTChietKhau,
                                  TienChietKhau = nvth.TienChietKhau,
                                  HeSo = nvth.HeSo,
                                  ChiaDeuChietKhau = nvth.ChiaDeuChietKhau,
                                  TinhHoaHongTruocCK = nvth.TinhHoaHongTruocCK,
                                  TenNhanVien = nv.TenNhanVien,
                              }).ToListAsync();
            return data;
        }

        public async Task<List<CreateOrEditNhanVienThucHienDto>> GetNhanVienThucHien_byIdHoaDonChiTiet(Guid idHoaDonChiTiet)
        {
            var dbContext = GetDbContext();
            var data = await (from nvth in dbContext.Set<BH_NhanVienThucHien>()
                              join nv in dbContext.Set<NS_NhanVien>()
                              on nvth.IdNhanVien equals nv.Id
                              where nvth.IdHoaDonChiTiet == idHoaDonChiTiet
                              select new CreateOrEditNhanVienThucHienDto
                              {
                                  Id = nvth.Id,
                                  IdNhanVien = nvth.IdNhanVien,
                                  IdHoaDon = nvth.IdHoaDon,
                                  IdHoaDonChiTiet = nvth.IdHoaDonChiTiet,
                                  IdQuyHoaDon = nvth.IdQuyHoaDon,
                                  LoaiChietKhau = nvth.LoaiChietKhau,
                                  PTChietKhau = nvth.PTChietKhau,
                                  TienChietKhau = nvth.TienChietKhau,
                                  HeSo = nvth.HeSo,
                                  ChiaDeuChietKhau = nvth.ChiaDeuChietKhau,
                                  TinhHoaHongTruocCK = nvth.TinhHoaHongTruocCK,
                                  TenNhanVien = nv.TenNhanVien,
                              }).ToListAsync();
            return data;
        }
        /// <summary>
        /// update all NVTH of hoadon
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="idHoaDon"></param>
        /// <param name="lstNV"></param>
        /// <returns></returns>
        public async Task<bool> UpdateNhanVienThucHien_byIdHoaDon(int? tenantId, Guid idHoaDon, List<CreateOrEditNhanVienThucHienDto> lstNV = null)
        {
            try
            {
                var dbContext = GetDbContext();
                var allQuyCT = (from qhd in dbContext.Set<QuyHoaDon>()
                                join qct in dbContext.Set<QuyHoaDon_ChiTiet>() on qhd.Id equals qct.IdQuyHoaDon
                                join hd in dbContext.Set<BH_HoaDon>() on qct.IdHoaDonLienQuan equals hd.Id
                                where hd.Id == idHoaDon && qhd.TrangThai != 0
                                && qct.IdHoaDonLienQuan == idHoaDon
                                select new
                                {
                                    qct.IdQuyHoaDon,
                                    qct.TienThu,
                                    qct.HinhThucThanhToan,
                                    qct.LaPTChiPhiNganHang,
                                    qct.ChiPhiNganHang,
                                }).ToList();
                var thuAll = (from qct in allQuyCT
                              group new { qct } by new { qct.IdQuyHoaDon } into g
                              select new
                              {
                                  g.Key.IdQuyHoaDon,
                                  TongTienThu = g.Where(x => x.qct.HinhThucThanhToan != 4 && x.qct.HinhThucThanhToan != 5)// HinhThucThanhToan= 5 (sử dụng điểm)
                                   .Sum(x => x.qct.TienThu)
                              }).ToList();
                var thuPos = (from qct in allQuyCT
                              group new { qct } by new { qct.IdQuyHoaDon } into g
                              select new
                              {
                                  g.Key.IdQuyHoaDon,
                                  TongTienThu = (g.Where(x => x.qct.HinhThucThanhToan == 2)
                                   .Sum(x => x.qct.LaPTChiPhiNganHang == 1 ? x.qct.TienThu * x.qct.ChiPhiNganHang / 100 : x.qct.ChiPhiNganHang)) ?? 0
                              }).ToList();
                var thuctinh = (from all in thuAll
                                join pos in thuPos on all.IdQuyHoaDon equals pos.IdQuyHoaDon
                                into thuc
                                from tt in thuc.DefaultIfEmpty()
                                select new
                                {
                                    all.IdQuyHoaDon,
                                    TongTienThu = all.TongTienThu - (thuc != null ? tt.TongTienThu : 0)
                                }).ToList();

                Guid? idSQFirst = Guid.Empty;
                List<BH_NhanVienThucHien> lstAdd = new();
                if (thuctinh != null && thuctinh.Count > 0)
                {
                    idSQFirst = thuctinh.FirstOrDefault().IdQuyHoaDon;
                }


                // get nv theo doanhthu + vnd
                var lstNV_notThucThu = lstNV.Where(x => x.LoaiChietKhau != 1).ToList();
                foreach (var nv in lstNV_notThucThu)
                {
                    BH_NhanVienThucHien nvNew = new()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId ?? 1,
                        IdNhanVien = nv.IdNhanVien,
                        IdHoaDon = idHoaDon,
                        IdHoaDonChiTiet = nv.IdHoaDonChiTiet,
                        IdQuyHoaDon = idSQFirst == Guid.Empty ? null : idSQFirst,
                        HeSo = nv.HeSo ?? 1,
                        LoaiChietKhau = nv.LoaiChietKhau,
                        PTChietKhau = nv.PTChietKhau,
                        TienChietKhau = nv.TienChietKhau,
                        TinhHoaHongTruocCK = nv.TinhHoaHongTruocCK,
                        ChiaDeuChietKhau = nv.ChiaDeuChietKhau,
                    };
                    await dbContext.Set<BH_NhanVienThucHien>().AddAsync(nvNew);
                }

                // get nv theo thucthu
                var lstNV_ThucThu = lstNV.Where(x => x.LoaiChietKhau == 1).ToList();
                foreach (var nv in lstNV_ThucThu)
                {
                    foreach (var sq in thuctinh)
                    {
                        BH_NhanVienThucHien objNew = new BH_NhanVienThucHien
                        {
                            Id = Guid.NewGuid(),
                            TenantId = tenantId ?? 1,
                            IdNhanVien = nv.IdNhanVien,
                            IdHoaDon = idHoaDon,
                            IdHoaDonChiTiet = nv.IdHoaDonChiTiet,
                            IdQuyHoaDon = sq.IdQuyHoaDon,
                            HeSo = nv.HeSo ?? 1,
                            LoaiChietKhau = nv.LoaiChietKhau,
                            PTChietKhau = nv.PTChietKhau,
                            TienChietKhau = (nv.HeSo * sq.TongTienThu * nv.PTChietKhau / 100) ?? 1,
                            TinhHoaHongTruocCK = nv.TinhHoaHongTruocCK,
                            ChiaDeuChietKhau = nv.ChiaDeuChietKhau,
                        };
                        await dbContext.Set<BH_NhanVienThucHien>().AddAsync(objNew);
                    }
                }

                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
