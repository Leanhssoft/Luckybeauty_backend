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
        public Guid Id { get; set; }
        public byte STT { get; set; }
        public Guid IdNhomKhach { get; set; }
        public byte LoaiDieuKien { get; set; }
        public byte LoaiSoSanh { get; set; }

        public float GiaTriSo { get; set; }
        public bool GiaTriBool { get; set; }
        public DateTime? GiaTriThoiGian { get; set; }
        public Guid? GiaTriKhuVuc { get; set; }
    }
}
