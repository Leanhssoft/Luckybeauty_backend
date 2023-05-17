using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace BanHangBeautify.Authorization
{
    public class SPAAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ?? context.CreatePermission(PermissionNames.Pages, L("Pages"));

            //Công ty
            var congTy = pages.CreateChildPermission(PermissionNames.Pages_CongTy, L("Company"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Create, L("CreateCompany"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Edit, L("EditCompany"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Delete, L("DeleteCompany"));

            //Chi nhánh
            var chiNhanh = pages.CreateChildPermission(PermissionNames.Pages_ChiNhanh, L("Branch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Create, L("CreateBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Edit, L("EditBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Delete, L("DeleteBranch"));

            //Loại Hàng Hóa
            var loaiHangHoa = pages.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa, L("Branch"));
            loaiHangHoa.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa_Create, L("CreateLoaiHangHoa"));
            loaiHangHoa.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa_Edit, L("EditLoaiHangHoa"));
            loaiHangHoa.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa_Delete, L("DeleteLoaiHangHoa"));

            //Hàng Hóa
            var hangHoa = pages.CreateChildPermission(PermissionNames.Pages_DM_HangHoa, L("HangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Create, L("CreateHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Edit, L("EditHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Delete, L("DeleteHangHoa"));
            //Đơn Vị Qui Đổi
            var donViQuiDoi = pages.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi, L("DonViQuiDoi"));
            donViQuiDoi.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi_Create, L("CreateDonViQuiDoi"));
            donViQuiDoi.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi_Edit, L("EditDonViQuiDoi"));
            donViQuiDoi.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi_Delete, L("DeleteDonViQuiDoi"));
            #region nhân viên
            var nhanSu = pages.CreateChildPermission(PermissionNames.Pages_NhanSu, L("Pages_NhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Create, L("CreatePages_NhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Edit, L("EditPages_NhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Delete, L("DeleteNhanSu"));

            var phongBan = pages.CreateChildPermission(PermissionNames.Pages_PhongBan, L("PhongBan"));
            phongBan.CreateChildPermission(PermissionNames.Pages_PhongBan_Create, L("CreatePhongBan"));
            phongBan.CreateChildPermission(PermissionNames.Pages_PhongBan_Edit, L("EditPhongBan"));
            phongBan.CreateChildPermission(PermissionNames.Pages_PhongBan_Delete, L("DeletePhongBan"));

            var chucVu = pages.CreateChildPermission(PermissionNames.Pages_ChucVu, L("ChucVu"));
            chucVu.CreateChildPermission(PermissionNames.Pages_ChucVu_Create, L("CreateChucVu"));
            chucVu.CreateChildPermission(PermissionNames.Pages_ChucVu_Edit, L("EditChucVu"));
            chucVu.CreateChildPermission(PermissionNames.Pages_ChucVu_Delete, L("DeleteChucVu"));

            var quaTrinhCongTac = pages.CreateChildPermission(PermissionNames.Pages_QuaTrinhCongTac, L("QuaTrinhCongTac"));
            quaTrinhCongTac.CreateChildPermission(PermissionNames.Pages_QuaTrinhCongTac_Create, L("CreateQuaTrinhCongTac"));
            quaTrinhCongTac.CreateChildPermission(PermissionNames.Pages_QuaTrinhCongTac_Edit, L("EditQuaTrinhCongTac"));
            quaTrinhCongTac.CreateChildPermission(PermissionNames.Pages_QuaTrinhCongTac_Delete, L("DeleteQuaTrinhCongTac"));


            var khachHang = pages.CreateChildPermission(PermissionNames.Pages_KhachHang, L("KhachHang"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Create, L("CreateKhahHang"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Edit, L("EditKhachHang"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Delete, L("DeleteKhachHang"));


            var loaiKhachHang = pages.CreateChildPermission(PermissionNames.Pages_LoaiKhach, L("LoaiKhachHang"));
            loaiKhachHang.CreateChildPermission(PermissionNames.Pages_LoaiKhach_Create, L("CreateLoaiKhahHang"));
            loaiKhachHang.CreateChildPermission(PermissionNames.Pages_LoaiKhach_Edit, L("EditLoaiKhachHang"));
            loaiKhachHang.CreateChildPermission(PermissionNames.Pages_LoaiKhach_Delete, L("DeleteLoaiKhachHang"));

            var nguonKhach = pages.CreateChildPermission(PermissionNames.Pages_NguonKhach, L("NguonKhachHang"));
            nguonKhach.CreateChildPermission(PermissionNames.Pages_NguonKhach_Create, L("CreateNguonKhachHang"));
            nguonKhach.CreateChildPermission(PermissionNames.Pages_NguonKhach_Edit, L("EditNguonKhachHang"));
            nguonKhach.CreateChildPermission(PermissionNames.Pages_NguonKhach_Delete, L("DeleteNguonKhachHang"));

            var nhomKhach = pages.CreateChildPermission(PermissionNames.Pages_NhomKhach, L("NhomKhachHang"));
            nhomKhach.CreateChildPermission(PermissionNames.Pages_NhomKhach_Create, L("CreateNhomKhahHang"));
            nhomKhach.CreateChildPermission(PermissionNames.Pages_NhomKhach_Edit, L("EditNhomKhachHang"));
            nhomKhach.CreateChildPermission(PermissionNames.Pages_NhomKhach_Delete, L("DeleteNhomKhachHang"));

            var booking = pages.CreateChildPermission(PermissionNames.Pages_Booking, L("Booking"));
            booking.CreateChildPermission(PermissionNames.Pages_Booking_Create, L("CreateBooking"));
            booking.CreateChildPermission(PermissionNames.Pages_Booking_Edit, L("EditBooking"));
            booking.CreateChildPermission(PermissionNames.Pages_Booking_Delete, L("DeleteBooking"));

            var caLamViec = pages.CreateChildPermission(PermissionNames.Pages_CaLamViec, L("CaLamViec"));
            caLamViec.CreateChildPermission(PermissionNames.Pages_CaLamViec_Create, L("CreateCaLamViec"));
            caLamViec.CreateChildPermission(PermissionNames.Pages_CaLamViec_Edit, L("EditCaLamViec"));
            caLamViec.CreateChildPermission(PermissionNames.Pages_CaLamViec_Delete, L("DeleteCaLamViec"));

            #endregion

            #region other
            /*var cauHinhPhanMem =*/ pages.CreateChildPermission(PermissionNames.Pages_CauHinhPhanMem, L("CauHinhPhanMem"));
            /*var mauIn =*/ pages.CreateChildPermission(PermissionNames.Pages_MauIn, L("MauIn"));
            /*var nganHang =*/ pages.CreateChildPermission(PermissionNames.Pages_NganHang, L("NganHang"));
           /* var taiKhoanNganHang =*/ pages.CreateChildPermission(PermissionNames.Pages_TaiKhoanNganHang, L("TaiKhoanNganHang"));
            /*var phong =*/ pages.CreateChildPermission(PermissionNames.Pages_Phong, L("Phong"));
            /*var heThongSMS = */pages.CreateChildPermission(PermissionNames.Pages_HeThongSMS, L("HeThongSMS"));
           /* var bookingColor =*/ pages.CreateChildPermission(PermissionNames.Pages_Booking_Color, L("BookingColor"));
           /* var cauHinhChungTu =*/ pages.CreateChildPermission(PermissionNames.Pages_CauHinhChungTu, L("CauHinhChungTu"));
           /* var cauHinhTichDiem = */pages.CreateChildPermission(PermissionNames.Pages_CauHinhTichDiem, L("CauHinhTichDiem"));
           /* var checkIn = */pages.CreateChildPermission(PermissionNames.Pages_CheckIn, L("CheckIn"));
            /*var chietKhauDichVu =*/ pages.CreateChildPermission(PermissionNames.Pages_ChietKhauDichVu, L("ChietKhauDichVu"));
            /*var chietKhauHoaDon =*/ pages.CreateChildPermission(PermissionNames.Pages_ChietKhauHoaDon, L("ChietKhauHoaDon"));
           /* var hoaDon = p*/pages.CreateChildPermission(PermissionNames.Pages_HoaDon, L("HoaDon"));
           /* var hoaDonAnh = */pages.CreateChildPermission(PermissionNames.Pages_HoaDon_Anh, L("AnhHoaDon"));
            /*var loaiChungTu =*/ pages.CreateChildPermission(PermissionNames.Pages_LoaiChungTu, L("LoaiChungTu"));
            /*var nhanVienThucHien =*/ pages.CreateChildPermission(PermissionNames.Pages_NhanVienThucHien, L("NhanVienThucHien"));
            /*var nhomKhachDieuKien = */pages.CreateChildPermission(PermissionNames.Pages_NhomKhach_DieuKien, L("NangNhomKhach"));
                /* var khuyenMai = */pages.CreateChildPermission(PermissionNames.Pages_KhuyenMai, L("KhuyenMai"));
            /*var khuyenMaiApDung = */pages.CreateChildPermission(PermissionNames.Pages_KhuyenMai_ApDung, L("KhuyenMaiApDung"));
            /*var lichLamViec =*/ pages.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec, L("LichLamViec"));
                /* var lichLamViecCa =*/ pages.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Ca, L("LichLamViecCa"));
            /*var ngayNghiLe = */pages.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe, L("NgayNghiLe"));
            /*var timeOff =*/ pages.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff, L("NhanVienNghi"));
          /*  var nhatKyThaoTac= */ pages.CreateChildPermission(PermissionNames.Pages_NhatKyThaoTac, L("NhatKyThaoTac"));
            /*var quyHoaDon =*/ pages.CreateChildPermission(PermissionNames.Pages_QuyHoaDon, L("QuyHoaDon"));
            /*var khoanThuChi = p*/ pages.CreateChildPermission(PermissionNames.Pages_KhoanThuChi, L("ThuChi"));
            #endregion

            //adminsitrantion
            var administration = pages.CreateChildPermission(PermissionNames.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(PermissionNames.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(PermissionNames.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_AuditLogs, L("AuditLogs"));

            //HOST-SPECIFIC PERMISSIONS

            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SPAConsts.LocalizationSourceName);
        }
    }
}
