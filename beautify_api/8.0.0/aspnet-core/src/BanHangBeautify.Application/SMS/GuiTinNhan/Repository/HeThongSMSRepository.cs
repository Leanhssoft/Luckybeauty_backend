using Abp.Application.Services.Dto;
using Abp.EntityFrameworkCore;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.SMS.GuiTinNhan.Repository
{
    public class HeThongSMSRepository : SPARepositoryBase<HeThong_SMS, Guid>, IHeThongSMSRepository
    {
        public HeThongSMSRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<PagedResultDto<CreateOrEditHeThongSMSDto>> GetListSMS(ParamSearch input)
        {
            if (input == null)
            {
                return new PagedResultDto<CreateOrEditHeThongSMSDto>();
            }
            string idChiNhanhs = string.Empty, trangThais = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.TrangThais != null && input.TrangThais.Count > 0)
            {
                trangThais = string.Join(",", input.TrangThais);
            }
            using var command = CreateCommand("spGetListSMS");
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@TrangThais", trangThais));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? string.Empty));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage ?? 0));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize ?? 50));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return new PagedResultDto<CreateOrEditHeThongSMSDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = ObjectHelper.FillCollection<CreateOrEditHeThongSMSDto>(ds.Tables[0])
                    };
                }
            }
            return new PagedResultDto<CreateOrEditHeThongSMSDto>();
        }
        public async Task<List<CustomerWithZOA>> JqAutoCustomer_byIdLoaiTin(ParamSearchSMS input, int? idLoaiTin = 1)
        {
            if (input == null)
            {
                return new List<CustomerWithZOA>();
            }
            string idChiNhanhs = string.Empty, trangThais = string.Empty, hinhThucGui = string.Empty;
            int isUserZalo = 0;// 1. nếu khách hàng có tài khoản Zalo, và quan tâm đến Cửa hàng
            if (input.TrangThais != null && input.TrangThais.Count > 0)
            {
                isUserZalo = Int32.Parse(input.TrangThais[0]);
            }
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.TrangThais != null && input.TrangThais.Count > 0)
            {
                trangThais = string.Join(",", input.TrangThais);
            }
            if (input.HinhThucGuiTins != null && input.HinhThucGuiTins.Count > 0)
            {
                hinhThucGui = string.Join(",", input.HinhThucGuiTins);
            }
            using var command = CreateCommand("spJqAutoCustomer_byIdLoaiTin");
            command.Parameters.Add(new SqlParameter("@IdLoaiTin", idLoaiTin));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@TrangThais", trangThais));
            command.Parameters.Add(new SqlParameter("@HinhThucGuiTins", hinhThucGui));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? ""));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IsUserZalo", isUserZalo));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ObjectHelper.FillCollection<CustomerWithZOA>(ds.Tables[0]);
                }
            }
            return new List<CustomerWithZOA>();
        }
        public async Task<PagedResultDto<PageKhachHangSMSDto>> GetListCustomer_byIdLoaiTin(ParamSearchSMS input, int? idLoaiTin = 1)
        {
            if (input == null)
            {
                return new PagedResultDto<PageKhachHangSMSDto>();
            }
            string idChiNhanhs = string.Empty, trangThais = string.Empty, hinhThucGui = string.Empty;
            if (input.IdChiNhanhs != null && input.IdChiNhanhs.Count > 0)
            {
                idChiNhanhs = string.Join(",", input.IdChiNhanhs);
            }
            if (input.TrangThais != null && input.TrangThais.Count > 0)
            {
                trangThais = string.Join(",", input.TrangThais);
            }
            if (input.HinhThucGuiTins != null && input.HinhThucGuiTins.Count > 0)
            {
                hinhThucGui = string.Join(",", input.HinhThucGuiTins);
            }
            using var command = CreateCommand("spGetListCustomer_byIdLoaiTin");
            command.Parameters.Add(new SqlParameter("@IdLoaiTin", idLoaiTin));
            command.Parameters.Add(new SqlParameter("@IdChiNhanhs", idChiNhanhs));
            command.Parameters.Add(new SqlParameter("@TrangThais", trangThais));
            command.Parameters.Add(new SqlParameter("@HinhThucGuiTins", hinhThucGui));
            command.Parameters.Add(new SqlParameter("@IsFilterCustomer", input?.IsFilterCustomer ?? false));
            command.Parameters.Add(new SqlParameter("@LoaiUser_CoTheGuiTin", input?.LoaiUser_CoTheGuiTin ?? 0));
            command.Parameters.Add(new SqlParameter("@TextSearch", input.TextSearch ?? ""));
            command.Parameters.Add(new SqlParameter("@FromDate", input.FromDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ToDate ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CurrentPage", input.CurrentPage));
            command.Parameters.Add(new SqlParameter("@PageSize", input.PageSize));

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                string[] array = { "Data" };
                var ds = new DataSet();
                ds.Load(dataReader, LoadOption.OverwriteChanges, array);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return new PagedResultDto<PageKhachHangSMSDto>()
                    {
                        TotalCount = int.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString()),
                        Items = ObjectHelper.FillCollection<PageKhachHangSMSDto>(ds.Tables[0])
                    };
                }
            }
            return new PagedResultDto<PageKhachHangSMSDto>();
        }

        public async Task<int> InsertNhatKyGuiTinSMS(NhatKyGuiTinSMSDto input, int tenantId)
        {
            if (input == null)
            {
                return 0;
            }
            using var command = CreateCommand("spInsertNhatKyGuiTinSMS");
            command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
            command.Parameters.Add(new SqlParameter("@IdHeThongSMS", input.IdHeThongSMS));
            command.Parameters.Add(new SqlParameter("@IdCustomer", input.IdKhachHang));
            command.Parameters.Add(new SqlParameter("@IdChiNhanh", input.IdChiNhanh));
            command.Parameters.Add(new SqlParameter("@IdLoaiTin", input.IdLoaiTin));
            command.Parameters.Add(new SqlParameter("@FromDate", input.ThoiGianTu));
            command.Parameters.Add(new SqlParameter("@ToDate", input.ThoiGianDen));
            return await command.ExecuteNonQueryAsync();
        }
    }
}
