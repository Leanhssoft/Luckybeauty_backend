using System;

namespace BanHangBeautify.NhanSu.LichLamViec.Dto
{
    public class PagedRequestLichLamViecDto : PagedRequestDto
    {
        public Guid IdChiNhanh { get; set; }
        public Guid? IdNhanVien { set; get; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
