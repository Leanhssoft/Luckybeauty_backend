using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.ZaloTemplate
{
    public class Zalo_TemplateDto
    {
        public Guid Id { get; set; }
        public string TenMauTin { get; set; }
        //public string MoTaChiTiet { get; set; }
        public bool? IsDefault { get; set; }
        public byte IdLoaiTin { get; set; }
        [JsonProperty("template_type")]
        public string TemplateType { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        public List<Zalo_ElementDto> elements { get; set; }
        public List<Zalo_ButtonDetailDto> buttons { get; set; }
    }

    public class Zalo_ElementDto
    {
        public Guid Id { get; set; }
        public Guid IdTemplate { get; set; }
        public string ElementType { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool? IsImage { get; set; }
        public string Content { get; set; }
        public List<Zalo_TableDetailDto> tables { get; set; }

    }
    public class Zalo_TableDetailDto
    {
        public Guid Id { get; set; }
        public Guid IdElement { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        public byte? ThuTuSapXep { get; set; }
    }
    public class Zalo_ButtonDetailDto
    {
        public Guid Id { get; set; }
        public Guid IdTemplate { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("payload")]
        public string Payload { get; set; }
        [JsonProperty("image_icon")]
        public string ImageIcon { get; set; }
        public byte? ThuTuSapXep { get; set; }
    }
}
