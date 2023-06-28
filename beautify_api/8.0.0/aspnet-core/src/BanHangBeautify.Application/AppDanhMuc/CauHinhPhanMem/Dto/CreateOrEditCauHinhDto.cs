using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.CauHinhPhanMem.Dto
{
    public class CreateOrEditCauHinhDto
    {
        public Guid Id { set; get; }
        public Guid IdChiNhanh { set; get; }
        public bool TichDiem { set; get; }
        public bool KhuyenMai { set; get; }
        public bool MauInMacDinh { set; get; }
        public bool SuDungMaChungTu { set; get; }
        public bool QLKhachHangTheoChiNhanh { set; get; }
    }
}
