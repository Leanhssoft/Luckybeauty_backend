using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyThaoTac.Dto
{
    public class PagedNhatKyRequestDto : PagedRequestDto
    {
        public List<int> LoaiNhatKys { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
    }
}
