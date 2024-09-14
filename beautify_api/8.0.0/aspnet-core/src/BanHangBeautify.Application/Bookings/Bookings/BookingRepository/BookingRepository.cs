using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BanHangBeautify.Bookings.Bookings.BookingRepository
{
    public class BookingRepository : SPARepositoryBase<Booking, Guid>, IBookingRepository
    {
        public BookingRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<string> FnGetBookingCode(Guid? idChiNhanh, int idLoaiChungTu)
        {
            using var command = CreateCommand("select dbo.fnGetBookingCode(@IdChiNhanh,@IdLoaiChungTu) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }
        public async Task<List<BookingGetAllItemDto>> GetAllBooking(PagedBookingResultRequestDto input, int tenantId, DateTime timeFrom, DateTime timeTo)
        {
            using (var cmd = CreateCommand("prc_booking_getAll"))
            {
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                cmd.Parameters.Add(new SqlParameter("@IdNhanVien", input.IdNhanVien));
                cmd.Parameters.Add(new SqlParameter("@IdDichVu", input.IdDichVu));
                cmd.Parameters.Add(new SqlParameter("@TimeFrom", timeFrom));
                cmd.Parameters.Add(new SqlParameter("@TimeTo", timeTo));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BookingGetAllItemDto>(ds.Tables[0]);
                        return data;
                    }
                }
            }
            return new List<BookingGetAllItemDto>();
        }
        public async Task<PagedResultDto<BookingDetailDto>> GetKhachHang_Booking(BookingRequestDto input)
        {
            using (var cmd = CreateCommand("prc_getKhachHang_Booking"))
            {
                var idChiNhanhs = string.Empty;
                if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
                {
                    idChiNhanhs = string.Join(",", input.IdChiNhanhs);
                }
                cmd.Parameters.Add(new SqlParameter("@TenantId", input.TenantId));
                cmd.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CurrentPage", input?.CurrentPage ?? 0));
                cmd.Parameters.Add(new SqlParameter("@PageSize", input?.PageSize ?? 20));
                cmd.Parameters.Add(new SqlParameter("@TrangThaiBook", input?.TrangThaiBook ?? 3));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BookingDetailDto>(ds.Tables[0]);
                        return new PagedResultDto<BookingDetailDto>()
                        {
                            TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                            Items = data
                        };
                    }
                }
            }
            return new PagedResultDto<BookingDetailDto>();
        }

        public async Task<List<BookingDetailDto>> GetInforBooking_byID(List<Guid> arrIdBooking)
        {
            if (arrIdBooking != null && arrIdBooking.Count > 0)
            {
                using var cmd = CreateCommand("prc_GetInforBooking_byID");
                string idBookings = string.Join(",", arrIdBooking);
                cmd.Parameters.Add(new SqlParameter("@IdBookings", idBookings));
                using var dataReader = await cmd.ExecuteReaderAsync();
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<BookingDetailDto>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<BookingDetailDto>();
        }

        public async Task<BookingInfoDto> GetBookingInfo(Guid id, int tenantId)
        {
            using (var cmd = CreateCommand("prc_getBookingInfo"))
            {
                cmd.Parameters.Add(new SqlParameter("@IdBooking", id));
                cmd.Parameters.Add(new SqlParameter("@TenantId", tenantId));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillObject<BookingInfoDto>(ds.Tables[0].Rows[0]);
                        data.DonGia = decimal.Parse(ds.Tables[0].Rows[0]["DonGia"].ToString());
                        return data;
                    }
                }
            }
            return new BookingInfoDto();
        }
    }
}
