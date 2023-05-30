using Abp.EntityFrameworkCore;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.Common;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu.Repository
{
    internal class ChietKhauDichVuRepository : SPARepositoryBase<NS_ChietKhauDichVu, Guid>, IChietKhauDichVuRepository
    {
        public ChietKhauDichVuRepository(IDbContextProvider<SPADbContext> dbContextProvider)
        : base(dbContextProvider)
        {
        }

        public async Task<ChietKhauDichVuDto> GetHoaHongNV_theoDichVu( int tenantId, Guid idNhanVien, Guid idDonViQuyDoi)
        {
            using var command = CreateCommand("GetHoaHongNV_theoDichVu");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdNhanVien", idNhanVien));
            command.Parameters.Add(new SqlParameter("@IdDonViQuyDoi", idDonViQuyDoi));
            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                var ddd = ds.Tables;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<ChietKhauDichVuDto>(ds.Tables[0]).FirstOrDefault();
                    return data;
                }
            }
            return new ChietKhauDichVuDto();
        }

    }
}
