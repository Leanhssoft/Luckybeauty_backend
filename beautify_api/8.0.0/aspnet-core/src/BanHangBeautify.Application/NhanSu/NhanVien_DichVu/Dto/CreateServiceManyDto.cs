using System;
using System.Collections.Generic;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu.Dto
{
    public class CreateServiceManyDto
    {
        public Guid IdNhanVien { get; set; }
        public List<Guid> IdDonViQuiDois { get; set; }
    }
}
