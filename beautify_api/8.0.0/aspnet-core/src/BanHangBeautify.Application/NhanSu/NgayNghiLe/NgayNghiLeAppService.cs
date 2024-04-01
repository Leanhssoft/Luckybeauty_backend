using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.NewFolder;
using BanHangBeautify.NhanSu.NgayNghiLe.Dto;
using BanHangBeautify.NhanSu.NgayNghiLe.Exporting;
using BanHangBeautify.NhanSu.NgayNghiLe.Repository;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace BanHangBeautify.NhanSu.NgayNghiLe
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu_NgayNghiLe)]
    public class NgayNghiLeAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_NgayNghiLe, Guid> _ngayNghiLeService;
        private readonly INgayNghiLeRepository _ngayNghiLeReponsitory;
        private readonly INgayNghiLeExcelExporter _ngayNghiLeExcelExporter;
        private readonly INhatKyThaoTacAppService _audilogService;
        public NgayNghiLeAppService(IRepository<DM_NgayNghiLe, Guid> ngayNghiLeService,
            INgayNghiLeRepository ngayNghiLeReponsitory,
            INgayNghiLeExcelExporter ngayNghiLeExcelExporter,
            INhatKyThaoTacAppService audilogService)
        {
            _ngayNghiLeService = ngayNghiLeService;
            _ngayNghiLeReponsitory = ngayNghiLeReponsitory;
            _ngayNghiLeExcelExporter = ngayNghiLeExcelExporter;
            _audilogService = audilogService;
        }
        public async Task<PagedResultDto<NgayNghiLeDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount == 0 || input.SkipCount == 1 ? 0 : ((input.SkipCount - 1) * input.MaxResultCount);
            input.Keyword = !string.IsNullOrEmpty(input.Keyword) ? input.Keyword : "";

            return await _ngayNghiLeReponsitory.GetAll(input, AbpSession.TenantId ?? 1);
        }
        [HttpPost]
        public async Task<CreateOrEditNgayNghiLeDto> GetForEdit(Guid id)
        {
            CreateOrEditNgayNghiLeDto result = new CreateOrEditNgayNghiLeDto();
            var data = await _ngayNghiLeService.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                result = ObjectMapper.Map<CreateOrEditNgayNghiLeDto>(data);
            }
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanSu_NgayNghiLe_Create, PermissionNames.Pages_NhanSu_NgayNghiLe_Edit)]
        public async Task<NgayNghiLeDto> CreateOrEdit(CreateOrEditNgayNghiLeDto input)
        {
            NgayNghiLeDto result = new NgayNghiLeDto();
            var checkExists = await _ngayNghiLeService.FirstOrDefaultAsync(x => x.Id == input.Id);
            result = checkExists == null ? await Create(input) : await Edit(input, checkExists);
            return result;
        }
        [NonAction]
        public async Task<NgayNghiLeDto> Create(CreateOrEditNgayNghiLeDto input)
        {
            DM_NgayNghiLe dto = new DM_NgayNghiLe();
            dto.Id = Guid.NewGuid();
            dto.TenNgayLe = input.TenNgayLe;
            dto.TuNgay = input.TuNgay;
            dto.DenNgay = input.DenNgay.Date.AddDays(1).AddSeconds(-1); // Set time to 23:59:59 on the same day
            var all = dto.DenNgay.Subtract(dto.TuNgay);
            var day = Math.Round(all.TotalDays);
            dto.TongSoNgay = (int)day; // Directly cast the rounded value to an int
            dto.CreationTime = DateTime.Now;
            dto.CreatorUserId = AbpSession.UserId;
            dto.TenantId = AbpSession.TenantId ?? 1;
            dto.IsDeleted = false;
            dto.TrangThai = 0;
            _ngayNghiLeService.Insert(dto);
            var result = ObjectMapper.Map<NgayNghiLeDto>(dto);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Ca làm việc";
            nhatKyThaoTacDto.NoiDung = "Thêm mới ngày lễ: " + input.TenNgayLe;
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới ngày lễ: " + input.TenNgayLe;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<NgayNghiLeDto> Edit(CreateOrEditNgayNghiLeDto input, DM_NgayNghiLe oldData)
        {
            oldData.TenNgayLe = input.TenNgayLe;
            oldData.TuNgay = input.TuNgay;
            oldData.DenNgay = input.DenNgay.Date.AddDays(1).AddSeconds(-1); // Set time to 23:59:59 on the same day
            var all = oldData.DenNgay.Subtract(oldData.TuNgay);
            var day = Math.Round(all.TotalDays);
            oldData.TongSoNgay = (int)day; // Directly cast the rounded value to an int
            oldData.LastModifierUserId = AbpSession.UserId;
            oldData.LastModificationTime = DateTime.Now;
            _ngayNghiLeService.Update(oldData);
            var result = ObjectMapper.Map<NgayNghiLeDto>(oldData);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Ngày lễ";
            nhatKyThaoTacDto.NoiDung = "Sửa thông tin ngày lễ: " + input.TenNgayLe;
            nhatKyThaoTacDto.NoiDungChiTiet = "Sửa thông tin ngày lễ: " + input.TenNgayLe;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanSu_NgayNghiLe_Delete)]
        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var checkExists = await _ngayNghiLeService.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExists != null)
            {
                checkExists.IsDeleted = true;
                checkExists.DeletionTime = DateTime.Now;
                checkExists.DeleterUserId = AbpSession.UserId;
                _ngayNghiLeService.Update(checkExists);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
                nhatKyThaoTacDto.ChucNang = "Ngày lễ";
                nhatKyThaoTacDto.NoiDung = "Xóa ngày lễ: " + checkExists.TenNgayLe;
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa ngày lễ: " + checkExists.TenNgayLe;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                result = true;
            }
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanSu_NgayNghiLe_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            var checkExists = await _ngayNghiLeService.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
            if (checkExists != null && checkExists.Count > 0)
            {
                _ngayNghiLeService.RemoveRange(checkExists);
                result.Status = "success";
                result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
                nhatKyThaoTacDto.ChucNang = "Ngày lễ";
                nhatKyThaoTacDto.NoiDung = "Xóa các ngày lễ: " + string.Join(", ", checkExists.Select(x => x.TenNgayLe).ToList());
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa các ngày lễ: " + string.Join(", ", checkExists.Select(x => x.TenNgayLe).ToList());
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            }
            return result;
        }
        public async Task<FileDto> ExportToExcel(PagedRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Keyword))
            {
                input.Keyword = "";

            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _ngayNghiLeReponsitory.GetAll(input, AbpSession.TenantId ?? 1);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Export;
            nhatKyThaoTacDto.ChucNang = "Ngày lễ";
            nhatKyThaoTacDto.NoiDung = "Xuất danh sách ngày lễ";
            nhatKyThaoTacDto.NoiDungChiTiet = "Xuất danh sách ngày lễ";
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return _ngayNghiLeExcelExporter.ExportDanhSachNgayNghiLe(data.Items.ToList());
        }
        public async Task<FileDto> ExportSelectedNghiLe(List<Guid> IdLichNghis)
        {
            PagedRequestDto input = new PagedRequestDto();
            input.Keyword = "";
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _ngayNghiLeReponsitory.GetAll(input, AbpSession.TenantId ?? 1);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Export;
            nhatKyThaoTacDto.ChucNang = "Ngày lễ";
            nhatKyThaoTacDto.NoiDung = "Xuất danh sách ngày lễ";
            nhatKyThaoTacDto.NoiDungChiTiet = "Xuất danh sách ngày lễ";
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return _ngayNghiLeExcelExporter.ExportDanhSachNgayNghiLe(data.Items.Where(x => IdLichNghis.Contains(x.Id)).ToList());
        }
        [HttpPost]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
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
                                CreateOrEditNgayNghiLeDto data = new CreateOrEditNgayNghiLeDto();
                                data.Id = Guid.NewGuid();
                                data.TenNgayLe = worksheet.Cells[row, 2].Value?.ToString();
                                data.TuNgay = DateTime.Parse(worksheet.Cells[row, 3].Value?.ToString());
                                data.DenNgay = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString());
                                await Create(data);
                                countImportData++;
                            }
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
                    var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                    nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Import;
                    nhatKyThaoTacDto.ChucNang = "Ngày lễ";
                    nhatKyThaoTacDto.NoiDung = "Nhập danh sách ngày lễ";
                    nhatKyThaoTacDto.NoiDungChiTiet = "Nhập danh sách ngày lễ";
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
    }
}