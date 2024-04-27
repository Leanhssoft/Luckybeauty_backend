using BanHangBeautify.AppCommon;
using BanHangBeautify.SMS.Dto;
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
    public class ParamSearchBaoCaoCheckin : CommonClass.ParamSearch
    {
        public List<string> IdNhomKhachs { get; set; }
        public int? SoNgayChuaCheckIn_From { get; set; }
        public int? SoNgayChuaCheckIn_To { get; set; }
        public int? SoLanCheckIn_From { get; set; }
        public int? SoLanCheckIn_To { get; set; }
        public int? SoLanDatHen_From { get; set; }
        public int? SoLanDatHen_To { get; set; }
    }
}
