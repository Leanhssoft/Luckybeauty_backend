using Abp.Application.Services.Dto;
using BanHangBeautify.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.Users.Repository
{
    public interface IUserRepository
    {
        Task<PagedResultDto<UserProfileDto>> GetAllUser(ParamSearch param);
    }
}
