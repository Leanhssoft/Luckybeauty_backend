namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class ExportExcelHangHoaDto
    {
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public double? GiaBan { get; set; }
        public string TxtTrangThaiHang { get; set; }
    }
    public class ImportExcelHangHoaDto
    {
        public string TenNhomHangHoa { get; set; }
        public string MaHangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public int IdLoaiHangHoa { get; set; }
        public double? GiaBan { get; set; }
        public float? SoPhutThucHien { get; set; } //= real sql
        public string GhiChu { get; set; }
    }
}
