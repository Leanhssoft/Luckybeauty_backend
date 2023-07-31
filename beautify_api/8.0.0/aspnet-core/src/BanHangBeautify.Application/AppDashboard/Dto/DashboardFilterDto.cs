using System;

namespace BanHangBeautify.AppDashboard.Dto
{
    public class DashboardFilterDto
    {
        public Guid IdChiNhanh { get; set; }
        public DateTime? ThoiGianTu { get; set; }
        public DateTime? ThoiGianDen { get; set; }
    }
}
