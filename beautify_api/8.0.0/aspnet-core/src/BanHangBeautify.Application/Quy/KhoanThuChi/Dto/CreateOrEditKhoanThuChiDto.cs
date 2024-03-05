using System;

namespace BanHangBeautify.Quy.KhoanThuChi.Dto
{
    public class CreateOrEditKhoanThuChiDto
    {
        public Guid Id { set; get; }
        public bool LaKhoanThu { set; get; }
        public string MaKhoanThuChi { set; get; }
        public string TenKhoanThuChi { set; get; }
        public string ChungTuApDung { set; get; }
        public string GhiChu { get; set; }
    }
}
