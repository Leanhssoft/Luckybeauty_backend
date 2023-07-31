using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.HangHoa.NhomHangHoa.Dto
{
    public class NhomHangHoaDto : EntityDto<Guid>
    {
        public string MaNhomHang { get; set; }
        public string TenNhomHang { get; set; }
        public string TenNhomHang_KhongDau { get; set; }
        public Guid? IdParent { get; set; }
        public bool? LaNhomHangHoa { get; set; } = false;
        public string Color { get; set; }
        public string MoTa { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public List<NhomHangHoaDto> children { get; set; }
    }
}
