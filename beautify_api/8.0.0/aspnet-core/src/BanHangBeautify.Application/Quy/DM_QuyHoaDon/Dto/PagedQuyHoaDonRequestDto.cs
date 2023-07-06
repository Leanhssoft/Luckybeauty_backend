using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Common.CommonClass;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class PagedQuyHoaDonRequestDto : ParamSearch
    {
        public List<string> KhoanThuChis { get; set; }
    }
}
