

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.NewFolder;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static BanHangBeautify.Configuration.Common.CommonClass;
using static BanHangBeautify.Configuration.Common.ObjectHelper;

namespace BanHangBeautify.HangHoa.HangHoa
{
    [AbpAuthorize(PermissionNames.Pages_DM_HangHoa)]
    public class HangHoaAppService : SPAAppServiceBase, IHangHoaAppService
    {
        private readonly IRepository<DM_HangHoa, Guid> _dmHangHoa;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _dmDonViQuiDoi;
        private readonly IHangHoaRepository _repository;
        private readonly IExcelBase _excelBase;
        public HangHoaAppService(IRepository<DM_HangHoa, Guid> repository,
            IHangHoaRepository productRepo,
            IRepository<DM_DonViQuiDoi, Guid> dvqd,
            IExcelBase excelBase
            )
        {
            _dmHangHoa = repository;
            _dmDonViQuiDoi = dvqd;
            _repository = productRepo;
            _excelBase = excelBase;
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
            hangHoa.SoPhutThucHien = dto.SoPhutThucHien;
            hangHoa.Image = dto.Image;
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
        public async Task DeleteMultipleProduct(List<Guid> lstIdHangHoa)
        {
            var arrIdQuyDoi = _dmDonViQuiDoi.GetAllList(x => lstIdHangHoa.Contains(x.IdHangHoa)).Select(x => x.Id).ToList();
            _dmHangHoa.GetAllList(x => lstIdHangHoa.Contains(x.Id)).ForEach(x =>
            {
                x.TrangThai = 0;
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
            });
            _dmDonViQuiDoi.GetAllList(x => arrIdQuyDoi.Contains(x.Id)).ForEach(x =>
            {
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
            });
            // todo remove image in google api
        }
        [HttpPost]
        public async Task ChuyenNhomHang(List<Guid> lstIdHangHoa, Guid idNhomHang)
        {
            _dmHangHoa.GetAllList(x => lstIdHangHoa.Contains(x.Id)).ForEach(x =>
            {
                x.IdNhomHangHoa = idNhomHang;
                x.LastModifierUserId = AbpSession.UserId;
                x.LastModificationTime = DateTime.Now;
            });
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
            var data = await _repository.GetDMHangHoa(input, AbpSession.TenantId ?? 1);
            var dataExcel = ObjectMapper.Map<List<ExportExcelHangHoaDto>>(data.Items);
            return _excelBase.WriteToExcel("DanhSachDichVu_", "DichVu_Export_Template.xlsx", dataExcel, 4);
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
                                    MaxCodeDto objMax = await _repository.SpGetProductCode(data.IdLoaiHangHoa, AbpSession.TenantId ?? 1);
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
                result.Message = "Có lỗi xảy ra trong quá trình import dữ liệu";
                result.Status = "error";
                result.Detail = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task<List<ExcelErrorDto>> ImportFile_DanhMucHangHoa(FileUpload file)
        {
            List<ExcelErrorDto> lstErr = new();
            try
            {
                if (file.Type != ".xlsx")
                {
                    lstErr.Add(new ExcelErrorDto
                    {
                        RowNumber = 0,
                        GiaTriDuLieu = "Định dạng file",
                        DienGiai = "File không đúng định dạng",
                        LoaiErr = 0,
                    });
                    return lstErr;
                }

                using MemoryStream stream = new MemoryStream(file.File);
                using var package = new ExcelPackage();
                package.Load(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                #region Check dữ liệu file
                // cột B: mã dịch vụ (cột thứ 2), đọc bắt đàu từ dòng số 3 --> rowCount
                var errDuplicate = Excel_CheckDuplicateData(worksheet, "B", 2, 3, rowCount);
                if (errDuplicate.Count > 0)
                {
                    foreach (var item in errDuplicate)
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = item.RowNumber,
                            TenTruongDuLieu = "Mã dịch vụ",
                            GiaTriDuLieu = item.GiaTriDuLieu,
                            DienGiai = "Mã dịch vụ bị trùng lặp",
                            LoaiErr = 1,
                        });
                    }
                }
                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string tenNhomHangHoa = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string maHangHoa = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    string tenHangHoa = worksheet.Cells[i, 3].Value?.ToString().Trim();
                    //string loaiHang = worksheet.Cells[i, 4].Value?.ToString().Trim(); (mặc định all dịch vụ)
                    string giaban = worksheet.Cells[i, 4].Value?.ToString();
                    string dataType_giaBan = worksheet.Cells[i, 4].Value?.GetType().ToString();
                    string sophutThucHien = worksheet.Cells[i, 5].Value?.ToString();
                    string dataType_sophut = worksheet.Cells[i, 5].Value?.GetType().ToString();

                    // nếu dòng trống: bỏ qua và nhảy sang dòng tiếp theo
                    if (!string.IsNullOrEmpty(tenNhomHangHoa)
                            || !string.IsNullOrEmpty(maHangHoa)
                            || !string.IsNullOrEmpty(tenHangHoa)
                           //|| !string.IsNullOrEmpty(loaiHang)
                           || !string.IsNullOrEmpty(giaban)
                           || !string.IsNullOrEmpty(sophutThucHien)
                           )
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }

                    // nếu tồn tại mã: cập nhật lại all thông tin theo file excel
                    //if (!string.IsNullOrEmpty(maHangHoa))
                    //{
                    //    maHangHoa = maHangHoa.ToUpper();
                    //    var exist = await CheckExistsMaHangHoa(maHangHoa);
                    //    if (exist)
                    //    {
                    //        lstErr.Add(new ExcelErrorDto
                    //        {
                    //            RowNumber = i,
                    //            TenTruongDuLieu = "Mã dịch vụ",
                    //            GiaTriDuLieu = maHangHoa,
                    //            DienGiai = "Mã dịch vụ đã tồn tại",
                    //            LoaiErr = 2,
                    //        });
                    //    }
                    //}
                    if (string.IsNullOrEmpty(tenHangHoa))
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Tên dịch vụ",
                            GiaTriDuLieu = tenHangHoa,
                            DienGiai = "Tên dịch vụ không được để trống",
                            LoaiErr = 1,
                        });
                    }

                    #region loaihang: nếu sau này cần dùng thì bỏ comment
                    //if (string.IsNullOrEmpty(loaiHang))
                    //{
                    //    lstErr.Add(new ExcelErrorDto
                    //    {
                    //        RowNumber = i,
                    //        TenTruongDuLieu = "Loại hàng hóa",
                    //        GiaTriDuLieu = loaiHang,
                    //        DienGiai = "Loại hàng hóa không được để trống",
                    //        LoaiErr = 1,
                    //    });
                    //}
                    //else
                    //{
                    //    string[] arrLoai = { "HH", "DV", "CB" };
                    //    if (!arrLoai.Contains(loaiHang))
                    //    {
                    //        lstErr.Add(new ExcelErrorDto
                    //        {
                    //            RowNumber = i,
                    //            TenTruongDuLieu = "Loại hàng hóa",
                    //            GiaTriDuLieu = loaiHang,
                    //            DienGiai = "Loại hàng không đúng định dạng ",
                    //            LoaiErr = 1,
                    //        });
                    //    }
                    //}
                    #endregion

                    if (!string.IsNullOrEmpty(giaban))
                    {
                        bool isNumber = Excel_CheckNumber(dataType_giaBan);
                        if (!isNumber)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Giá bán",
                                GiaTriDuLieu = giaban,
                                DienGiai = "Giá bán không phải dạng số",
                                LoaiErr = 1,
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(sophutThucHien))
                    {
                        bool isNumber = Excel_CheckNumber(dataType_sophut);
                        if (!isNumber)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Số phút thực hiện",
                                GiaTriDuLieu = sophutThucHien,
                                DienGiai = "Số phút thực hiện không phải dạng số",
                                LoaiErr = 1,
                            });
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                lstErr.Add(new ExcelErrorDto
                {
                    RowNumber = -1,
                    TenTruongDuLieu = "Exception",
                    GiaTriDuLieu = "",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            if (lstErr.Count == 0)
            {
                // thực hiện import
                lstErr = await Execute_ImportDanhMucHangHoa(file);// phải load lại file, vì Excel bị dispose
            }
            return lstErr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<ExcelErrorDto>> Execute_ImportDanhMucHangHoa(FileUpload file)
        {
            List<ExcelErrorDto> lstErr = new();
            try
            {
                using MemoryStream stream = new MemoryStream(file.File);
                using var package = new ExcelPackage();
                package.Load(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string tenNhomHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string maHangHoa = worksheet.Cells[i, 2].Value?.ToString().Trim().ToUpper();
                    string tenHangHoa = worksheet.Cells[i, 3].Value?.ToString().Trim();
                    //string loaiHang = worksheet.Cells[i, 4].Value?.ToString().Trim();
                    string giaban = worksheet.Cells[i, 4].Value?.ToString();
                    double giaBanNew = !string.IsNullOrEmpty(giaban) ? double.Parse(giaban) : 0;
                    string sophutThucHien = worksheet.Cells[i, 5].Value?.ToString();
                    float sophutThucHienNew = !string.IsNullOrEmpty(sophutThucHien) ? float.Parse(sophutThucHien) : 0;

                    if (!string.IsNullOrEmpty(maHangHoa)
                        || !string.IsNullOrEmpty(maHangHoa)
                        || !string.IsNullOrEmpty(tenHangHoa)
                        //|| !string.IsNullOrEmpty(loaiHang)
                        || !string.IsNullOrEmpty(giaban)
                        || !string.IsNullOrEmpty(sophutThucHien)
                           )
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }
                    ImportExcelHangHoaDto newObj = new()
                    {
                        TenNhomHangHoa = tenNhomHang,
                        MaHangHoa = maHangHoa,
                        TenHangHoa = tenHangHoa,
                        IdLoaiHangHoa = 2,
                        //IdLoaiHangHoa = loaiHang == "HH" ? 1 : loaiHang == "DV" ? 2 : 3,
                        GiaBan = giaBanNew,
                        SoPhutThucHien = sophutThucHienNew,
                        GhiChu = worksheet.Cells[i, 6].Value?.ToString().Trim(),
                    };
                    try
                    {
                        await _repository.ImportDanhMucHangHoa(AbpSession.TenantId ?? 1, AbpSession.UserId, newObj);
                    }
                    catch (Exception ex)
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Import",
                            GiaTriDuLieu = "Import",
                            DienGiai = ex.Message.ToString(),
                            LoaiErr = 2,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                lstErr.Add(new ExcelErrorDto
                {
                    RowNumber = -1,
                    GiaTriDuLieu = "Exception",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            return lstErr;
        }
    }
}
