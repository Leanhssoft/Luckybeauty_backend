﻿using Abp.EntityFrameworkCore;
using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.Common;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Checkin.Repository
{
    public class KHCheckInRespository : SPARepositoryBase<DM_KhachHang, Guid>, IKHCheckInRespository
    {
        private readonly SPADbContext _context;
        public KHCheckInRespository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            using (var command = CreateCommand("spGetListCustomerChecking"))
            {
                command.Parameters.Add(new SqlParameter("@TenantId", tenantId ?? 1));
                command.Parameters.Add(new SqlParameter("@TextSearch", input.keyword ?? ""));
                command.Parameters.Add(new SqlParameter("@CurrentPage", input.SkipCount));
                command.Parameters.Add(new SqlParameter("@PageSize", input.MaxResultCount));


                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    List<PageKhachHangCheckingDto> xx = new();

                    string[] array = { "Data" };
                    var ds = new DataSet();
                    ds.Load(dataReader, LoadOption.OverwriteChanges, array);
                    var ddd = ds.Tables;

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectHelper.FillCollection<PageKhachHangCheckingDto>(ds.Tables[0]); ;
                    }
                }
                return new List<PageKhachHangCheckingDto>();
            }
        }
        public async Task<List<PageKhachHangCheckingDto>> GetListCustomerChecking2(PagedKhachHangResultRequestDto input, int? tenantId)
        {
            var sqlPr = new List<SqlParameter>();
            sqlPr.Add(new SqlParameter("@TenantId", tenantId ?? 1));
            sqlPr.Add(new SqlParameter("@TextSearch", input.keyword ?? ""));
            sqlPr.Add(new SqlParameter("@CurrentPage", input.SkipCount));
            sqlPr.Add(new SqlParameter("@PageSize", input.MaxResultCount));

            var data = await _context.Database.SqlQueryRaw<PageKhachHangCheckingDto>("dbo.spGetListCustomerChecking @TenantId, @TextSearch, @CurrentPage, @PageSize", sqlPr.ToArray()).ToListAsync();
            return data;
        }
    }
}