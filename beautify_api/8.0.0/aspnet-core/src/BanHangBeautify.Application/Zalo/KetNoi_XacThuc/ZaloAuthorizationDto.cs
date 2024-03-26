using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.KetNoi_XacThuc
{
    public class ZaloAuthorizationDto
    {
        public Guid? Id { get; set; }
        public string CodeVerifier { get; set; }
        public string CodeChallenge { get; set; }
        public string AuthorizationCode { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresToken { get; set; }// Thời hạn của access token (đơn vị tính: giây)
        public bool? IsExpiresAccessToken { get; set; } = false;
        public DateTime? CreationTime { get; set; } = DateTime.Now;
    }

    public class ZaloToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
    }

    public class Zalo_Template
    {
        public Guid Id { get; set; }
        public byte IdLoaiTin { get; set; }
        [JsonProperty("template_type")]
        [MaxLength(50)]
        public string TemplateType { get; set; }
        [JsonProperty("language")]
        [MaxLength(10)]
        public string Language { get; set; }
        public List<Zalo_Element> elements { get; set; }
        public List<Zalo_ButtonDetails> buttons { get; set; }
    }

    public class Zalo_Element
    {
        public Guid Id { get; set; }
        public Guid IdTemplate { get; set; }
        [MaxLength(50)]
        public string ElementType { get; set; }
        public byte? ThuTuSapXep { get; set; }
        public bool? IsImage { get; set; }
        public string Content { get; set; }
        public List<Zalo_TableDetails> tables { get; set; }

    }
    public class Zalo_TableDetails
    {
        public Guid Id { get; set; }
        public Guid IdElement { get; set; }
        [MaxLength(35)]
        [JsonProperty("key")]
        public string Key { get; set; }
        [MaxLength(100)]
        [JsonProperty("value")]
        public string Value { get; set; }
        public byte? ThuTuSapXep { get; set; }
    }
    public class Zalo_ButtonDetails
    {
        public Guid Id { get; set; }
        public Guid IdElement { get; set; }
        [MaxLength(50)]
        [JsonProperty("type")]
        public string Type { get; set; }
        [MaxLength(100)]
        [JsonProperty("title")]
        public string Title { get; set; }
        [MaxLength(1000)]
        [JsonProperty("payload")]
        public string Payload { get; set; }
        [JsonProperty("image_icon")]
        public string ImageIcon { get; set; }
        public byte? ThuTuSapXep { get; set; }
    }

    // Khai báo các lớp để mô tả kiểu dữ liệu trong dữ liệu JSON
    public class Recipient
    {
        public string user_id { get; set; }
    }

    public class PayloadContent
    {
        public string value { get; set; }
        public string key { get; set; }
    }

    public class ButtonPayload
    {
        public string url { get; set; }
    }

    public class Button
    {
        public string title { get; set; }
        public string image_icon { get; set; }
        public string type { get; set; }
        public ButtonPayload payload { get; set; }
    }

    public class Payload
    {
        public string template_type { get; set; }
        public string language { get; set; }
        public List<object> elements { get; set; }
        public List<Button> buttons { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload payload { get; set; }
    }

    public class Message
    {
        public Attachment attachment { get; set; }
    }

    public class RequestData
    {
        public Recipient recipient { get; set; }
        public Message message { get; set; }
    }
}
