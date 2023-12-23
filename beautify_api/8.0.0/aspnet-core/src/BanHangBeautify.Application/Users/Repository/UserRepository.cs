using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.Roles.Repository;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Users.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.Users.Repository
{
    public class UserRepository : SPARepositoryBase<User, long>, IUserRepository
    {
        public UserRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<UserProfileDto>> GetAllUser(ParamSearch param)
        {
            using (var cmd = CreateCommand("GetAllUser"))
            {
                cmd.Parameters.Add(new SqlParameter("@TextSearch", param.TextSearch ?? (object)DBNull.Value)); ;
                cmd.Parameters.Add(new SqlParameter("@CurrentPage", param.CurrentPage ?? 0));
                cmd.Parameters.Add(new SqlParameter("@PageSize", param.PageSize ?? 10));
                cmd.Parameters.Add(new SqlParameter("@ColumnSort", param.ColumnSort ?? "createTime"));
                cmd.Parameters.Add(new SqlParameter("@TypeSort", param.TypeSort ?? "desc"));
                using var dataReadder = await cmd.ExecuteReaderAsync();
                string[] array = {"TotalCount", "Data" };
                var ds = new DataSet();
                ds.Load(dataReadder, LoadOption.OverwriteChanges, array);
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<UserProfileDto>(ds.Tables[1]);
                    return new PagedResultDto<UserProfileDto>()
                    {
                        Items = data,
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalCount"].ToString() ?? "0")
                    };
                }
            }
            return new PagedResultDto<UserProfileDto>();
        }
    }
}
