using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Dto
{
    public class MauTinSMSDto
    {
        public Guid Id { get; set; }
        public int TenantId { get; set; }
        public byte IdLoaiTin { get; set; }
        public string TenMauTin { get; set; }
        public string NoiDungTinMau { get; set; }
        public byte? TrangThai { get; set; } = 1;
        public bool? LaMacDinh { get; set; } = true;

        public string STrangThai { get; set; }
        public string LoaiTin { get; set; }
    }
}
