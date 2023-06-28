using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec_Ca.Dto
{
    public class CreateOrEditLichLamViecCaDto
    {
        public Guid Id { get; set; }
        public Guid IdLichLamViec { get; set; }
        public Guid IdCaLamViec { get; set; }
        public string GiaTri { get; set; }
    }
}
