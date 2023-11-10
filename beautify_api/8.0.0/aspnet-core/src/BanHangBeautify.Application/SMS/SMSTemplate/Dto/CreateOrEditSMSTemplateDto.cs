using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.SMSTemplate.Dto
{
    public class CreateOrEditSMSTemplateDto
    {
        public Guid Id { get; set; }
        public byte? IdLoaiTin { get; set; }
        public string TenMauTin { get; set; }
        public string NoiDungTinMau { get; set; }
        public bool? LaMacDinh { get; set; }
        public byte? TrangThai { get; set; }
    }
    public class SMSTemplateViewDto
    {
        public Guid Id { get; set; }
        public int IdLoaiTin { get; set; }
        public string TenMauTin { get; set; }
        public string NoiDungTinMau { get; set; }
    }
}
