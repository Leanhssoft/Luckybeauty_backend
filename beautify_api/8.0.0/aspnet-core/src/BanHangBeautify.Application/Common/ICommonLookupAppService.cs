using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanHangBeautify.Common.Dto;

namespace BanHangBeautify.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);
    }
}
