using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class ChietKhauDichVuDto
    {
       public Guid Id{set;get;}
       public Guid IdChiNhanh{set;get;}
       public Guid IdNhanVien{set;get;}
       public Guid IdDonViQuyDoi{set;get;}
       public Guid LoaiChietKhau{set;get;}
       public Guid GiaTri{set;get;}
       public Guid LaPhanTram { set; get; }
    }
}
