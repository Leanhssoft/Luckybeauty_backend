using System;
using System.Collections.Generic;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaGroupTheoNhomDto : HangHoaDto
    {
        public Guid? IdNhomHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public string Color { get; set; }
        public List<HangHoaDto> HangHoas { get; set; }
    }
}
