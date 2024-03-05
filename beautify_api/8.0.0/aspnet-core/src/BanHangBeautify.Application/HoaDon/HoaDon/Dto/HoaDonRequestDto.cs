using System.Collections.Generic;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class HoaDonRequestDto : ParamSearch
    {
        public List<string> IdLoaiChungTus { get; set; }
    }
}
