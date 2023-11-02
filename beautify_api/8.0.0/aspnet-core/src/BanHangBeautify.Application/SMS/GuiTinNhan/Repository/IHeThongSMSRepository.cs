using Abp.Application.Services.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using BanHangBeautify.SMS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.SMS.GuiTinNhan.Repository
{
    public interface IHeThongSMSRepository
    {
        Task<PagedResultDto<CreateOrEditHeThongSMSDto>> GetListSMS(ParamSearch input);
    }
}
