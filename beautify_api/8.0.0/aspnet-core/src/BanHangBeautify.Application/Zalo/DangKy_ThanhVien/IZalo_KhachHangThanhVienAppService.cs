﻿using BanHangBeautify.Zalo.DangKyThanhVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.DangKy_ThanhVien
{
    public interface IZalo_KhachHangThanhVienAppService
    {
        Task<Zalo_KhachHangThanhVienDto> DangKyThanhVienZOA(Zalo_KhachHangThanhVienDto dto);
    }
}