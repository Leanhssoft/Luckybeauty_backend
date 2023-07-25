using System;

namespace BanHangBeautify.AppDanhMuc.Phong.Dto
{
    public class PhongDto
    {
        public Guid Id { get; set; }
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public Guid IdViTri { get; set; }
    }
}
