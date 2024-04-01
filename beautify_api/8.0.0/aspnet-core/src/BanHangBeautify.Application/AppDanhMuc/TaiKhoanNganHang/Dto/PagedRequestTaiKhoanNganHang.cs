using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.TaiKhoanNganHang.Dto
{
    public class PagedRequestTaiKhoanNganHang : PagedRequestDto
    {
        public Guid IdChiNhanh { get; set; }
    }
}
