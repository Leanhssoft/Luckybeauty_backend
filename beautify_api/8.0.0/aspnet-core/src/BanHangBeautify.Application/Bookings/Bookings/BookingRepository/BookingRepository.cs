using Abp.EntityFrameworkCore;
using BanHangBeautify.Bookings.Bookings.Dto;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BanHangBeautify.Bookings.Bookings.BookingRepository
{
    public class BookingRepository : SPARepositoryBase<Booking, Guid>, IBookingRepository
    {
        public BookingRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
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
        public async Task<List<BookingDetailDto>> GetKhachHang_Booking(BookingRequestDto input)
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
                cmd.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage));
                cmd.Parameters.Add(new SqlParameter("@PageSize", input.PageSize));
                cmd.Parameters.Add(new SqlParameter("@TrangThaiBook", input.TrangThaiBook));
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var data = ObjectHelper.FillCollection<BookingDetailDto>(ds.Tables[0]);
                        return data;
                    }
                }
            }
            return new List<BookingDetailDto>();
        }
    }
}
