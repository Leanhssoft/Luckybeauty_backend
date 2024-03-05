using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.NewFolder;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using BanHangBeautify.NhanSu.CaLamViec.Exporting;
using BanHangBeautify.NhanSu.CaLamViec.Repository;
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
using static NPOI.HSSF.UserModel.HeaderFooter;

namespace BanHangBeautify.NhanSu.CaLamViec
{
    [AbpAuthorize(PermissionNames.Pages_CaLamViec)]
    public class CaLamViecAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_CaLamViec, Guid> _repository;
        private readonly ICaLamViecRepository _caLamViecRepository;
        private readonly ICaLamViecExcelExporter _caLamViecExcelExporter;
        private readonly INhatKyThaoTacAppService _audiLogService;
        public CaLamViecAppService(IRepository<NS_CaLamViec, Guid> repository,
            ICaLamViecRepository caLamViecRepository, 
            ICaLamViecExcelExporter caLamViecExcelExporter,
            INhatKyThaoTacAppService audiLogService)
        {
            _repository = repository;
            _caLamViecRepository = caLamViecRepository;
            _caLamViecExcelExporter = caLamViecExcelExporter;
            _audiLogService = audiLogService;
        }
        [AbpAuthorize(PermissionNames.Pages_CaLamViec_Create, PermissionNames.Pages_CaLamViec_Edit)]
        public async Task<CaLamViecDto> CreateOrEdit(CreateOrEditCaLamViecDto dto)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (caLamViec == null)
            {
                return await Create(dto);
            }
            return await Edit(dto, caLamViec);
        }
        [NonAction]
        public async Task<CaLamViecDto> Create(CreateOrEditCaLamViecDto dto)
        {
            NS_CaLamViec data = new NS_CaLamViec();
            
            data.Id = Guid.NewGuid();
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var count = _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).Count() + 1;
                if (count.ToString().Length>=3)
                {
                    data.MaCa = "MS" + count.ToString();
                }
                else if (count.ToString().Length==2)
                {
                    data.MaCa = "MS0" + count.ToString();
                }
                else
                {
                    data.MaCa = "MS00" + count.ToString();
                }
                
            }
            data.IdChiNhanh = dto.IdChiNhanh;
            data.TenCa = dto.TenCa;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.LaNghiGiuaCa = data.LaNghiGiuaCa== true? true: false;
            if (dto.LaNghiGiuaCa==true&& !string.IsNullOrEmpty(dto.GioNghiTu)&& !string.IsNullOrEmpty(dto.GioNghiDen))
            {
                data.GioVao = DateTime.Parse(dto.GioVao.ToString());
                data.GioRa = DateTime.Parse(dto.GioRa.ToString());
                data.GioNghiTu = DateTime.Parse(dto.GioNghiTu.ToString());
                data.GioNghiDen = DateTime.Parse(dto.GioNghiDen.ToString());
                var thoiGianNghi = (float)(data.GioNghiDen.Value.Subtract(data.GioNghiTu.Value).TotalMinutes /60);
                data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60) - thoiGianNghi;
            }
            else
            {
                data.GioVao = DateTime.Parse(dto.GioVao.ToString());
                data.GioRa = DateTime.Parse(dto.GioRa.ToString());
                data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60);
            }
            
            data.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.InsertAsync(data);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Ca làm việc";
            nhatKyThaoTacDto.NoiDung = "Thêm mới ca làm việc: " + data.TenCa + "(" + data.MaCa + ")";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<CaLamViecDto> Edit(CreateOrEditCaLamViecDto dto, NS_CaLamViec data)
        {
            data.MaCa = dto.MaCa;
            data.TenCa = dto.TenCa;
            data.IdChiNhanh = dto.IdChiNhanh;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.LaNghiGiuaCa = dto.LaNghiGiuaCa;
            if (dto.LaNghiGiuaCa == true && !string.IsNullOrEmpty(dto.GioNghiTu) && !string.IsNullOrEmpty(dto.GioNghiDen))
            {
                data.GioVao = DateTime.Parse(dto.GioVao.ToString());
                data.GioRa = DateTime.Parse(dto.GioRa.ToString());
                data.GioNghiTu = DateTime.Parse(dto.GioNghiTu.ToString());
                data.GioNghiDen = DateTime.Parse(dto.GioNghiDen.ToString());
                var thoiGianNghi =(float)(data.GioNghiDen.Value.Subtract(data.GioNghiTu.Value).TotalMinutes/60);
                data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60) - thoiGianNghi;
            }
            else
            {
                data.GioVao = DateTime.Parse(dto.GioVao.ToString());
                data.GioRa = DateTime.Parse(dto.GioRa.ToString());
                data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60);
            }
            
            data.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.UpdateAsync(data);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Ca làm việc";
            nhatKyThaoTacDto.NoiDung = "Sửa thông tin ca làm việc: " + data.TenCa + "(" + data.MaCa + ")";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_CaLamViec_Delete)]
        public async Task<CaLamViecDto> Delete(Guid Id)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == Id);
            if (caLamViec != null)
            {
                caLamViec.TrangThai = 1;
                caLamViec.IsDeleted = true;
                caLamViec.DeletionTime = DateTime.Now;
                caLamViec.DeleterUserId = AbpSession.UserId;
                await _repository.UpdateAsync(caLamViec);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Ca làm việc";
                nhatKyThaoTacDto.NoiDung = "Xóa ca làm việc: " + caLamViec.TenCa + "(" + caLamViec.MaCa + ")";
                await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return ObjectMapper.Map<CaLamViecDto>(caLamViec);
            }
            return new CaLamViecDto();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_CaLamViec_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            {
                var finds = await _repository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (finds != null && finds.Count > 0)
                {
                    _repository.RemoveRange(finds);
                    result.Status = "success";
                    result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                    var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                    nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                    nhatKyThaoTacDto.ChucNang = "Ca làm việc";
                    nhatKyThaoTacDto.NoiDung = "Xóa nhiều ca làm việc: " + string.Join(",", finds.SelectMany(x => x.TenCa).ToList());
                    await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);

                }
                return result;
            }
        }
        public async Task<CreateOrEditCaLamViecDto> GetForEdit(Guid id)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (caLamViec != null)
            {
                var data = ObjectMapper.Map<CreateOrEditCaLamViecDto>(caLamViec);
                data.GioVao = caLamViec.GioVao.ToString("HH:mm");
                data.GioRa = caLamViec.GioRa.ToString("HH:mm");
                data.GioNghiTu = caLamViec.GioNghiTu.HasValue? caLamViec.GioNghiTu.Value.ToString("HH:mm") : null;
                data.GioNghiDen = caLamViec.GioNghiDen.HasValue? caLamViec.GioNghiDen.Value.ToString("HH:mm"): null;
                data.LaNghiGiuaCa = caLamViec.LaNghiGiuaCa;
                return data;
            }
            return new CreateOrEditCaLamViecDto();
        }
        public async Task<PagedResultDto<CaLamViecDto>> GetAll(PagedRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Keyword))
            {
                input.Keyword = "";

            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            return await _caLamViecRepository.GetAll(input, AbpSession.TenantId ?? 1);
        }
        public async Task<FileDto> ExportToExcel(PagedRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Keyword))
            {
                input.Keyword = "";

            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _caLamViecRepository.GetAll(input, AbpSession.TenantId ?? 1);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Export;
            nhatKyThaoTacDto.ChucNang = "Ca làm việc";
            nhatKyThaoTacDto.NoiDung = "Xuất danh sách ca làm việc ra file Excel";
            nhatKyThaoTacDto.NoiDungChiTiet = "Xuất danh sách ca làm việc ra file Excel";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return _caLamViecExcelExporter.ExportDanhSachCaLamViec(data.Items.ToList());
        }
        public async Task<FileDto> ExportSelectedCaLamViec(List<Guid> IdCaLamViecs)
        {
            PagedRequestDto input = new PagedRequestDto();
            input.Keyword = "";
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await _caLamViecRepository.GetAll(input, AbpSession.TenantId ?? 1);
            return _caLamViecExcelExporter.ExportDanhSachCaLamViec(data.Items.Where(x=>IdCaLamViecs.Contains(x.Id)).ToList());
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
                                CreateOrEditCaLamViecDto data = new CreateOrEditCaLamViecDto();
                                data.Id = Guid.NewGuid();
                                data.TenCa = worksheet.Cells[row, 2].Value?.ToString();
                                data.GioVao = worksheet.Cells[row, 3].Value?.ToString();
                                data.GioVao = worksheet.Cells[row, 4].Value?.ToString();
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
                    nhatKyThaoTacDto.ChucNang = "Ca làm việc";
                    nhatKyThaoTacDto.NoiDung = "Nhập danh sách ca làm việc từ file Excel";
                    nhatKyThaoTacDto.NoiDungChiTiet = "Nhập danh sách ca làm việc từ file Excel";
                    await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
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
