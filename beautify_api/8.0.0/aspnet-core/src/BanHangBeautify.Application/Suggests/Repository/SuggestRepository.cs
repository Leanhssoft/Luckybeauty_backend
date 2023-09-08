using Abp.EntityFrameworkCore;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.Suggests.Dto;
using BanHangBeautify.Users.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests.Repository
{
    public class SuggestRepository : SPARepositoryBase<DM_ChiNhanh, Guid>, ISuggestRepository
    {
        public SuggestRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<SuggestNhanSu>> SuggestNhanSu(int tenantId,Guid idChiNhanh)
        {
            using (var cmd = CreateCommand("prc_SuggestNhanVien"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TenantId",tenantId ));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<SuggestNhanSu>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<SuggestNhanSu>();
            }
        }

        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienByIdDichVu(int tenantId, Guid idChiNhanh, Guid idDichVu)
        {
            using (var cmd = CreateCommand("prc_bookingOnline_SuggestNhanVien"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
                cmd.Parameters.Add(new SqlParameter("@IdDichVu", idDichVu));
                cmd.Parameters.Add(new SqlParameter("@TenNhanVien", ""));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<SuggestEmpolyeeExecuteServiceDto>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<SuggestEmpolyeeExecuteServiceDto>();
            }
        }

        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienThucHienDichVu(int tenantId, Guid idChiNhanh, Guid? idNhanVien)
        {
            using (var cmd = CreateCommand("prc_SuggestNhanVienDichVu"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));

                cmd.Parameters.Add(new SqlParameter("@IdNhanVien", idNhanVien));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<SuggestEmpolyeeExecuteServiceDto>(ds.Tables[0]);
                        return data;
                    }
                }
                return new List<SuggestEmpolyeeExecuteServiceDto>();
            }
        }
    }
}
