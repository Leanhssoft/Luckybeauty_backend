using Abp.Application.Services.Dto;
using BanHangBeautify.NhanSu.LichLamViec.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec.Repository
{
    public interface ILichLamViecRespository
    {
        public Task<PagedResultDto<LichLamViecNhanVien>> GetAllLichLamViecWeek(PagedRequestLichLamViecDto input,int tenantId);
    }
}
