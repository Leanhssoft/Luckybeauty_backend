using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NewFolder;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using BanHangBeautify.NhanSu.CaLamViec.Exporting;
using BanHangBeautify.NhanSu.CaLamViec.Repository;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.Storage;
using Castle.MicroKernel.SubSystems.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace BanHangBeautify.NhanSu.CaLamViec
{
    [AbpAuthorize(PermissionNames.Pages_CaLamViec)]
    public class CaLamViecAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_CaLamViec, Guid> _repository;
        private readonly ICaLamViecRepository _caLamViecRepository;
        private readonly ICaLamViecExcelExporter _caLamViecExcelExporter; 
        public CaLamViecAppService(IRepository<NS_CaLamViec, Guid> repository,ICaLamViecRepository caLamViecRepository,ICaLamViecExcelExporter caLamViecExcelExporter)
        {
            _repository = repository;
            _caLamViecRepository = caLamViecRepository;
            _caLamViecExcelExporter = caLamViecExcelExporter;
        }
        [AbpAuthorize(PermissionNames.Pages_CaLamViec_Create,PermissionNames.Pages_CaLamViec_Edit)]
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
            var count =await  _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId??1)).ToListAsync();
            data.Id = Guid.NewGuid();
            data.MaCa = "MS00"+ (count.Count+1).ToString();
            data.TenCa = dto.TenCa;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.GioVao = DateTime.Parse(dto.GioVao.ToString());
            data.GioRa = DateTime.Parse(dto.GioRa.ToString());
            data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60);
            data.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<CaLamViecDto> Edit(CreateOrEditCaLamViecDto dto, NS_CaLamViec data)
        {
            data.MaCa = dto.MaCa;
            data.TenCa = dto.TenCa;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.GioVao = DateTime.Parse(dto.GioVao.ToString());
            data.GioRa = DateTime.Parse(dto.GioRa.ToString());
            data.TongGioCong = (float)(data.GioRa.Subtract(data.GioVao).TotalMinutes / 60);
            data.LastModificationTime = DateTime.Now;
            var result = ObjectMapper.Map<CaLamViecDto>(data);
            await _repository.UpdateAsync(data);
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
                return ObjectMapper.Map<CaLamViecDto>(caLamViec);
            }
            return new CaLamViecDto();
        }
        public async Task<CreateOrEditCaLamViecDto> GetForEdit(Guid id)
        {
            var caLamViec = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (caLamViec != null)
            { var data = ObjectMapper.Map<CreateOrEditCaLamViecDto>(caLamViec);
                data.GioVao = caLamViec.GioVao.ToString("HH:mm");
                data.GioRa = caLamViec.GioRa.ToString("HH:mm");
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
            return _caLamViecExcelExporter.ExportDanhSachCaLamViec(data.Items.ToList());
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
