using Abp.Application.Services.Dto;
using System;

namespace BanHangBeautify.NhanSu.QuaTrinhCongTac.Dto
{
    public class QuaTrinhConTacDto : EntityDto<Guid>
    {
        public Guid IdNhanVien { get; set; }
        public DateTime TuNgay { set; get; }
        public DateTime DenNgay { set; get; }
        public int TrangThai { get; set; }
    }
}