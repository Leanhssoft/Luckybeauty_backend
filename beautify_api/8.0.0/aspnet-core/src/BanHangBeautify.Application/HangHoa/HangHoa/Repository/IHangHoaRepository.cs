using Abp.Application.Services.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using System;
using System.Threading.Tasks;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.HangHoa.HangHoa.Repository
{
    public interface IHangHoaRepository
    {
        Task<HangHoaDto> GetDetailProduct(Guid idDonViQuyDoi, int? tenantId);
        Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaRequestDto input, int? tenantId);
        Task<string> GetProductCode(int? loaiHangHoa, int? tenantId);
        Task<MaxCodeDto> SpGetProductCode(int? loaiHangHoa, int? tenantId);
    }
}
