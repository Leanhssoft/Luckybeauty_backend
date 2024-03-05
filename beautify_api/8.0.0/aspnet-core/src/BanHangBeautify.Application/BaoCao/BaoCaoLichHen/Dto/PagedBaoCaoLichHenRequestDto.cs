using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoCao.BaoCaoLichHen.Dto
{
    public class PagedBaoCaoLichHenRequestDto
    {
        public string Filter { get; set; } = "";
        public Guid? IdChiNhanh { get; set; }
        public Guid? IdDichVu { get; set; }
        public Guid? IdKhachHang { set; get; }
        public int? TrangThai { get; set; }
        public string SortBy { get; set; } = "";
        public string SortType { get; set; } = "";
        public int SkipCount { get; set; } = 0;
        public int MaxResultCount { get; set; } = 10;
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
    }
}
