using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.NhomHangHoa
{
    public class NhomHangHoaAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_NhomHangHoa, Guid> _dmNhomHangHoa;

        public NhomHangHoaAppService(IRepository<DM_NhomHangHoa, Guid> dmNhomHangHoa)
        {
            _dmNhomHangHoa = dmNhomHangHoa;
        }

        public async Task<PagedResultDto<DM_NhomHangHoa>> GetNhomDichVu()
        {
            PagedResultDto<DM_NhomHangHoa> result = new();
            List<DM_NhomHangHoa> lst = await _dmNhomHangHoa.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.TenNhomHang).ToListAsync();
            result.Items = lst;
            return result;
        }
    }
}
