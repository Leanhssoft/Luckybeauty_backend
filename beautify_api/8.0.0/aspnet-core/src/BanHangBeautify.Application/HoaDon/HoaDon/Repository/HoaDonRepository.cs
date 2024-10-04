using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon.Repository
{
    public class HoaDonRepository : SPARepositoryBase<BH_HoaDon, Guid>, IHoaDonRepository
    {
        public HoaDonRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<string> FnGetMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon)
        {
            using var command = CreateCommand("select dbo.fnGetMaHoaDon(@TenantId,@IdChiNhanh,@IdLoaiChungTu,@NgayLapHoaDon) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
            command.Parameters.Add(new SqlParameter("@NgayLapHoaDon", ngayLapHoaDon ?? DateTime.Now));
            var code = (await command.ExecuteScalarAsync()).ToString();
            return code;
        }
        public async Task<double> GetMaxNumber_ofMaHoaDon(int tenantId, Guid? idChiNhanh, int idLoaiChungTu, DateTime? ngayLapHoaDon)
        {
            using var command = CreateCommand("select dbo.fnGetMaxNumber_ofMaHoaDon(@TenantId,@IdChiNhanh,@IdLoaiChungTu,@NgayLapHoaDon) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", idChiNhanh ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTu", idLoaiChungTu));
            command.Parameters.Add(new SqlParameter("@NgayLapHoaDon", ngayLapHoaDon ?? DateTime.Now));
            var data = await command.ExecuteScalarAsync();
            return Convert.ToDouble(data);
        }
        public async Task<PagedResultDto<PageHoaDonDto>> GetListHoaDon(HoaDonRequestDto param, int? tenantId = 1)
        {
            string idChiNhanhs = string.Empty, idLoaiChungTus = string.Empty,
                trangThaiHDs = string.Empty, trangThaiNos = string.Empty;
            if (param.IdChiNhanhs != null && param.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", param.IdChiNhanhs);
            }
            if (param.IdLoaiChungTus != null && param.IdLoaiChungTus.Count > 0)
            {
                idLoaiChungTus = string.Join(",", param.IdLoaiChungTus);
            }
            if (param.TrangThais != null && param.TrangThais.Count > 0)
            {
                trangThaiHDs = string.Join(",", param.TrangThais);
            }
            if (param.TrangThaiNos != null && param.TrangThaiNos.Count > 0)
            {
                trangThaiNos = string.Join(",", param.TrangThaiNos);
            }
            using var command = CreateCommand("spGetListHoaDon");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdUserLogin", param.IdUserLogin));
            command.Parameters.Add(new SqlParameter("@IdLoaiChungTus", idLoaiChungTus));
            command.Parameters.Add(new SqlParameter("@TrangThaiHDs", trangThaiHDs));
            command.Parameters.Add(new SqlParameter("@TrangThaiNos", trangThaiNos));
            command.Parameters.Add(new SqlParameter("@DateFrom", param.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DateTo", param.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", param.TextSearch ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CurrentPage", param.CurrentPage));
            command.Parameters.Add(new SqlParameter("@PageSize", param.PageSize));
            command.Parameters.Add(new SqlParameter("@ColumnSort", param.ColumnSort));
            command.Parameters.Add(new SqlParameter("@TypeSort", param.TypeSort));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageHoaDonDto>(ds.Tables[0]);
                    return new PagedResultDto<PageHoaDonDto>()
                    {
                        TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<PageHoaDonDto>();
        }

        public async Task<List<PageHoaDonDto>> GetInforHoaDon_byId(Guid id)
        {
            using var command = CreateCommand("spGetInforHoaDon_byId");
            command.Parameters.Add(new SqlParameter("@Id", id));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageHoaDonDto>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<PageHoaDonDto>();
        }
        public async Task<List<PageHoaDonChiTietDto>> GetChiTietHoaDon_byIdHoaDon(Guid idHoaDon)
        {
            using var command = CreateCommand("spGetChiTietHoaDon_byIdHoaDon");
            command.Parameters.Add(new SqlParameter("@IdHoaDon", idHoaDon));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<PageHoaDonChiTietDto>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<PageHoaDonChiTietDto>();
        }

        public PageHoaDonChiTietDto GetChiTietHoaDon_byIdChiTiet(Guid idChiTiet)
        {
            var db = GetDbContext();
            var thisCTHD = (db.Set<BH_HoaDon_ChiTiet>()).Where(x => x.Id == idChiTiet);
            var data = (from ct in thisCTHD
                       join qd in db.Set<DM_DonViQuiDoi>() on ct.IdDonViQuyDoi equals qd.Id
                       join hh in db.Set<DM_HangHoa>() on qd.IdHangHoa equals hh.Id
                       select new PageHoaDonChiTietDto
                       {
                           TenantId = ct.TenantId,
                           Id = ct.Id,
                           IdHoaDon = ct.IdHoaDon,
                           IdDonViQuyDoi = ct.IdDonViQuyDoi,
                           IdChiTietHoaDon = ct.IdChiTietHoaDon,
                           IdHangHoa = hh.Id,
                           MaHangHoa = qd.MaHangHoa,
                           TenHangHoa = hh.TenHangHoa,
                           TenDonViTinh = qd.TenDonViTinh,
                           GiaBan = qd.GiaBan,
                           STT = ct.STT,
                           SoLuong = ct.SoLuong,
                           DonGiaTruocCK = ct.DonGiaTruocCK,
                           PTChietKhau = ct.PTChietKhau,
                           TienChietKhau = ct.TienChietKhau,
                           DonGiaSauCK = ct.DonGiaSauCK,
                           PTThue = ct.PTThue,
                           TienThue = ct.TienThue,
                           DonGiaSauVAT = ct.DonGiaSauVAT,
                           ThanhTienTruocCK = ct.ThanhTienTruocCK,
                           ThanhTienSauCK = ct.ThanhTienSauCK,
                           ThanhTienSauVAT = ct.ThanhTienSauVAT,
                           GhiChu = ct.GhiChu,
                           TrangThai = ct.TrangThai,
                           TonLuyKe = ct.TonLuyKe
                       }).ToList().FirstOrDefault();
            return data;
        }
        public async Task<List<ChiTietSuDungGDV>> GetChiTiet_SuDungGDV_ofCustomer(ParamSearchNhatKyGDV param)
        {
            string idChiNhanhs = string.Empty, trangThaiConBuoi = string.Empty;
            if (param.IdChiNhanhs != null && param.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", param.IdChiNhanhs);
            }
            if (param.TrangThais != null && param.TrangThais.Count > 0)
            {
                trangThaiConBuoi = string.Join(",", param.TrangThais);
            }

            using var command = CreateCommand("GetChiTiet_SuDungGDV_ofCustomer");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@IdCustomer", param?.IdCustomer ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", param?.TextSearch ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DateFrom", param?.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DateTo", param?.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TrangThaiConBuoi", trangThaiConBuoi));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<ChiTietSuDungGDV>(ds.Tables[0]);
                    return data;
                }
            }
            return new List<ChiTietSuDungGDV>();
        }
        public async Task<PagedResultDto<ChiTietNhatKySuDungGDVDto>> GetNhatKySuDungGDV_ofKhachHang(ParamSearchNhatKyGDV param)
        {
            using var command = CreateCommand("GetNhatKySuDungGDV_ofKhachHang");
            command.Parameters.Add(new SqlParameter("@IdCustomer", param?.IdCustomer ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IdGoiDichVu", param?.IdGoiDichVu?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TextSearch", param?.TextSearch ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CurrentPage", param?.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", param?.PageSize ?? 10));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = ObjectHelper.FillCollection<ChiTietNhatKySuDungGDVDto>(ds.Tables[0]);
                    return new PagedResultDto<ChiTietNhatKySuDungGDVDto>()
                    {
                        TotalCount = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = data
                    };
                }
            }
            return new PagedResultDto<ChiTietNhatKySuDungGDVDto>();
        }

        public async Task<bool> CheckGDV_DaSuDung(Guid idGoiDV)
        {
            using var command = CreateCommand("select dbo.FnCheckGDV_DaSuDung (@IdGoiDV) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@IdGoiDV", idGoiDV));
            var data = await command.ExecuteScalarAsync();
            return data != null && Convert.ToBoolean(data);
        } 
        public async Task<bool> CheckChiTietGDV_DaSuDung(Guid idGoiDV)
        {
            using var command = CreateCommand("select dbo.FnCheckChiTietGDV_DaSuDung (@IdChiTietGDV) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@IdChiTietGDV", idGoiDV));
            var data = await command.ExecuteScalarAsync();
            return data != null && Convert.ToBoolean(data);
        } 
        public async Task<double> GetSoDuTheGiaTri_ofKhachHang(Guid idKhachHang)
        {
            using var command = CreateCommand("select dbo.FnGetSoDuTheGiaTri_ofKhachHang (@IdKhachHang) ", System.Data.CommandType.Text);
            command.Parameters.Add(new SqlParameter("@IdKhachHang", idKhachHang));
            var data = await command.ExecuteScalarAsync();
            return Convert.ToDouble(data);
        }
    }
}
