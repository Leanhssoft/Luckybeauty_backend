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
}
