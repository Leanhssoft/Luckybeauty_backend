

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HangHoa.HangHoa.Exporting;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.NewFolder;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Transactions;
using static BanHangBeautify.Common.CommonClass;


namespace BanHangBeautify.HangHoa.HangHoa
{
    [AbpAuthorize(PermissionNames.Pages_DM_HangHoa)]
    public class HangHoaAppService : SPAAppServiceBase, IHangHoaAppService
    {
        private readonly IRepository<DM_HangHoa, Guid> _dmHangHoa;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _dmDonViQuiDoi;
        private readonly IHangHoaRepository _repository;
        private readonly IHangHoaExcelExporter _hangHoaExcelExporter;
        public HangHoaAppService(IRepository<DM_HangHoa, Guid> repository,
            IHangHoaRepository productRepo,
            IRepository<DM_DonViQuiDoi, Guid> dvqd,
            IHangHoaExcelExporter hangHoaExcelExporter
            )
        {
            _dmHangHoa = repository;
            _dmDonViQuiDoi = dvqd;
            _repository = productRepo;
            _hangHoaExcelExporter = hangHoaExcelExporter;
        }

        public string FormatMaHangHoa(string firstChar, float? maxVal = 0)
        {
            if (maxVal < 10)
            {
                return string.Concat(firstChar, "0", maxVal);
            }
            else
            {
                return string.Concat(firstChar, maxVal);
            }
        }
        [AbpAuthorize(PermissionNames.Pages_DM_HangHoa_Create, PermissionNames.Pages_DM_HangHoa_Edit)]
        public async Task<CreateOrEditHangHoaDto> CreateOrEdit(CreateOrEditHangHoaDto dto)
        {
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (findHangHoa == null)
            {
                return await Create(dto);
            }
            else
            {
                return await Edit(dto, findHangHoa);
            }
        }
        [NonAction]
        public async Task<CreateOrEditHangHoaDto> Create(CreateOrEditHangHoaDto dto)
        {
            List<DM_DonViQuiDoi> lstDVT = new();
            DM_HangHoa hangHoa = ObjectMapper.Map<DM_HangHoa>(dto);
            Guid productId = Guid.NewGuid();
            hangHoa.Id = productId;
            hangHoa.TenantId = AbpSession.TenantId ?? 1;
            hangHoa.CreatorUserId = AbpSession.UserId;
            hangHoa.SoPhutThucHien = dto.SoPhutThucHien;
            hangHoa.CreationTime = DateTime.Now;
            hangHoa.LastModificationTime = DateTime.Now;
            hangHoa.LastModifierUserId = AbpSession.UserId;
            if (dto.DonViQuiDois != null && dto.DonViQuiDois.Count > 0)
            {
                float? max = 1;
                foreach (var item in dto.DonViQuiDois)
                {
                    string maHangHoa = item.MaHangHoa;
                    if (string.IsNullOrEmpty(maHangHoa))
                    {
                        MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                        max = objMax.MaxVal;
                        maHangHoa = FormatMaHangHoa(objMax.FirstStr, max);
                    }

                    DM_DonViQuiDoi dvt = ObjectMapper.Map<DM_DonViQuiDoi>(item);
                    dvt.Id = Guid.NewGuid();
                    dvt.TenantId = hangHoa.TenantId;
                    dvt.IdHangHoa = productId;
                    dvt.MaHangHoa = maHangHoa;
                    lstDVT.Add(dvt);
                    max += 1;
                }
            }
            else
            {
                MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                DM_DonViQuiDoi dvt = new()
                {
                    Id = Guid.NewGuid(),
                    IdHangHoa = productId,
                    TenantId = hangHoa.TenantId,
                    MaHangHoa = FormatMaHangHoa(objMax.FirstStr, objMax.MaxVal),
                    TenDonViTinh = string.Empty,
                };
                lstDVT.Add(dvt);
            }
            await _dmHangHoa.InsertAsync(hangHoa);
            await _dmDonViQuiDoi.InsertRangeAsync(lstDVT);

            hangHoa.DonViQuiDois = lstDVT;
            var result = ObjectMapper.Map<CreateOrEditHangHoaDto>(hangHoa);
            return result;
        }

