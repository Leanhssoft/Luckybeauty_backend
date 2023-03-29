using Abp.Application.Services.Dto;
using BanHangBeautify.Common;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaPagedResultRequestDto : PagedResultRequestDto
    {
        public string IdNhomHangHoas { set; get; }
        public CommonClass.ParamSearch CommonParam { set; get; }
    }
}
