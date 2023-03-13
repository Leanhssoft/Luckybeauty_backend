using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class CreateOrEditHangHoaDto:EntityDto<Guid>
    {
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public int TrangThai { get; set; }
        public Guid IdLoaiHangHoa { get; set; }
        public string MoTa { get; set; }
    }
}
