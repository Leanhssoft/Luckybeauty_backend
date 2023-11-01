using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests.Dto
{
    public class SuggestTaiKhoanNganHangQRDto
    {
        public string SoTaiKhoan { get; set; }
        public string TenTaiKhoan { get; set; }
        public string bin { get; set; }
        public string TenRutGon { get; set; }
        public bool IsDefault { get; set; }
    }
}
