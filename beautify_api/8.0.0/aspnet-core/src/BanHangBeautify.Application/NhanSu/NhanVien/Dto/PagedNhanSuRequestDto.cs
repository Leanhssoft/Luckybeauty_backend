﻿using System;

namespace BanHangBeautify.NhanSu.NhanVien.Dto
{
    public class PagedNhanSuRequestDto
    {
        public int? TenantId { get; set; }
        public string Filter { get; set; } = "";
        public Guid? IdChiNhanh { get; set; }
        public Guid? IdChucvu { get; set; } = null;
        public string SortBy { get; set; } = "";
        public string SortType { get; set; } = "";
        public int SkipCount { get; set; } = 0;
        public int MaxResultCount { get; set; } = 10;
    }
}
