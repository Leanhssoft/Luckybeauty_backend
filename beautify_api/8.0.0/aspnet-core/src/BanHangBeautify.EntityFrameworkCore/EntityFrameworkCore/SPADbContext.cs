﻿using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.Data.Entities;
using Abp.Authorization;
using BanHangBeautify.Entities;

namespace BanHangBeautify.EntityFrameworkCore
{
    public class SPADbContext : AbpZeroDbContext<Tenant, Role, User, SPADbContext>
    {
        public DbSet<DatLich_ChiTiet> DatLiches { set; get; }
        public DbSet<DatLich_ChiTiet> DatLich_ChiTiets { set; get; }
        public DbSet<DatLich_DichVu> DatLich_NhanViens { get; set; }
        public DbSet<DichVu_NhanVien> DichVu_NhanViens { set; get; }
        public DbSet<NS_NhanVien> NS_NhanViens { set; get; }
        public DbSet<NS_CaLamViec> NS_CaLamViecs { get; set; }
        public DbSet<NS_QuaTrinh_CongTac> NS_QuaTrinh_CongTacs { get; set; }
        public DbSet<DM_HangHoa> DM_HangHoas { set; get; }
        public DbSet<DM_LoaiHangHoa> DM_LoaiHangHoas { set; get; }
        public DbSet<DM_DonViQuiDoi> DM_DonViQuiDois { set; get; }
        public DbSet<DM_KhachHang> DM_KhachHangs { set; get; }
        public DbSet<DM_LoaiKhach> DM_LoaiKhaches { set; get; }
        public DbSet<DM_NguonKhach> DM_NguonKhaches { set; get; }
        public DbSet<DM_NhomKhachHang> DM_NhomKhachHangs { set; get; }
        public DbSet<DM_PhongBan> DM_PhongBans { set; get; }
        public DbSet<NS_ChucVu> NS_ChucVus { set; get; }
        public DbSet<DM_ChiNhanh> DM_ChiNhanhs { set; get; }
        public DbSet<HT_CongTy> HT_CongTys { get; set; }

        /* Define a DbSet for each entity of the application */

        public SPADbContext(DbContextOptions<SPADbContext> options)
            : base(options)
        {

        }
    }
}
