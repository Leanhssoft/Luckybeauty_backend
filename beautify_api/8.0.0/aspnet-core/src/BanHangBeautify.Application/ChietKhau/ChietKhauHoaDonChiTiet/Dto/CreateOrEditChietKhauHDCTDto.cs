﻿using System;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDonChiTiet.Dto
{
    public class CreateOrEditChietKhauHDCTDto
    {
        public Guid Id { set; get; }
        public Guid IdChietKhauHD { set; get; }
        public Guid IdNhanVien { set; get; }
    }
}
