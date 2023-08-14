using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Security;
using BanHangBeautify.Common;
using BanHangBeautify.Common.Consts;
using BanHangBeautify.DatLichOnline.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.SignalR.Bookings;
using BanHangBeautify.Suggests.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BanHangBeautify.DatLichOnline
{
    public class OnlineBookingAppService : SPAAppServiceBase
    {
        IRepository<Tenant, int> _tenantRepository;
        public OnlineBookingAppService(
            IRepository<Tenant, int> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        public List<string> GetAllTenant()
        {
            List<string> result = new List<string>();
            result = _tenantRepository.GetAll().Where(x => x.IsDeleted == false && x.IsActive == true).Select(x => x.TenancyName.ToLower()).ToList();
            return result;
        }

        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVien(PagedRequestSuggestNhanVien input)
        {
            List<SuggestEmpolyeeExecuteServiceDto> result = new List<SuggestEmpolyeeExecuteServiceDto>();
            input.TenNhanVien = string.IsNullOrEmpty(input.TenNhanVien) ? "" : input.TenNhanVien;
            string connecStringInServer = $"data source=DESKTOP-8D36GBJ;initial catalog=SPADb;persist security info=True;user id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == input.TenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            var conn = new SqlConnection(@connectionString);
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand("prc_bookingOnline_SuggestNhanVien", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TenantId", tenant.Id));
                        cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
                        cmd.Parameters.Add(new SqlParameter("@IdDichVu", input.IdDichVu));
                        cmd.Parameters.Add(new SqlParameter("@TenNhanVien", input.TenNhanVien??""));
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
                    conn.Close();

                }

            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Open)
                {
                    result = new List<SuggestEmpolyeeExecuteServiceDto>();
                }
                conn.Close();
            }
            return result;
        }
        public async Task<List<SuggestDichVuBookingOnlineDto>> SuggestDichVu(PagedRequestSuggestDichVu input)
        {

            List<SuggestDichVuBookingOnlineDto> result = new List<SuggestDichVuBookingOnlineDto>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == input.TenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            string connecStringInServer = $"data source=DESKTOP-8D36GBJ;initial catalog=SPADb;persist security info=True;user id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            string sqlQuery = "";
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    sqlQuery += "SELECT ISNULL(nhh.TenNhomHang,N'Chưa phân loại') as TenNhomHangHoa,nhh.Color,dvqd.Id,hh.TenHangHoa as TenDichVu,dvqd.GiaBan as DonGia,hh.SoPhutThucHien,hh.Image FROM DM_DonViQuiDoi dvqd \n";
                    sqlQuery += "JOIN DM_HangHoa hh on hh.Id = dvqd.IdHangHoa \n";
                    sqlQuery += "LEFT JOIN DM_NhomHangHoa nhh on nhh.Id = hh.IdNhomHangHoa \n";
                    sqlQuery += string.Format($"WHERE hh.IdLoaiHangHoa != {LoaiHangHoaConst.HangHoa} \n");
                    sqlQuery += string.Format($"AND hh.TenantId = {tenant.Id} AND hh.IsDeleted = 0 \n");
                    if (!string.IsNullOrEmpty(input.TenNhomDichVu))
                    {
                        sqlQuery += string.Format($"AND LOWER(ISNULL(nhh.TenNhomHang,N'Chưa phân loại')) = LOWER(N'{input.TenNhomDichVu.ToLower()}') \n");
                    }
                    if (!string.IsNullOrEmpty(input.Keyword))
                    {
                        sqlQuery += string.Format($"AND (LOWER(nhh.TenNhomHang) LIKE N'%{input.Keyword.ToLower()}%' OR LOWER(hh.TenHangHoa) LIKE N'%{input.Keyword.ToLower()}%') \n");
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        // Insert into the database
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlQuery;
                            await cmd.ExecuteNonQueryAsync();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                string[] array = { "Data"};
                                var ds = new DataSet();
                                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    var data = ObjectHelper.FillCollection<SuggestDichVuBookingDto>(ds.Tables[0]);
                                    for (int i = 0; i < data.Count; i++)
                                    {
                                        var donGia = ds.Tables[0].Rows[i]["DonGia"].ToString();
                                        data[i].DonGia = decimal.Parse(string.IsNullOrEmpty(donGia) ? "0" : donGia);
                                    }
                                    var group = data.ToList().GroupBy(x =>new { x.TenNhomHangHoa,x.Color }).ToList();
                                    foreach (var item in group)
                                    {
                                        SuggestDichVuBookingOnlineDto dto = new SuggestDichVuBookingOnlineDto();
                                        dto.TenNhomHangHoa = item.Key.TenNhomHangHoa;
                                        dto.Color = item.Key.Color;
                                        dto.DanhSachDichVu = item.ToList();
                                        result.Add(dto);
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        result = new List<SuggestDichVuBookingOnlineDto>();
                    }
                }
            }

            return result;
        }
        public async Task<List<SuggestChiNhanhBooking>> SuggestChiNhanh(string tenantName)
        {
            List<SuggestChiNhanhBooking> result = new List<SuggestChiNhanhBooking>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            string connecStringInServer = $"data source=DESKTOP-8D36GBJ;initial catalog=SPADb;persist security info=True;user id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            string sqlQuery = "";
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    sqlQuery += "SELECT Id,TenChiNhanh,DiaChi,SoDienThoai,Logo FROM DM_ChiNhanh \n";
                    sqlQuery += string.Format($"WHERE TenantId = {tenant.Id} AND IsDeleted = 0 \n");
                    if (conn.State == ConnectionState.Open)
                    {
                        // Insert into the database
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlQuery;
                            await cmd.ExecuteNonQueryAsync();
                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                // Create a dataset to hold the retrieved data
                                DataSet dataSet = new DataSet();
                                dataAdapter.Fill(dataSet, "ChiNhanh");
                                DataTable dataTable = dataSet.Tables["ChiNhanh"];
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    SuggestChiNhanhBooking rdo = new SuggestChiNhanhBooking();
                                    rdo.Id = Guid.Parse(row["Id"].ToString());
                                    rdo.TenChiNhanh = row["TenChiNhanh"].ToString();
                                    rdo.Logo = row["Logo"].ToString();
                                    rdo.DiaChi = row["DiaChi"].ToString();
                                    rdo.SoDienThoai = row["SoDienThoai"].ToString();
                                    result.Add(rdo);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    result = new List<SuggestChiNhanhBooking>();
                }

            }
            return result;
        }
        public async Task<List<SuggestNhomHangHoaBookingOnlineDto>> SuggestNhomDichVu(string tenantName)
        {
            List<SuggestNhomHangHoaBookingOnlineDto> result = new List<SuggestNhomHangHoaBookingOnlineDto>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
            if (tenant == null)
            {
                return null;
            }
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            string connecStringInServer = $"data source=DESKTOP-8D36GBJ;initial catalog=SPADb;persist security info=True;user id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = connecStringInServer;
            }
            string sqlQuery = "";
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    sqlQuery += "SELECT ISNULL(nhh.Color,'#FFF') as Color, ISNULL(nhh.TenNhomHang,N'Chưa phân loại') as TenNhomHangHoa FROM DM_DonViQuiDoi dvqd \n";
                    sqlQuery += "JOIN DM_HangHoa hh on hh.Id = dvqd.IdHangHoa \n";
                    sqlQuery += "LEFT JOIN DM_NhomHangHoa nhh on nhh.Id = hh.IdNhomHangHoa \n";
                    sqlQuery += string.Format($"WHERE hh.IdLoaiHangHoa != {LoaiHangHoaConst.HangHoa} \n");
                    sqlQuery += string.Format($"AND hh.TenantId = {tenant.Id} AND hh.IsDeleted = 0 \n");
                    sqlQuery += "GROUP BY nhh.Color,nhh.TenNhomHang";
                    if (conn.State == ConnectionState.Open)
                    {
                        // Insert into the database
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlQuery;
                            await cmd.ExecuteNonQueryAsync();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                string[] array = { "Data" };
                                var ds = new DataSet();
                                ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    var data = ObjectHelper.FillCollection<SuggestNhomHangHoaBookingOnlineDto>(ds.Tables[0]);
                                    result = data;

                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        result = new List<SuggestNhomHangHoaBookingOnlineDto>();
                    }
                }
            }

            return result;
        }

        public async Task<ExecuteResultDto> CreateBooking(string tenantName, DatLichDto data)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName);
                if (tenant == null)
                {
                    return null;
                }
                string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
                string connecStringInServer = $"data source=DESKTOP-8D36GBJ;initial catalog=SPADb;persist security info=True;user id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = connecStringInServer;
                }
                DateTime bookingDate = DateTime.Parse(data.BookingDate);
                DateTime startTime = DateTime.Parse(bookingDate.ToString("yyyy-MM-dd") + " " + data.StartTime);
                data.EndTime = startTime.AddMinutes(data.SoPhutThucHien);
                using (var conn = new SqlConnection(@connectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (var cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO 
                                            Booking(Id,TenantId,TenKhachHang,SoDienThoai,IdChiNhanh,BookingDate,StartTime,EndTime,GhiChu,LoaiBooking,TrangThai,CreationTime,IsDeleted)
                                            VALUES(@Id,@TenantId,@TenKhachHang,@SoDienThoai,@IdChiNhanh,@BookingDate,@StartTime,@Endtime,@GhiChu,@LoaiBooking,@TrangThaiBooking,@CreationTime,@IsDeleted);";
                            cmd.CommandText += @"INSERT INTO BookingNhanVien(Id,TenantId,IdBooking,IdNhanVien,CreationTime,IsDeleted) 
                                                VALUES(@IdBookingNhanVien,@TenantId,@Id,@IdNhanVien,@CreationTime,@IsDeleted);
                                            ";
                            cmd.CommandText += @"INSERT INTO BookingService(Id,TenantId,IdBooking,IdDonViQuiDoi,CreationTime,IsDeleted) 
                                                VALUES(@IdBookingNhanVien,@TenantId,@Id,@IdDichVu,@CreationTime,@IsDeleted);
                                            ";

                            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("@IdBookingNhanVien", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("@IdBookingDichVu", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("@TenantId", tenant.Id);
                            cmd.Parameters.AddWithValue("@TenKhachHang", data.TenKhachHang);
                            cmd.Parameters.AddWithValue("@SoDienThoai", data.SoDienThoai);
                            cmd.Parameters.AddWithValue("@IdChiNhanh", data.IdChiNhanh);
                            cmd.Parameters.AddWithValue("@IdDichVu", data.IdDichVu);
                            cmd.Parameters.AddWithValue("@IdNhanVien", data.IdNhanVien);
                            cmd.Parameters.AddWithValue("@BookingDate", bookingDate);
                            cmd.Parameters.AddWithValue("@StartTime", startTime);
                            cmd.Parameters.AddWithValue("@EndTime", data.EndTime);
                            cmd.Parameters.AddWithValue("@GhiChu", data.GhiChu);
                            cmd.Parameters.AddWithValue("@LoaiBooking", 2);
                            cmd.Parameters.AddWithValue("@TrangThaiBooking", 1);
                            cmd.Parameters.AddWithValue("@CreationTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@IsDeleted", false);
                            await cmd.ExecuteNonQueryAsync();
                            conn.Close();
                        }
                        result.Message = "Đặt lịch thành công!";
                        result.Status = "success";
                    }

                }

            }
            catch (Exception ex)
            {
                result.Message = "Đặt lịch thất bại!";
                result.Status = "error";
                result.Detail = ex.Message;
            }
            return result;
        }
    }
}
public class PagedRequestSuggestDichVu
{
    [Required]
    public string TenantName { get; set; }
    public string TenNhomDichVu { get; set; }
    public string Keyword { get; set; }

}
public class PagedRequestSuggestNhanVien
{
    [Required]
    public string TenantName { get; set; }
    public Guid IdChiNhanh { get; set; }
    public Guid IdDichVu { get; set; }
    public string TenNhanVien { get; set; }

}

public class SuggestChiNhanhBooking : SuggestChiNhanh
{
    public string Logo { get; set; }
    public string DiaChi { get; set; }
    public string SoDienThoai { get; set; }
}
public class SuggestDichVuBookingDto : SuggestDichVuDto
{
    public string TenNhomHangHoa{set;get;}
    public string Image { get; set; }
    public string Color { get; set; }
    public float SoPhutThucHien { get; set; }
}
public class SuggestDichVuBookingOnlineDto 
{
    public string TenNhomHangHoa { set; get; }
    public string Color { get; set; }
    public List<SuggestDichVuBookingDto> DanhSachDichVu { set; get; }
}
public class SuggestNhomHangHoaBookingOnlineDto
{
    public string Color { set; get; }
    public string TenNhomHangHoa { get; set; }
}