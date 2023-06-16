using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang.Repository
{
    public class KhachHangRespository : SPARepositoryBase<DM_KhachHang,Guid>, IKhachHangRespository
    {
        public KhachHangRespository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<PagedResultDto<KhachHangView>> GetAllKhachHang(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("prc_KhachHang_GetAll"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@PageSize", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]);
                        return new PagedResultDto<KhachHangView>()
                        {
                            TotalCount = int.Parse(ds.Tables[0].Rows[1]["@totalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<KhachHangView>();
            }
        }
        public async Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input,int tenantId)
        {
            using (var command = CreateCommand("prc_KhachHang_GetAll"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@Filter", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortBy ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]);
                        for (int i = 0; i < data.Count; i++)
                        {
                            var tongChiTieu = ds.Tables[0].Rows[i]["TongChiTieu"].ToString();
                            data[i].TongChiTieu = float.Parse(string.IsNullOrEmpty(tongChiTieu) ? "0" : tongChiTieu);
                        }
                        return new PagedResultDto<KhachHangView>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<KhachHangView>();
            }
        }
        public async Task<List<KhachHangView>> JqAutoCustomer(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("spJqAutoCustomer"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@LoaiDoiTuong", input.LoaiDoiTuong ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@PageSize", input.MaxResultCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data"};
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]); ;
                    }
                }
                return new List<KhachHangView>();
            }
        }
    }
}
