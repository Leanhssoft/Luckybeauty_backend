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
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Create, L("CreateCompany"), multiTenancySides: MultiTenancySides.Host);
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Edit, L("EditCompany"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Delete, L("DeleteCompany"), multiTenancySides: MultiTenancySides.Host);

            //Chi nhánh
            var chiNhanh = pages.CreateChildPermission(PermissionNames.Pages_ChiNhanh, L("Branch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Create, L("CreateBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Edit, L("EditBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Delete, L("DeleteBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Export, L("ExportBranch"));  
            
            var brandname = pages.CreateChildPermission(PermissionNames.Pages_Brandname, L("Brandname"));
            brandname.CreateChildPermission(PermissionNames.Pages_Brandname_Create, L("Create"));
            brandname.CreateChildPermission(PermissionNames.Pages_Brandname_Edit, L("Edit"));
            brandname.CreateChildPermission(PermissionNames.Pages_Brandname_Delete, L("Delete"));
            brandname.CreateChildPermission(PermissionNames.Pages_Brandname_Export, L("Export"));
            brandname.CreateChildPermission(PermissionNames.Pages_Brandname_NopTien, L("NopTienBrandname"));

            var brandnameSMS = pages.CreateChildPermission(PermissionNames.Pages_Brandname_ChuyenTien, L("BrandnameSMS"));
            brandnameSMS.CreateChildPermission(PermissionNames.Pages_Brandname_ChuyenTien_Create, L("Create"));
            brandnameSMS.CreateChildPermission(PermissionNames.Pages_Brandname_ChuyenTien_Edit, L("Edit"));
            brandnameSMS.CreateChildPermission(PermissionNames.Pages_Brandname_ChuyenTien_Delete, L("Delete"));

            //Loại Hàng Hóa
            var loaiHangHoa = pages.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa, L("LoaiHangHoa"));
            loaiHangHoa.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa_Create, L("CreateLoaiHangHoa"));
            loaiHangHoa.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa_Edit, L("EditLoaiHangHoa"));
            loaiHangHoa.CreateChildPermission(PermissionNames.Pages_DM_LoaiHangHoa_Delete, L("DeleteLoaiHangHoa"));

            //Hàng Hóa
            var hangHoa = pages.CreateChildPermission(PermissionNames.Pages_DM_HangHoa, L("HangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Create, L("CreateHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Edit, L("EditHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Delete, L("DeleteHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Export, L("ExportHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Import, L("ImportHangHoa"));
            hangHoa.CreateChildPermission(PermissionNames.Pages_DM_HangHoa_Restore, L("Restore"));

            //Nhóm hàng hóa
            var nhomhangHoa = pages.CreateChildPermission(PermissionNames.Pages_DM_NhomHangHoa, L("NhomHangHoa"));
            nhomhangHoa.CreateChildPermission(PermissionNames.Pages_DM_NhomHangHoa_Create, L("CreateNhomHangHoa"));
            nhomhangHoa.CreateChildPermission(PermissionNames.Pages_DM_NhomHangHoa_Edit, L("EditNhomHangHoa"));
            nhomhangHoa.CreateChildPermission(PermissionNames.Pages_DM_NhomHangHoa_Delete, L("DeleteNhomHangHoa"));

            //Đơn Vị Qui Đổi
            var donViQuiDoi = pages.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi, L("DonViQuiDoi"));
            donViQuiDoi.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi_Create, L("CreateDonViQuiDoi"));
            donViQuiDoi.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi_Edit, L("EditDonViQuiDoi"));
            donViQuiDoi.CreateChildPermission(PermissionNames.Pages_DonViQuiDoi_Delete, L("DeleteDonViQuiDoi"));
            #region nhân viên
            var nhanSu = pages.CreateChildPermission(PermissionNames.Pages_NhanSu, L("NhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Create, L("CreateNhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Edit, L("EditNhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Delete, L("DeleteNhanSu"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Import, L("Import"));
            nhanSu.CreateChildPermission(PermissionNames.Pages_NhanSu_Export, L("Export"));

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
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Create, L("CreateKhachHang"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Edit, L("EditKhachHang"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Delete, L("DeleteKhachHang"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Import, L("Import"));
            khachHang.CreateChildPermission(PermissionNames.Pages_KhachHang_Export, L("Export"));


            var loaiKhachHang = pages.CreateChildPermission(PermissionNames.Pages_LoaiKhach, L("LoaiKhachHang"));
            loaiKhachHang.CreateChildPermission(PermissionNames.Pages_LoaiKhach_Create, L("CreateLoaiKhahHang"));
            loaiKhachHang.CreateChildPermission(PermissionNames.Pages_LoaiKhach_Edit, L("EditLoaiKhachHang"));
            loaiKhachHang.CreateChildPermission(PermissionNames.Pages_LoaiKhach_Delete, L("DeleteLoaiKhachHang"));

            var nguonKhach = pages.CreateChildPermission(PermissionNames.Pages_NguonKhach, L("NguonKhachHang"));
            nguonKhach.CreateChildPermission(PermissionNames.Pages_NguonKhach_Create, L("CreateNguonKhachHang"));
            nguonKhach.CreateChildPermission(PermissionNames.Pages_NguonKhach_Edit, L("EditNguonKhachHang"));
            nguonKhach.CreateChildPermission(PermissionNames.Pages_NguonKhach_Delete, L("DeleteNguonKhachHang"));

            var nhomKhach = pages.CreateChildPermission(PermissionNames.Pages_NhomKhach, L("NhomKhachHang"));
            nhomKhach.CreateChildPermission(PermissionNames.Pages_NhomKhach_Create, L("CreateNhomKhachHang"));
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
            caLamViec.CreateChildPermission(PermissionNames.Pages_CaLamViec_Import, L("Import"));
            caLamViec.CreateChildPermission(PermissionNames.Pages_CaLamViec_Export, L("Export"));

            #endregion

            #region other
            var cauHinhPhanMem = pages.CreateChildPermission(PermissionNames.Pages_CauHinhPhanMem, L("CauHinhPhanMem"));
            cauHinhPhanMem.CreateChildPermission(PermissionNames.Pages_CauHinhPhanMem_Create, L("Create"));
            cauHinhPhanMem.CreateChildPermission(PermissionNames.Pages_CauHinhPhanMem_Edit, L("Edit"));
            cauHinhPhanMem.CreateChildPermission(PermissionNames.Pages_CauHinhPhanMem_Delete, L("Delete"));

            var mauIn = pages.CreateChildPermission(PermissionNames.Pages_MauIn, L("MauIn"));
            mauIn.CreateChildPermission(PermissionNames.Pages_MauIn_Create, L("Create"));
            mauIn.CreateChildPermission(PermissionNames.Pages_MauIn_Edit, L("Edit"));
            mauIn.CreateChildPermission(PermissionNames.Pages_MauIn_Delete, L("Delete"));

            var nganHang = pages.CreateChildPermission(PermissionNames.Pages_NganHang, L("NganHang"));
            nganHang.CreateChildPermission(PermissionNames.Pages_NganHang_Create, L("Create"));
            nganHang.CreateChildPermission(PermissionNames.Pages_NganHang_Update, L("Edit"));
            nganHang.CreateChildPermission(PermissionNames.Pages_NganHang_Delete, L("Delete"));

            var taiKhoanNganHang = pages.CreateChildPermission(PermissionNames.Pages_TaiKhoanNganHang, L("TaiKhoanNganHang"));
            taiKhoanNganHang.CreateChildPermission(PermissionNames.Pages_TaiKhoanNganHang_Create, L("Create"));
            taiKhoanNganHang.CreateChildPermission(PermissionNames.Pages_TaiKhoanNganHang_Edit, L("Edit"));
            taiKhoanNganHang.CreateChildPermission(PermissionNames.Pages_TaiKhoanNganHang_Delete, L("Delete"));

            var phong = pages.CreateChildPermission(PermissionNames.Pages_Phong, L("Phong"));
            phong.CreateChildPermission(PermissionNames.Pages_Phong_Create, L("Create"));
            phong.CreateChildPermission(PermissionNames.Pages_Phong_Edit, L("Edit"));
            phong.CreateChildPermission(PermissionNames.Pages_Phong_Delete, L("Delete"));

            var heThongSMS = pages.CreateChildPermission(PermissionNames.Pages_HeThongSMS, L("HeThongSMS"));
            heThongSMS.CreateChildPermission(PermissionNames.Pages_HeThongSMS_Create, L("Create"));
            heThongSMS.CreateChildPermission(PermissionNames.Pages_HeThongSMS_Edit, L("Edit"));
            heThongSMS.CreateChildPermission(PermissionNames.Pages_HeThongSMS_Delete, L("Delete"));
            heThongSMS.CreateChildPermission(PermissionNames.Pages_HeThongSMS_Resend, L("Resend"));

            var smsMauTin = pages.CreateChildPermission(PermissionNames.Pages_SMS_Template, L("SMS_Template"));
            smsMauTin.CreateChildPermission(PermissionNames.Pages_SMS_Template_Create, L("Create"));
            smsMauTin.CreateChildPermission(PermissionNames.Pages_SMS_Template_Edit, L("Edit"));
            smsMauTin.CreateChildPermission(PermissionNames.Pages_SMS_Template_Delete, L("Delete"));

            //var bookingColor = 
            pages.CreateChildPermission(PermissionNames.Pages_Booking_Color, L("BookingColor"));

            var cauHinhChungTu = pages.CreateChildPermission(PermissionNames.Pages_CauHinhChungTu, L("CauHinhChungTu"));
            cauHinhChungTu.CreateChildPermission(PermissionNames.Pages_CauHinhChungTu_Create, L("Create"));
            cauHinhChungTu.CreateChildPermission(PermissionNames.Pages_CauHinhChungTu_Edit, L("Edit"));
            cauHinhChungTu.CreateChildPermission(PermissionNames.Pages_CauHinhChungTu_Delete, L("Delete"));

            var cauHinhTichDiem = pages.CreateChildPermission(PermissionNames.Pages_CauHinhTichDiem, L("CauHinhTichDiem"));
            cauHinhTichDiem.CreateChildPermission(PermissionNames.Pages_CauHinhTichDiem_Create, L("Create"));
            cauHinhTichDiem.CreateChildPermission(PermissionNames.Pages_CauHinhTichDiem_Edit, L("Edit"));
            cauHinhTichDiem.CreateChildPermission(PermissionNames.Pages_CauHinhTichDiem_Delete, L("Delete"));


            var checkIn = pages.CreateChildPermission(PermissionNames.Pages_CheckIn, L("CheckIn"));
            checkIn.CreateChildPermission(PermissionNames.Pages_CheckIn_Create, L("Create"));
            checkIn.CreateChildPermission(PermissionNames.Pages_CheckIn_Edit, L("Edit"));
            checkIn.CreateChildPermission(PermissionNames.Pages_CheckIn_Delete, L("Delete"));

            var chietKhauDichVu = pages.CreateChildPermission(PermissionNames.Pages_ChietKhauDichVu, L("ChietKhauDichVu"));
            chietKhauDichVu.CreateChildPermission(PermissionNames.Pages_ChietKhauDichVu_Create, L("Create"));
            chietKhauDichVu.CreateChildPermission(PermissionNames.Pages_ChietKhauDichVu_Edit, L("Edit"));
            chietKhauDichVu.CreateChildPermission(PermissionNames.Pages_ChietKhauDichVu_Delete, L("Delete"));
            chietKhauDichVu.CreateChildPermission(PermissionNames.Pages_ChietKhauDichVu_Export, L("Export"));

            var chietKhauHoaDon = pages.CreateChildPermission(PermissionNames.Pages_ChietKhauHoaDon, L("ChietKhauHoaDon"));
            chietKhauHoaDon.CreateChildPermission(PermissionNames.Pages_ChietKhauHoaDon_Create, L("Create"));
            chietKhauHoaDon.CreateChildPermission(PermissionNames.Pages_ChietKhauHoaDon_Edit, L("Edit"));
            chietKhauHoaDon.CreateChildPermission(PermissionNames.Pages_ChietKhauHoaDon_Delete, L("Delete"));

            /* var hoaDon = p*/
            var hoaDon = pages.CreateChildPermission(PermissionNames.Pages_HoaDon, L("HoaDon"));
            hoaDon.CreateChildPermission(PermissionNames.Pages_HoaDon_Create, L("CreateHoaDon"));
            hoaDon.CreateChildPermission(PermissionNames.Pages_HoaDon_Edit, L("EditHoaDon"));
            hoaDon.CreateChildPermission(PermissionNames.Pages_HoaDon_Delete, L("DeleteHoaDon"));
            hoaDon.CreateChildPermission(PermissionNames.Pages_HoaDon_Import, L("Import"));
            hoaDon.CreateChildPermission(PermissionNames.Pages_HoaDon_Export, L("Export"));
            hoaDon.CreateChildPermission(PermissionNames.Pages_HoaDon_Print, L("Print"));

            var hoaDonAnh = pages.CreateChildPermission(PermissionNames.Pages_HoaDon_Anh, L("AnhHoaDon"));
            hoaDonAnh.CreateChildPermission(PermissionNames.Pages_HoaDon_Anh_Create, L("Create"));
            hoaDonAnh.CreateChildPermission(PermissionNames.Pages_HoaDon_Anh_Update, L("Edit"));
            hoaDonAnh.CreateChildPermission(PermissionNames.Pages_HoaDon_Anh_Delete, L("Delete"));

            var loaiChungTu = pages.CreateChildPermission(PermissionNames.Pages_LoaiChungTu, L("LoaiChungTu"));
            loaiChungTu.CreateChildPermission(PermissionNames.Pages_LoaiChungTu_Create, L("Create"));
            loaiChungTu.CreateChildPermission(PermissionNames.Pages_LoaiChungTu_Edit, L("Edit"));
            loaiChungTu.CreateChildPermission(PermissionNames.Pages_LoaiChungTu_Delete, L("Delete"));

            var nhanVienThucHien = pages.CreateChildPermission(PermissionNames.Pages_NhanVienThucHien, L("NhanVienThucHien"));
            nhanVienThucHien.CreateChildPermission(PermissionNames.Pages_NhanVienThucHien_Create, L("Create"));
            nhanVienThucHien.CreateChildPermission(PermissionNames.Pages_NhanVienThucHien_Edit, L("Edit"));
            nhanVienThucHien.CreateChildPermission(PermissionNames.Pages_NhanVienThucHien_Delete, L("Delete"));

            var nhanVienDichVu = pages.CreateChildPermission(PermissionNames.Pages_NhanVien_DichVu, L("NhanVienDichVu"));
            nhanVienDichVu.CreateChildPermission(PermissionNames.Pages_NhanVien_DichVu_Create, L("Create"));
            nhanVienDichVu.CreateChildPermission(PermissionNames.Pages_NhanVien_DichVu_Edit, L("Edit"));
            nhanVienDichVu.CreateChildPermission(PermissionNames.Pages_NhanVien_DichVu_Delete, L("Delete"));

            var nhomKhachDieuKien = pages.CreateChildPermission(PermissionNames.Pages_NhomKhach_DieuKien, L("NangNhomKhach"));
            nhomKhachDieuKien.CreateChildPermission(PermissionNames.Pages_NhomKhach_DieuKien_Create, L("Create"));
            nhomKhachDieuKien.CreateChildPermission(PermissionNames.Pages_NhomKhach_DieuKien_Edit, L("Edit"));
            nhomKhachDieuKien.CreateChildPermission(PermissionNames.Pages_NhomKhach_DieuKien_Delete, L("Delete"));

            var khuyenMai = pages.CreateChildPermission(PermissionNames.Pages_KhuyenMai, L("KhuyenMai"));
            khuyenMai.CreateChildPermission(PermissionNames.Pages_KhuyenMai_Create, L("Create"));
            khuyenMai.CreateChildPermission(PermissionNames.Pages_KhuyenMai_Edit, L("Edit"));
            khuyenMai.CreateChildPermission(PermissionNames.Pages_KhuyenMai_Delete, L("Delete"));

            var khuyenMaiApDung = pages.CreateChildPermission(PermissionNames.Pages_KhuyenMai_ApDung, L("KhuyenMaiApDung"));
            khuyenMaiApDung.CreateChildPermission(PermissionNames.Pages_KhuyenMai_ApDung_Create, L("Create"));
            khuyenMaiApDung.CreateChildPermission(PermissionNames.Pages_KhuyenMai_ApDung_Edit, L("Edit"));
            khuyenMaiApDung.CreateChildPermission(PermissionNames.Pages_KhuyenMai_ApDung_Delete, L("Delete"));

            var lichLamViec = pages.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec, L("LichLamViec"));
            lichLamViec.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Create, L("Create"));
            lichLamViec.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Edit, L("Edit"));
            lichLamViec.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Delete, L("Delete"));

            var lichLamViecCa = pages.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Ca, L("LichLamViecCa"));
            lichLamViecCa.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Ca_Create, L("Create"));
            lichLamViecCa.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Ca_Edit, L("Edit"));
            lichLamViecCa.CreateChildPermission(PermissionNames.Pages_NhanSu_LichLamViec_Ca_Delete, L("Delete"));

            var ngayNghiLe = pages.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe, L("NgayNghiLe"));
            ngayNghiLe.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe_Create, L("Create"));
            ngayNghiLe.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe_Edit, L("Edit"));
            ngayNghiLe.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe_Delete, L("Delete"));
            ngayNghiLe.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe_Import, L("Import"));
            ngayNghiLe.CreateChildPermission(PermissionNames.Pages_NhanSu_NgayNghiLe_Export, L("Export"));

            var timeOff = pages.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff, L("NhanVienNghi"));
            timeOff.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff_Create, L("Create"));
            timeOff.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff_Edit, L("Edit"));
            timeOff.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff_Delete, L("Delete"));
            timeOff.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff_Import, L("Import"));
            timeOff.CreateChildPermission(PermissionNames.Pages_NhanSu_TimeOff_Export, L("Export"));

            var nhatKyThaoTac = pages.CreateChildPermission(PermissionNames.Pages_NhatKyThaoTac, L("NhatKyThaoTac"));
            nhatKyThaoTac.CreateChildPermission(PermissionNames.Pages_NhatKyThaoTac_Create, L("Create"));
            nhatKyThaoTac.CreateChildPermission(PermissionNames.Pages_NhatKyThaoTac_Edit, L("Edit"));
            nhatKyThaoTac.CreateChildPermission(PermissionNames.Pages_NhatKyThaoTac_Delete, L("Delete"));

            var quyHoaDon = pages.CreateChildPermission(PermissionNames.Pages_QuyHoaDon, L("QuyHoaDon"));
            quyHoaDon.CreateChildPermission(PermissionNames.Pages_QuyHoaDon_Create, L("Create"));
            quyHoaDon.CreateChildPermission(PermissionNames.Pages_QuyHoaDon_Edit, L("Edit"));
            quyHoaDon.CreateChildPermission(PermissionNames.Pages_QuyHoaDon_Delete, L("Delete"));
            quyHoaDon.CreateChildPermission(PermissionNames.Pages_QuyHoaDon_Export, L("Export"));
            quyHoaDon.CreateChildPermission(PermissionNames.Pages_QuyHoaDon_Print, L("Print"));

            var khoanThuChi = pages.CreateChildPermission(PermissionNames.Pages_KhoanThuChi, L("ThuChi"));
            khoanThuChi.CreateChildPermission(PermissionNames.Pages_KhoanThuChi_Create, L("Create"));
            khoanThuChi.CreateChildPermission(PermissionNames.Pages_KhoanThuChi_Edit, L("Edit"));
            khoanThuChi.CreateChildPermission(PermissionNames.Pages_KhoanThuChi_Delete, L("Delete"));

            #endregion

            // Notification
            var notification = pages.CreateChildPermission(PermissionNames.Notifications, L("Notifications"));
            notification.CreateChildPermission(PermissionNames.Notifications_Booking, L("Booking"));
            notification.CreateChildPermission(PermissionNames.Notifications_Birthday, L("Birthday"));

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