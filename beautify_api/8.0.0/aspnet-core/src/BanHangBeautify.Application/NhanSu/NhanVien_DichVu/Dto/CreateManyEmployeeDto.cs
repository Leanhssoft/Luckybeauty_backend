using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class CreateManyEmployeeDto
    {
        public Guid IdDonViQuiDoi { get; set; }
        public List<Guid> IdNhanViens { get; set; }
    }
}
