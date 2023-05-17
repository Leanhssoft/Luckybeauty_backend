using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class CreateOrEditChietKhauDichVuDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public Guid IdNhanVien { set; get; }
        public Guid IdDonViQuyDoi { set; get; }
        public byte LoaiChietKhau { set; get; }
        public decimal GiaTri { set; get; }
        public bool LaPhanTram { set; get; }
    }
}
