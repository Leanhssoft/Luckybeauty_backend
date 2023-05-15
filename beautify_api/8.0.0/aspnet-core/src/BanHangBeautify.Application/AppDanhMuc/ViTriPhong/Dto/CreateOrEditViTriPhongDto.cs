using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.ViTriPhong.Dto
{
    public class CreateOrEditViTriPhongDto
    {
        public Guid Id { set; get; }
        public Guid MaViTriPhong { set; get; }
        public Guid TenViTriPhong { set; get; }
    }
    public class ViTriPhongDto
    {
        public Guid Id { set; get; }
        public Guid MaViTriPhong { set; get; }
        public Guid TenViTriPhong { set; get; }
    }
}
