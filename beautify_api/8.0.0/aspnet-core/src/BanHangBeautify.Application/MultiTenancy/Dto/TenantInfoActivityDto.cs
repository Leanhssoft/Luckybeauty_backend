using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.MultiTenancy.Dto
{
    public class TenantInfoActivityDto
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public string EditionName { get; set; }
        public string Status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public DateTime LastActivityTime { get; set; } 
    }
}
