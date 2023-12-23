using Abp;
using Abp.Authorization.Users;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.Users.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles.Repository
{
    public class UserRoleRepository : SPARepositoryBase<UserRole, long>, IUserRoleRepository
    {
        public UserRoleRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<UserIdentifierDto>> GetListUser_havePermission(int tenantId, Guid idChiNhanh, string permissionsName)
        {
            using (var cmd = CreateCommand("GetListUser_havePermission"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                cmd.Parameters.Add(new SqlParameter("@PermissionsName", permissionsName));
                using var dataReader = await cmd.ExecuteReaderAsync();
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<UserIdentifierDto>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<UserIdentifierDto>();
        }
    }
}
