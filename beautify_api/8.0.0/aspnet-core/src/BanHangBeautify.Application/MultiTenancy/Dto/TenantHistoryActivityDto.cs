using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.MultiTenancy.Dto
{
    public class TenantHistoryActivityDto
    {
        public string ChucNang { get; set; }
        public int LoaiNhatKy { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungChiTiet { get; set; }
        public DateTime CreationTime { get; set; }
        public string TenNguoiThaoTac { get; set; }
        public Guid? IdChiNhanh { get; set; }
        public string ChiNhanh { get; set; }
        public int CreatorUserId { get; set; }
    }
}