        [NonAction]
        public async Task<CreateOrEditHangHoaDto> Edit(CreateOrEditHangHoaDto dto, DM_HangHoa hangHoa)
        {
            #region compare dvt & update IsDeleted = true if not exists
            var dvt = _dmDonViQuiDoi.GetAllList(x => x.IdHangHoa == hangHoa.Id);
            var idOlds = dvt.Select(x => x.Id).ToList();
            var idNews = dto.DonViQuiDois.Select(x => x.Id);
            var idDeletes = (from idOld in idOlds
                             join idNew in idNews on idOld equals idNew
                             into tbl
                             from de in tbl.DefaultIfEmpty()
                             where de == Guid.Empty
                             select idOld).ToList();
            _dmDonViQuiDoi.GetAllList(x => idDeletes.Contains(x.Id)).ForEach(x => x.IsDeleted = true);
            #endregion
            hangHoa.IdNhomHangHoa = dto.IdNhomHangHoa;
            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TenHangHoa_KhongDau = dto.TenHangHoa_KhongDau;
            hangHoa.MoTa = dto.MoTa;
            hangHoa.TrangThai = dto.TrangThai;
            hangHoa.LastModificationTime = DateTime.Now;
            hangHoa.LastModifierUserId = AbpSession.UserId;
            await _dmHangHoa.UpdateAsync(hangHoa);

            foreach (var item in dto.DonViQuiDois)
            {
                DM_DonViQuiDoi objDVT = _dmDonViQuiDoi.FirstOrDefault(item.Id);
                if (objDVT != null)
                {
                    // update
                    string maHangHoa = item.MaHangHoa;
                    if (string.IsNullOrEmpty(maHangHoa))
                    {
                        MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                        maHangHoa = FormatMaHangHoa(objMax.FirstStr, objMax.MaxVal);
                    }
                    objDVT.MaHangHoa = maHangHoa;
                    objDVT.TenDonViTinh = item.TenDonViTinh;
                    objDVT.TyLeChuyenDoi = item.TyLeChuyenDoi;
                    objDVT.GiaBan = item.GiaBan;
                    await _dmDonViQuiDoi.UpdateAsync(objDVT);
                }
                else
                {
                    // insert
                    string maHangHoa = item.MaHangHoa;
                    if (string.IsNullOrEmpty(maHangHoa))
                    {
                        MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                        maHangHoa = FormatMaHangHoa(objMax.FirstStr, objMax.MaxVal);
                    }
                    DM_DonViQuiDoi dvtNew = ObjectMapper.Map<DM_DonViQuiDoi>(item);
                    dvtNew.MaHangHoa = maHangHoa;
                    dvtNew.IdHangHoa = hangHoa.Id;
                    dvtNew.TenantId = hangHoa.TenantId;
                    dvtNew.LaDonViTinhChuan = item.LaDonViTinhChuan;
                    await _dmDonViQuiDoi.InsertAsync(dvtNew);
                    hangHoa.DonViQuiDois.Add(dvtNew);// used to return
                }
            }

            // only return dvt not delete (todo)
            var result = ObjectMapper.Map<CreateOrEditHangHoaDto>(hangHoa);
            return result;
        }
        public async Task<HangHoaDto> GetDetailProduct(Guid idDonViQuyDoi)
        {
            return await _repository.GetDetailProduct(idDonViQuyDoi, AbpSession.TenantId ?? 1);
        }
        public async Task<PagedResultDto<DM_HangHoa>> GetAll(HangHoaRequestDto input)
        {
            PagedResultDto<DM_HangHoa> result = new PagedResultDto<DM_HangHoa>();
            var lstHangHoa = await _dmHangHoa.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstHangHoa.Count();
            if (!string.IsNullOrEmpty(input.TextSearch))
            {
                lstHangHoa = lstHangHoa.Where(x => x.TenHangHoa.Contains(input.TextSearch) || x.TenHangHoa.Contains(input.TextSearch)).ToList();
            }
            if (input.CurrentPage > 0)
            {
                input.CurrentPage *= 10;
            }
            result.Items = lstHangHoa.Skip(input.CurrentPage ?? 0).Take(input.CurrentPage ?? 10).ToList();
            return result;
        }
        [HttpPost]
        public async Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaRequestDto input)
        {
            return await _repository.GetDMHangHoa(input, AbpSession.TenantId ?? 1);
        }
        [HttpPost]
        public async Task<List<HangHoaGroupTheoNhomDto>> GetDMHangHoa_groupByNhom(HangHoaRequestDto input)
        {
            var data = await _repository.GetDMHangHoa(input, AbpSession.TenantId ?? 1);
            var dataGroup = data.Items.GroupBy(x => new { x.IdNhomHangHoa, x.TenNhomHang, x.Color })
                .Select(x => new HangHoaGroupTheoNhomDto
                {
                    IdNhomHangHoa = x.Key.IdNhomHangHoa,
                    TenNhomHang = x.Key.TenNhomHang,
                    Color = x.Key.Color,
                    HangHoas = x.ToList()
                }).ToList();
            return dataGroup;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_DM_HangHoa_Delete)]
        public async Task<CreateOrEditHangHoaDto> Delete(Guid id)
        {
            CreateOrEditHangHoaDto result = new();
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == id);
            if (findHangHoa != null)
            {
                findHangHoa.IsDeleted = true;
                findHangHoa.TrangThai = 0;
                findHangHoa.DeletionTime = DateTime.Now;
                findHangHoa.DeleterUserId = AbpSession.UserId;
                _dmHangHoa.Update(findHangHoa);

                _dmDonViQuiDoi.GetAllList(x => x.IdHangHoa == id).ForEach(x => x.IsDeleted = true);

                result = ObjectMapper.Map<CreateOrEditHangHoaDto>(findHangHoa);
            }
            return result;
        }
        [HttpPost]
        public async Task<CreateOrEditHangHoaDto> RestoreProduct(Guid idHangHoa)
        {
            CreateOrEditHangHoaDto result = new();
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == idHangHoa);
            if (findHangHoa != null)
            {
                findHangHoa.IsDeleted = false;
                findHangHoa.TrangThai = 1;
                findHangHoa.LastModificationTime = null;
                findHangHoa.LastModifierUserId = AbpSession.UserId;
                _dmHangHoa.Update(findHangHoa);

                _dmDonViQuiDoi.GetAllList(x => x.IdHangHoa == idHangHoa).ForEach(x => x.IsDeleted = false);

                result = ObjectMapper.Map<CreateOrEditHangHoaDto>(findHangHoa);
            }
            return result;
        }

        [HttpGet]
        public async Task<bool> CheckExistsMaHangHoa(string mahanghoa, Guid? id = null)
        {
            if (id != null && id != Guid.Empty)
            {
                var lst = await _dmDonViQuiDoi.GetAllListAsync(x => x.Id != id && x.MaHangHoa.ToUpper() == mahanghoa.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _dmDonViQuiDoi.GetAllListAsync(x => x.MaHangHoa.ToUpper() == mahanghoa.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_DM_HangHoa_Export)]
        public async Task<FileDto> ExportToExcel(HangHoaRequestDto input)
        {
            input.CurrentPage = 1;
            input.PageSize = int.MaxValue;
            var data = await _repository.GetDMHangHoa(input, AbpSession.TenantId ?? 1);
            return _hangHoaExcelExporter.ExportHangHoaToExcel(data.Items.ToList());
        }
        [HttpPost]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        [AbpAuthorize(PermissionNames.Pages_DM_HangHoa_Import)]
        public async Task<ExecuteResultDto> ImportExcel(FileUpload file)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                int countImportData = 0;
                int countImportLoi = 0;
                if (file.Type == ".xlsx")
                {
                    using (MemoryStream stream = new MemoryStream(file.File))
                    {
                        using (var package = new ExcelPackage())
                        {
                            package.Load(stream);
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is on the first worksheet
                            int rowCount = worksheet.Dimension.Rows;

                            for (int row = 3; row <= rowCount; row++) // Assuming the first row is the header row
                            {
                                CreateOrEditHangHoaDto data = new CreateOrEditHangHoaDto();
                                data.Id = Guid.NewGuid();
                                data.TenHangHoa = worksheet.Cells[row, 3].Value?.ToString();
                                switch (worksheet.Cells[row, 6].Value?.ToString())
                                {
                                    case "HH":
                                        data.IdLoaiHangHoa = 1;
                                        break;
                                    case "DV":
                                        data.IdLoaiHangHoa = 2;
                                        break;
                                    case "CB":
                                        data.IdLoaiHangHoa = 3;
                                        break;
                                    default:
                                        data.IdLoaiHangHoa = 1;
                                        break;
                                }
                                string maHangHoa = worksheet.Cells[row, 2].Value?.ToString();
                                if (string.IsNullOrEmpty(maHangHoa))
                                {
                                    MaxCodeDto objMax = await _repository.SpGetProductCode(data.IdLoaiHangHoa, AbpSession.TenantId??1);
                                    float? max = objMax.MaxVal;
                                    maHangHoa = FormatMaHangHoa(objMax.FirstStr, max);
                                }
                                data.DonViQuiDois = new List<DonViQuiDoiDto>()
                                {

                                    new DonViQuiDoiDto(){
                                        GiaBan = float.Parse(worksheet.Cells[row,4].Value?.ToString()??"0"),
                                        LaDonViTinhChuan = 1,
                                        TyLeChuyenDoi = 1,
                                        MaHangHoa = maHangHoa,

                                    }
                                };
                                float soPhutThucHien = float.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? "0");
                                if (soPhutThucHien > 0)
                                {
                                    data.SoPhutThucHien = soPhutThucHien;
                                }
                                data.MoTa = worksheet.Cells[row, 7].Value?.ToString();
                                await Create(data);
                                countImportData++;
                            }

                        }
                        if (countImportData > 0)
                        {
                            result.Message = "Nhập dữ liệu thành công: " + countImportData.ToString() + " bản ghi! Lỗi: " + countImportLoi.ToString();
                            result.Status = "success";
                        }
                        else
                        {
                            result.Message = "Không có dữ liệu được nhập";
                            result.Status = "info";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "Có lỗi sảy ra trong quá trình import dữ liệu";
                result.Status = "error";
                result.Detail = ex.Message;
            }

            return result;
        }
    }
}
