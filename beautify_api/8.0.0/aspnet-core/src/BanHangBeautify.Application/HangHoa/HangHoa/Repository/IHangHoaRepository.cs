using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa.Repository
{
    public interface IHangHoaRepository : IRepository<DM_HangHoa, Guid>
    {
        Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaPagedResultRequestDto input, int? tenantId);
        Task<string> GetProductCode(int loaiHangHoa, int? tenantId);
    }
}
