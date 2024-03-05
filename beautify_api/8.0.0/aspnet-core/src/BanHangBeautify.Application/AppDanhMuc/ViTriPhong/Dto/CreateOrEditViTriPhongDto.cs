using System;

namespace BanHangBeautify.AppDanhMuc.ViTriPhong.Dto
{
    public class CreateOrEditViTriPhongDto
    {
        public Guid Id { set; get; }
        public string MaViTriPhong { set; get; }
        public string TenViTriPhong { set; get; }
    }
    public class ViTriPhongDto
    {
        public Guid Id { set; get; }
        public string MaViTriPhong { set; get; }
        public string TenViTriPhong { set; get; }
    }
}
