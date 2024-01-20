using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.KhachHang.KhachHang.Repository
{
    public class KhachHangRespository : SPARepositoryBase<DM_KhachHang, Guid>, IKhachHangRespository
    {
        public KhachHangRespository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<KhachHangView>> GetKhachHang_noBooking(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("prc_getKhachHang_noBooking"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@Filter", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]);
                    }
                }
                return new List<KhachHangView>();
            }
        }
        public async Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input, int tenantId)
        {
            using var command = CreateCommand("prc_KhachHang_GetAll");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@Filter", input.keyword ?? ""));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
            command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
            command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
            command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
            command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
            command.Parameters.Add(new SqlParameter("@IdNhomKHach", input.IdNhomKhach));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data", "TotalCount" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]);
                    return new PagedResultDto<KhachHangView>()
                    {
                        TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<KhachHangView>();
        }

        public async Task<CustomerDetail_FullInfor> GetCustomerDetail_FullInfor(Guid idKhachHang)
        {
            using var command = CreateCommand("GetCustomerDetail_FullInfor");
            command.Parameters.Add(new SqlParameter("@IdKhachHang", idKhachHang));
            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<CustomerDetail_FullInfor>(ds.Tables[0]);
                    return data.FirstOrDefault();
                }
                return new CustomerDetail_FullInfor();
            }
        }
        public async Task<List<HoatDongKhachHang>> GetNhatKyHoatDong_ofKhachHang(Guid idKhachHang)
        {
            using var command = CreateCommand("GetNhatKyHoatDong_ofKhachHang");
            command.Parameters.Add(new SqlParameter("@IdKhachHang", idKhachHang));
            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<HoatDongKhachHang>(ds.Tables[0]);
                    return data;
                }
                return new List<HoatDongKhachHang>();
            }
        }

        public async Task<PagedResultDto<LichSuDatLichDto>> LichSuDatLich(Guid idKhachHang, int tenantId, PagedRequestDto input)
        {
            using (var command = CreateCommand("prc_lichSuDatLich"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdKhachHang", idKhachHang));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<LichSuDatLichDto>(ds.Tables[0]);
                        return new PagedResultDto<LichSuDatLichDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<LichSuDatLichDto>();
            }
        }
        public async Task<PagedResultDto<LichSuHoaDonDto>> LichSuGiaoDich(Guid idKhachHang, int tenantId, PagedRequestDto input)
        {
            using (var command = CreateCommand("prc_lichSuGiaoDich"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                command.Parameters.Add(new SqlParameter("@IdKhachHang", idKhachHang));
                command.Parameters.Add(new SqlParameter("@SortBy", input.SortBy ?? ""));
                command.Parameters.Add(new SqlParameter("@SortType", input.SortType ?? "desc"));
                command.Parameters.Add(new SqlParameter("@MaxResultCount", input.MaxResultCount));
                command.Parameters.Add(new SqlParameter("@SkipCount", input.SkipCount));
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    string[] array = { "Data", "TotalCount" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<LichSuHoaDonDto>(ds.Tables[0]);
                        return new PagedResultDto<LichSuHoaDonDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            Items = data
                        };
                    }
                }
                return new PagedResultDto<LichSuHoaDonDto>();
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
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectHelper.FillCollection<KhachHangView>(ds.Tables[0]); ;
                    }
                }
                return new List<KhachHangView>();
            }
        }

        public async Task ImportDanhMucKhachHang(int? tenantId, long? userId, ImportExcelKhachHangDto dataKhachHang)
        {
            using var command = CreateCommand("spImportDanhMucKhachHang");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@CreatorUserId", userId));
            command.Parameters.Add(new SqlParameter("@TenNhomKhachHang", dataKhachHang.TenNhomKhachHang ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiKhach", dataKhachHang.IdLoaiKhach ?? 1));
            command.Parameters.Add(new SqlParameter("@MaKhachHang", dataKhachHang.MaKhachHang ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TenKhachHang", dataKhachHang.TenKhachHang ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SoDienThoai", dataKhachHang.SoDienThoai ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@NgaySinh", dataKhachHang.NgaySinh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@GioiTinhNam", dataKhachHang.GioiTinhNam ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DiaChi", dataKhachHang.DiaChi ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@MoTa", dataKhachHang.MoTa ?? (object)DBNull.Value));
            await command.ExecuteNonQueryAsync();
        }
    }
}
