using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.CauHinh.CauHinhChungTu.Dto
{
    public class CreateOrEditCauHinhChungTuDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public int IdLoaiChungTu { set; get; }
        public bool SuDungMaChiNhanh{set;get;}
        public string MaLoaiChungTu { get; set; }
        public string KiTuNganCach1{set;get;}
        public string KiTuNganCach2{set;get;}
        public string NgayThangNam{set;get;}
        public string KiTuNganCach3{set;get;}
        public int DoDaiSTT{set;get;}
    }
}
