using System;
using System.Collections.Generic;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class CreateManyEmployeeDto
    {
        public Guid IdDonViQuiDoi { get; set; }
        public List<Guid> IdNhanViens { get; set; }
    }
}
