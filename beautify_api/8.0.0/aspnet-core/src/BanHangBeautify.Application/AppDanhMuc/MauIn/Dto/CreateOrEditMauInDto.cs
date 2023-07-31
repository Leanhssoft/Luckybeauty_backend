using System;

namespace BanHangBeautify.AppDanhMuc.MauIn.Dto
{
    public class CreateOrEditMauInDto
    {
        public Guid Id { get; set; }
        public int LoaiChungTu { set; get; }
        public string TenMauIn{set;get;}
        public bool LaMacDinh { set; get; } = false;
        public string NoiDungMauIn{set;get;}
        public Guid IdChiNhanh { get; set; }
    }
}
