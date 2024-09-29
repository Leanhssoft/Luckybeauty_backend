using System;
using System.Collections.Generic;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.HoaDon.HoaDon.Dto
{
    public class HoaDonRequestDto : ParamSearch
    {
        public List<string> IdLoaiChungTus { get; set; }
        public HashSet<int> TrangThaiNos { get; set; }
    } 
    public class ParamSearchNhatKyGDV : ParamSearch
    {
        public string IdCustomer { get; set; }
        public Guid? IdGoiDichVu { get; set; }
    }
}
