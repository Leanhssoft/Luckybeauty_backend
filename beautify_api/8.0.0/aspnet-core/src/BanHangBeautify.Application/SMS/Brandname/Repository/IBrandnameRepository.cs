using Abp.Application.Services.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Brandname.Repository
{
    public interface IBrandnameRepository
    {
        Task<PagedResultDto<PageBrandnameDto>> GetListBandname(ParamSearchBrandname param, int? tenantId);
        Task<PageBrandnameDto> GetInforBrandname_byId(Guid idBrandname);
    }
}
