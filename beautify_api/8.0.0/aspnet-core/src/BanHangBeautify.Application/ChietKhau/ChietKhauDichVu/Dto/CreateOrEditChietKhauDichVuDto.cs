﻿using System;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto
{
    public class CreateOrEditChietKhauDichVuDto
    {
        public Guid Id { set; get; }
        public Guid? IdChiNhanh { set; get; }
        public Guid IdNhanVien { set; get; }
        public Guid IdDonViQuiDoi { set; get; }
        public byte? LoaiChietKhau { set; get; } = 1;
        public double? GiaTri { set; get; } = 0;
        public bool? LaPhanTram { set; get; }
    }
}
