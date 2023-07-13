using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class CreateServiceManyDto
    {
        public Guid IdNhanVien { get; set; }
        public List<Guid> IdDonViQuiDois { get; set; }
    }
}
