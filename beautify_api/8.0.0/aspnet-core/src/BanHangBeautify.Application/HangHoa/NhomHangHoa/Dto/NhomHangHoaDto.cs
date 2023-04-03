using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.NhomHangHoa.Dto
{
    public class NhomHangHoaDto : EntityDto<Guid>
    {
        public string MaNhomHang { get; set; }
        public string TenNhomHang { get; set; }
        public bool? LaNhomHangHoa { get; set; } = false;
        public string Color { get; set; }
        public string MoTa { get; set; }
    }
}
