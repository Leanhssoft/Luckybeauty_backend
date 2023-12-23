using System;

namespace BanHangBeautify.Users.Dto
{
    public class ProfileDto
    {
        public long Id { get; set; }
        public Guid? NhanSuId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string NgaySinh { get; set; }
        public string CCCD { get; set; }
        public byte? GioiTinh { get; set; }
        public string Avatar { get; set; }
        public string NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string DiaChi { get; set; }
        public string Password { get; set; }
       
    }

    public class UserProfileDto : ProfileDto
    {
        public string TenNhanVien { get; set; }// tennhanvien có thể # name, surname trong bang User (vi NV co the bi cap nhat)
        public string TenChiNhanh { get; set; }// chi nhanh mac dinh
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime CreationTime { get; set; }
        public string RoleNames { get; set; }
        public string TxtTrangThai { get; set; }// get from IsActive
    }
}
