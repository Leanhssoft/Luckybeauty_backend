using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang.Dto
{
    public class CuaHangDto
    {
        public Guid Id { get; set; }
        public string TenCongTy { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
    }
}
