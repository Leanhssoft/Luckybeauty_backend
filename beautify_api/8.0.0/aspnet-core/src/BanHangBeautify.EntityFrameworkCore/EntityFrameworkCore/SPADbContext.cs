using Abp.Zero.EntityFrameworkCore;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace BanHangBeautify.EntityFrameworkCore
{
    public class SPADbContext : AbpZeroDbContext<Tenant, Role, User, SPADbContext>
    {
        #region Booking
        public DbSet<Booking> Booking { set; get; }
        public DbSet<BookingNhanVien> BookingNhanVien { set; get; }
        public DbSet<BookingService> BookingService { get; set; }
        public DbSet<DichVu_NhanVien> DichVu_NhanVien { set; get; }
        public DbSet<KH_CheckIn> KH_CheckIn { get; set; }
        public DbSet<Booking_Color> Booking_Color { get; set; }
        #endregion
        #region Danh Mục
        public DbSet<DM_HangHoa> DM_HangHoa { set; get; }
        public DbSet<DM_NhomHangHoa> DM_NhomHangHoa { set; get; }
        public DbSet<DM_LoaiHangHoa> DM_LoaiHangHoa { set; get; }
        public DbSet<DM_DonViQuiDoi> DM_DonViQuiDoi { set; get; }
        public DbSet<DM_KhachHang> DM_KhachHang { set; get; }
        public DbSet<DM_NhomKhach_DieuKien> DM_NhomKhach_DieuKien { get; set; }
        public DbSet<DM_LoaiKhach> DM_LoaiKhach { set; get; }
        public DbSet<DM_NguonKhach> DM_NguonKhach { set; get; }
        public DbSet<DM_NhomKhachHang> DM_NhomKhachHang { set; get; }
        public DbSet<DM_NgayNghiLe> DM_NgayNghiLe { set; get; }
        public DbSet<DM_PhongBan> DM_PhongBan { set; get; }
        public DbSet<DM_ChiNhanh> DM_ChiNhanh { set; get; }
        public DbSet<DM_Phong> DM_Phong { get; set; }
        public DbSet<DM_ViTriPhong> DM_ViTriPhong { get; set; }
        public DbSet<DM_NganHang> DM_NganHang { get; set; }
        public DbSet<DM_TaiKhoanNganHang> DM_TaiKhoanNganHang { get; set; }
        public DbSet<DM_KhoanThuChi> DM_KhoanThuChi { set; get; }
        public DbSet<DM_LoaiChungTu> DM_LoaiChungTu { get; set; }
        public DbSet<DM_MauIn> DM_MauIn { get; set; }
        public DbSet<DM_KhuyenMai> DM_KhuyenMai { get; set; }
        public DbSet<DM_KhuyenMai_ApDung> DM_KhuyenMai_ApDung { get; set; }
        public DbSet<DM_KhuyenMai_ChiTiet> DM_KhuyenMai_ChiTiet { get; set; }
        public DbSet<HT_CauHinh_TichDiem> HT_CauHinh_TichDiem { get; set; }
        public DbSet<HT_CauHinh_TichDiemChiTiet> HT_CauHinh_TichDiemChiTiet { get; set; }
        public DbSet<HT_CauHinh_ChungTu> HT_CauHinh_ChungTu { get; set; }
        public DbSet<HeThong_SMS> HeThong_SMS { get; set; }
        #endregion

        #region Nhân viên
        public DbSet<NS_NhanVien> NS_NhanVien { set; get; }
        public DbSet<NS_CaLamViec> NS_CaLamViec { get; set; }
        public DbSet<NS_QuaTrinh_CongTac> NS_QuaTrinh_CongTac { get; set; }
        public DbSet<NS_ChucVu> NS_ChucVu { set; get; }
        public DbSet<NS_LichLamViec> NS_LichLamViec { set; get; }
        public DbSet<NS_LichLamViec_Ca> NS_LichLamViec_Ca { set; get; }
        public DbSet<NS_NhanVien_TimeOff> NS_NhanVien_TimeOff { set; get; }
        #region Hoa hồng nhân viên
        public DbSet<NS_ChietKhauDichVu> NS_ChietKhauDichVu { get; set; }
        public DbSet<NS_ChietKhauHoaDon> NS_ChietKhauHoaDon { get; set; }
        public DbSet<NS_ChietKhauHoaDon_ChiTiet> NS_ChietKhauHoaDon_ChiTiet { get; set; }
        public DbSet<BH_NhanVienThucHien> BH_NhanVienThucHien { get; set; }
        #endregion
        #endregion
        #region Hiện trạng
        public DbSet<HT_CongTy> HT_CongTy { get; set; }
        public DbSet<HT_CauHinhPhanMem> HT_CauHinhPhanMem { get; set; }
        public DbSet<HT_NhatKyThaoTac> HT_NhatKyThaoTac { get; set; }
        #endregion
        #region Hóa Đơn

        public DbSet<BH_HoaDon> BH_HoaDon { set; get; }
        public DbSet<BH_HoaDon_ChiTiet> BH_HoaDon_ChiTiet { set; get; }
        public DbSet<BH_HoaDon_Anh> BH_HoaDon_Anh { set; get; }
        
        public DbSet<QuyHoaDon> QuyHoaDon { set; get; }
        public DbSet<QuyHoaDon_ChiTiet> QuyHoaDon_ChiTiet { set; get; }
        #endregion
      
       

        /* Define a DbSet for each entity of the application */

        public SPADbContext(DbContextOptions<SPADbContext> options)
            : base(options)
        {

        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DichVu_NhanVien>().HasKey(key => new { key.IdHangHoa, key.IdNhanVien });
        //}
    }
}
