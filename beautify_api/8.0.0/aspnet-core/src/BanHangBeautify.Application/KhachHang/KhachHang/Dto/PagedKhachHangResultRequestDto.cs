using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.KhachHang.KhachHang.Dto
{
    public class PagedKhachHangResultRequestDto : PagedResultRequestDto
    {
        public string keyword { get; set; }
        public int? LoaiDoiTuong { get; set; }
        public string SortBy { set; get; }
        public string SortType { get; set; } = "desc";
        public Guid? IdChiNhanh { get; set; }
        public Guid? IdNhomKhach { get; set; }
        public double? TongChiTieuTu { get; set; }
        public double? TongChiTieuDen { get; set; }
        public bool?  GioiTinh{ get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
    }
}