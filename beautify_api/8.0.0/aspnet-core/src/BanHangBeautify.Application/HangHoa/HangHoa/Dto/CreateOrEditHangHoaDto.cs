using Abp.Application.Services.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class CreateOrEditHangHoaDto : EntityDto<Guid>
    {
        public Guid? IdDonViQuyDoi { get; set; }
        public string TenHangHoa { get; set; }
        public string TenHangHoa_KhongDau { get; set; }
        public int TrangThai { get; set; } = 1;
        public int IdLoaiHangHoa { get; set; } = 2;
        public Guid? IdNhomHangHoa { get; set; }
        public string MoTa { get; set; }
        public float? SoPhutThucHien { get; set; }
        public Guid? NguoiTao { get; set; }
        public List<DonViQuiDoiDto> DonViQuiDois { get; set; }
    }
}
