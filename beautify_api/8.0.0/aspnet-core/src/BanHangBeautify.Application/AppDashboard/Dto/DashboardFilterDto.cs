using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDashboard.Dto
{
    public class DashboardFilterDto
    {
        public Guid IdChiNhanh { get; set; }
        public DateTime? ThoiGianTu { get; set; }
        public DateTime? ThoiGianDen { get; set; }
    }
}
