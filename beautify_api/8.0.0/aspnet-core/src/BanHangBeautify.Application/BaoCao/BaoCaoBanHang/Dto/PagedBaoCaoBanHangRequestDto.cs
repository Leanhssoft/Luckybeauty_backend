using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.BaoCao.BaoCaoBanHang.Dto
{
    public class PagedBaoCaoBanHangRequestDto
    {
        public string Filter { get; set; } = "";
        public Guid? IdChiNhanh { get; set; }
        public Guid? IdDichVu { get; set; }
        public string SortBy { get; set; } = "";
        public string SortType { get; set; } = "";
        public int SkipCount { get; set; } = 0;
        public int MaxResultCount { get; set; } = 10;
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
    }
    public class ParamSearchBaoCaoBanHang : ParamSearch
    {
        public string IdNhomHangHoa { get; set; }
        public List<Excel_CellData> ReportValueCell { get; set; }
    }
}
