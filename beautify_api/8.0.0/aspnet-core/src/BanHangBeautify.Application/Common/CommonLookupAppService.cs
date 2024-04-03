using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Collections.Extensions;
using BanHangBeautify.Common.Dto;
using BanHangBeautify.Editions;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BanHangBeautify.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : SPAAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;

        public CommonLookupAppService(EditionManager editionManager)
        {
            _editionManager = editionManager;
          }
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            input.SkipCount = input.SkipCount>1 ? (input.SkipCount-1)* input.MaxResultCount : 0;
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Keyword.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Keyword) ||
                            u.Surname.Contains(input.Keyword) ||
                            u.UserName.Contains(input.Keyword) ||
                            u.EmailAddress.Contains(input.Keyword)
                    ).WhereIf(input.ExcludeCurrentUser, u => u.Id != AbpSession.GetUserId());

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }
    }
}
