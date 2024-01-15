using Abp.Dependency;
using BanHangBeautify.Suggests.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests
{
    public interface ISuggestAppService
    {
        Task<List<SuggestDichVuDto>> SuggestDichVu(Guid? idNhanVien);
    }
}
