using System.Collections.Generic;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaRequestDto : ParamSearch
    {
        public List<string> IdNhomHangHoas { set; get; }
        public int? IdLoaiHangHoa { set; get; } = 0;
        public string Where { set; get; } = null;
    }
}
