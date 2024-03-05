using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests.Dto
{
    public class SuggestNganHangDto
    {
        public Guid Id { get; set; }
        public string MaNganHang { get; set; }
        public string TenNganHang { get; set; }
        public string TenRutGon  { get; set; }
        public string Logo { get; set; }
        public string BIN { get; set; }
    }
}
