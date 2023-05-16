using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.Phong.Dto
{
    public class CreateOrEditPhongDto
    {
        public Guid Id { get; set; }
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public Guid IdViTri { get; set; }
    }
}
