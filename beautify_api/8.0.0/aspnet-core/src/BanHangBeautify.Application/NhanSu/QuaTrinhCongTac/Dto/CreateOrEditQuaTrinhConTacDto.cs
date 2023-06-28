using Abp.Domain.Entities;
using System;

namespace BanHangBeautify.NhanSu.QuaTrinhCongTac.Dto
{
    public class CreateOrEditQuaTrinhConTacDto : Entity<Guid>
    {
        public Guid IdNhanVien { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public Guid? IdPhongBan { set; get; }
        public DateTime TuNgay { set; get; }
        public DateTime DenNgay { set; get; }
    }
}
