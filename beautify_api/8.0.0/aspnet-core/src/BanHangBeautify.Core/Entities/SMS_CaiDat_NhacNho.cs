using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Entities
{
    public class SMS_CaiDat_NhacNho : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public byte? IdLoaiTin { get; set; } = 0; // 0. Tin thường, 1. Sinh nhật, 2. Giao dịch, 3.Lịch hẹn
        public float? NhacTruocKhoangThoiGian { get; set; } = 0;
        public byte? LoaiThoiGian { get; set; } = 1;// 0.Phút, 1.Giờ, 2.Ngày
        public Guid? IdMauTin { get; set; } = null;// Có thể chọn từ tin mẫu - hoặc nhập nội dung trực tiếp
        [ForeignKey("IdMauTin")]
        public SMS_Template SMS_Template { get; set; }
        public string NoiDungTin { get; set; } = string.Empty;
    }
}
