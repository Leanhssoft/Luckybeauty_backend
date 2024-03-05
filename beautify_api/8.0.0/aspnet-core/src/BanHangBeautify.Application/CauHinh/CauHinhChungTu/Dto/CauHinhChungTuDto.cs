using System;

namespace BanHangBeautify.CauHinh.CauHinhChungTu.Dto
{
    public class CauHinhChungTuDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public string MaLoaiCHungTu { set; get; }
        public int IdLoaiChungTu { set; get; }
        public bool SuDungMaChiNhanh { set; get; }
        public string KiTuNganCach1 { set; get; }
        public string KiTuNganCach2 { set; get; }
        public string NgayThangNam { set; get; }
        public string KiTuNganCach3 { set; get; }
        public int DoDaiSTT { set; get; }
    }
}
