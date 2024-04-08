using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{
    public class SMS_CaiDat_NhacNho : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public byte? IdLoaiTin { get; set; } = 0; // 0. Tin thường, 1. Sinh nhật, 2. Giao dịch, 3.Lịch hẹn
        public float? NhacTruocKhoangThoiGian { get; set; } = 0;
        public byte? LoaiThoiGian { get; set; } = 1;// 0.Phút, 1.Giờ, 2.Ngày
        public byte? TrangThai { get; set; } = 1;
        public byte? HinhThucGui { get; set; } = 0;// 1. SMS, 2.Zalo, 3.Email
        [MaxLength(36)]
        public string IdMauTin { get; set; } = null;// lấy từ mẫu tin zalo/hoặc SMS (--> không khóa ngoại)
    }
}
