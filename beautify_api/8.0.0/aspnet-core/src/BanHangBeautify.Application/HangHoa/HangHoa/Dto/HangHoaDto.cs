using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaDto
    {
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public int TrangThai { get; set; }
        public Guid IdLoaiHangHoa { get; set; }
        public string MoTa { get; set; }
    }
}
