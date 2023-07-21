using Abp.Domain.Repositories;
using Abp.Runtime.Security;
using BanHangBeautify.Common;
using BanHangBeautify.Common.Consts;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.Suggests.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DatLichOnline
{
    public class OnlineBookingAppService :SPAAppServiceBase
    {
        IRepository<Tenant, int> _tenantRepository;
        public OnlineBookingAppService(IRepository<Tenant, int> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        public List<string> GetAllTenant()
        {
            List<string> result = new List<string>();
            result = _tenantRepository.GetAll().Where(x => x.IsDeleted == false && x.IsActive == true).Select(x=>x.TenancyName.ToLower()).ToList();
            return result;
        }

        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVien(string tenantName,string idChiNhanh)
        {
            List<SuggestEmpolyeeExecuteServiceDto> result = new List<SuggestEmpolyeeExecuteServiceDto>();
            string connecStringInServer = $"data source=DESKTOP-8D36GBJ;initial catalog=SPADb;persist security info=True;user id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework;Encrypt=False";
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x=>x.TenancyName.ToLower() ==tenantName.ToLower());
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
                    using (var cmd = new SqlCommand("prc_bookingOnline_SuggestNhanVien",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TenantId", tenant.Id));
                        cmd.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh));
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

            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Open)
                {
                    result = new List<SuggestEmpolyeeExecuteServiceDto>();
                }
            }
            return result;
        }
        public async Task<List<SuggestDichVuDto>> SuggestDichVu(string tenantName)
        {
            List<SuggestDichVuDto> result = new List<SuggestDichVuDto>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
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
                    sqlQuery += "SELECT dvqd.Id,hh.TenHangHoa,dvqd.GiaBan FROM DM_DonViQuiDoi dvqd \n";
                    sqlQuery += "JOIN DM_HangHoa hh on hh.Id = dvqd.IdHangHoa \n";
                    sqlQuery += string.Format($"WHERE hh.IdLoaiHangHoa != {LoaiHangHoaConst.HangHoa} \n");
                    sqlQuery += string.Format($"AND hh.TenantId = {tenant.Id} AND hh.IsDeleted = 0 \n");
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

                                // Fill the dataset with the results of the query
                                dataAdapter.Fill(dataSet, "DichVu");

                                // Access the retrieved data (assuming you have a table called "YourTable")
                                DataTable dataTable = dataSet.Tables["DichVu"];
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    SuggestDichVuDto rdo = new SuggestDichVuDto();
                                    rdo.Id = Guid.Parse(row["Id"].ToString());
                                    rdo.TenDichVu = row["TenHangHoa"].ToString();
                                    rdo.DonGia = decimal.Parse(row["GiaBan"].ToString()??"0");
                                    result.Add(rdo);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        result = new List<SuggestDichVuDto>();
                    }
                }
            }

            return result;
        }
        public async Task<List<SuggestChiNhanhBooking>> SuggestChiNhanh(string tenantName)
        {
            List<SuggestChiNhanhBooking> result = new List<SuggestChiNhanhBooking>();
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
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

        public async Task<string> Booking(string tenantName)
        {
            string result = "";
            var tenant = await TenantManager.Tenants.FirstOrDefaultAsync(x => x.TenancyName.ToLower() == tenantName.ToLower());
            string connectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString);
            var conn = new SqlConnection(@connectionString);
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO Booking" +
                                                "(" +
                                                    "Id," +
                                                    "TenantId," +
                                                    "IdKhachHang," +
                                                    "IdChiNhanh, " +
                                                    "TenKhachHang," +
                                                    "SoDienThoai," +
                                                    "StartTime," +
                                                    "EndTime," +
                                                    "BookingDate," +
                                                    "LoaiBooking," +
                                                    "TrangThai," +
                                                    "GhiChu," +
                                                    "CreationTime," +
                                                    "IsDeleted" +
                                                ") " +
                                          "VALUES (" +
                                                  "@Id," +
                                                  "@TenantId," +
                                                  "@IdKhachHang, " +
                                                  "@IdChiNhanh, " +
                                                  "@TenKhachHang, " +
                                                  "@SoDienThoai, " +
                                                  "@StartTime, " +
                                                  "@EndTime, " +
                                                  "@BookingDate, " +
                                                  "@LoaiBooking, " +
                                                  "@TrangThai, " +
                                                  "@GhiChu, " +
                                                  "@CreationTime, " +
                                                  "@IsDeleted" +
                                          ")";
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@TenantId", tenant.Id);
                        cmd.Parameters.AddWithValue("@MaNguon", "P01");
                        cmd.Parameters.AddWithValue("@TenNguon", "Mạnh test thêm");
                        cmd.Parameters.AddWithValue("@TrangThai", 1);
                        cmd.Parameters.AddWithValue("@CreationTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsDeleted", false);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    result = "Connection opened successfully!";

                }

            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Open)
                {
                    result = "Failed to open the connection";
                }
            }
            return result;
        }
    }
}
public class SuggestChiNhanhBooking: SuggestChiNhanh
{
    public string Logo { get; set; }
    public string DiaChi { get; set; }
    public string SoDienThoai { get; set; }
}