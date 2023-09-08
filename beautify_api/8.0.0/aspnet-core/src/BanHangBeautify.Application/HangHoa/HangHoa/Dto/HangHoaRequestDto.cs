using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.HangHoa.HangHoa.Dto
{
    public class HangHoaRequestDto : ParamSearch
    {
        public string IdNhomHangHoas { set; get; }
        public int? IdLoaiHangHoa { set; get; } = 0;
        public string Where { set; get; } = null;
    }
}
