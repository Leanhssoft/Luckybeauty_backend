using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Common.CommonClass;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class HoaDonRequestDto: ParamSearch
    {
        public List<string> IdLoaiChungTus { get; set; }
    }
}
