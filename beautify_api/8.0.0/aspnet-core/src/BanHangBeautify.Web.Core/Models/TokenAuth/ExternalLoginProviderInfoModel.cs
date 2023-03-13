using Abp.AutoMapper;
using BanHangBeautify.Authentication.External;

namespace BanHangBeautify.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
