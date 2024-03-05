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
        #region Extending Non-Abstract Entities
        public virtual DbSet<UserRoleChiNhanh> UserRoleChiNhanh { get; set; }
        #endregion
        #region Booking
        public DbSet<Booking> Booking { set; get; }
        public DbSet<BookingNhanVien> BookingNhanVien { set; get; }
        public DbSet<BookingService> BookingService { get; set; }
        public DbSet<DichVu_NhanVien> DichVu_NhanVien { set; get; }
        public DbSet<KH_CheckIn> KH_CheckIn { get; set; }
        public DbSet<Booking_Color> Booking_Color { get; set; }
        public DbSet<Booking_CheckIn_HoaDon> Booking_CheckIn_HoaDon { get; set; }
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

        #endregion

        #region SMS
        public DbSet<HeThong_SMS> HeThong_SMS { get; set; }
        public DbSet<HT_SMSBrandname> HT_SMSBrandname { get; set; }
        public DbSet<SMS_Template> SMS_Template { get; set; }
        public DbSet<SMS_CaiDat_NhacNho> SMS_CaiDat_NhacNho { get; set; }
        public DbSet<CaiDat_NhacNho_ChiTiet> CaiDat_NhacNho_ChiTiet { get; set; }
        public DbSet<SMS_LichSuNap_ChuyenTien> SMS_LichSuNap_ChuyenTien { get; set; }
        public DbSet<SMS_NhatKy_GuiTin> SMS_NhatKy_GuiTin { get; set; }
        #endregion
        #region Zalo
        public DbSet<ZaloAuthorization> ZaloAuthorization { get; set; }
        public DbSet<Zalo_KhachHangThanhVien> Zalo_KhachHangThanhVien { get; set; }
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
        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public SPADbContext(DbContextOptions<SPADbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DM_LoaiHangHoa>().HasData(
                new DM_LoaiHangHoa()
                {
                    Id = 1,
                    IsDeleted = false,
                    MaLoaiHangHoa = "HH",
                    TenLoaiHangHoa = "Hàng Hóa",
                    TrangThai = 1
                },
                new DM_LoaiHangHoa()
                {
                    Id = 2,
                    IsDeleted = false,
                    MaLoaiHangHoa = "DV",
                    TenLoaiHangHoa = "Dịch Vụ",
                    TrangThai = 1
                },
                new DM_LoaiHangHoa()
                {
                    Id = 3,
                    IsDeleted = false,
                    MaLoaiHangHoa = "CB",
                    TenLoaiHangHoa = "Combo"
                }
            );
            modelBuilder.Entity<DM_LoaiKhach>().HasData(
                new DM_LoaiKhach()
                {
                    Id = 1,
                    IsDeleted = false,
                    MaLoaiKhachHang = "KH",
                    TenLoaiKhachHang = "Khách hàng",
                    TrangThai = 1
                }, new DM_LoaiKhach()
                {
                    Id = 2,
                    IsDeleted = false,
                    MaLoaiKhachHang = "NCC",
                    TenLoaiKhachHang = "Nhà cung cấp",
                    TrangThai = 1
                });

            modelBuilder.Entity<DM_LoaiChungTu>().HasData(
               new DM_LoaiChungTu()
               {
                   Id = 1,
                   IsDeleted = false,
                   MaLoaiChungTu = "HD",
                   TenLoaiChungTu = "Hóa đơn bán",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 2,
                   IsDeleted = false,
                   MaLoaiChungTu = "GDV",
                   TenLoaiChungTu = "Gói dịch vụ",
                   TrangThai = 1
               },
               new DM_LoaiChungTu()
               {
                   Id = 3,
                   IsDeleted = false,
                   MaLoaiChungTu = "BG",
                   TenLoaiChungTu = "Báo giá",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 4,
                   IsDeleted = false,
                   MaLoaiChungTu = "PNK",
                   TenLoaiChungTu = "Phiếu nhập kho",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 5,
                   IsDeleted = false,
                   MaLoaiChungTu = "PXK",
                   TenLoaiChungTu = "Phiếu xuất kho",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 6,
                   IsDeleted = false,
                   MaLoaiChungTu = "TH",
                   TenLoaiChungTu = "Khách trả hàng",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 7,
                   IsDeleted = false,
                   MaLoaiChungTu = "THNCC",
                   TenLoaiChungTu = "Trả hàng nhà cung cấp",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 8,
                   IsDeleted = false,
                   MaLoaiChungTu = "TGT",
                   TenLoaiChungTu = "Thẻ giá trị",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 9,
                   IsDeleted = false,
                   MaLoaiChungTu = "PKK",
                   TenLoaiChungTu = "Phiếu kiểm kê",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 10,
                   IsDeleted = false,
                   MaLoaiChungTu = "CH",
                   TenLoaiChungTu = "Chuyển hàng",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 11,
                   IsDeleted = false,
                   MaLoaiChungTu = "SQPT",
                   TenLoaiChungTu = "Phiếu thu",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 12,
                   IsDeleted = false,
                   MaLoaiChungTu = "SQPC",
                   TenLoaiChungTu = "Phiếu chi",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 13,
                   IsDeleted = false,
                   MaLoaiChungTu = "DCGV",
                   TenLoaiChungTu = "Điều chỉnh giá vốn",
                   TrangThai = 1
               }, new DM_LoaiChungTu()
               {
                   Id = 14,
                   IsDeleted = false,
                   MaLoaiChungTu = "NH",
                   TenLoaiChungTu = "Nhận hàng",
                   TrangThai = 1
               });
        }
    }
}
