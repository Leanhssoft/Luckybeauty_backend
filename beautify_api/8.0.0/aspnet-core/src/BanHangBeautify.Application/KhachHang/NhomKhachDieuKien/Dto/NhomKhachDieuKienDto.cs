using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.NhomKhachDieuKien.Dto
{
    public class NhomKhachDieuKienDto
    {
        public byte STT { get; set; }
        public Guid IdNhomKhach { get; set; }
        [ForeignKey("IdNhomKhach")]
        public DM_NhomKhachHang DM_NhomKhachHang { get; set; }

        public byte LoaiDieuKien { get; set; }
        public byte LoaiSoSanh { get; set; } // 1: > , 2: >= , 3: = , 4: <=, 5: <, 6 : khác

        public float GiaTriSo { get; set; }
        public bool GiaTriBool { get; set; }
        public DateTime? GiaTriThoiGian { get; set; }
        public Guid? GiaTriKhuVuc { get; set; }
    }
}
