using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NewFolder;
using BanHangBeautify.NhanSu.NhanVien.Dto;
using BanHangBeautify.NhanSu.NhanVien.Exporting;
using BanHangBeautify.NhanSu.NhanVien.Responsitory;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace BanHangBeautify.NhanSu.NhanVien
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu)]
    public class NhanSuAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_NhanVien, Guid> _repository;
        private readonly IRepository<NS_ChucVu, Guid> _chucVuRepository;
        private readonly IHostingEnvironment _env;
        private readonly IRepository<NS_QuaTrinh_CongTac, Guid> _quaTrinhCongTac;
        private readonly INhanSuRepository _nhanSuRepository;
        private readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhService;
        private readonly INhanVienExcelExporter _nhanVienExcelExporter;
        public NhanSuAppService(IRepository<NS_NhanVien, Guid> repository,
            IRepository<NS_ChucVu, Guid> chucVuRepository,
            IRepository<NS_QuaTrinh_CongTac, Guid> quaTrinhCongTac,
            INhanSuRepository nhanSuRepository, IHostingEnvironment env,
            IRepository<DM_ChiNhanh, Guid> chiNhanhService,
            INhanVienExcelExporter nhanVienExcelExporter
         )
        {
            _repository = repository;
            _chucVuRepository = chucVuRepository;
            _quaTrinhCongTac = quaTrinhCongTac;
            _nhanSuRepository = nhanSuRepository;
            _env = env;
            _chiNhanhService = chiNhanhService;
            _nhanVienExcelExporter = nhanVienExcelExporter;
        }
        [AbpAuthorize(PermissionNames.Pages_NhanSu_Create, PermissionNames.Pages_NhanSu_Edit)]
        public async Task<NhanSuItemDto> CreateOrEdit(CreateOrEditNhanSuDto dto)
        {
            try
            {
                var find = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (find == null)
                {
                    return await Create(dto);
                }
                else
                {
                    return await Edit(dto, find);
                }

            }
            catch (Exception)
            {
                return new NhanSuItemDto();
            }
        }
        [NonAction]
        public async Task<NhanSuItemDto> Create(CreateOrEditNhanSuDto dto)
        {
            NS_NhanVien nhanSu = new NS_NhanVien();
            nhanSu.Id = Guid.NewGuid();
            nhanSu.IdChucVu = dto.IdChucVu;
            //nhanSu.IdPhongBan = dto.IdPhongBan;
            var curentTenant = await GetCurrentTenantAsync();
            string tenantName = "";
            if (curentTenant == null)
            {
                tenantName = "AdminTenant";
            }
            else
            {
                tenantName = curentTenant.TenancyName;
            }
            var countNhanVien = _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).ToList().Count();
            nhanSu.MaNhanVien = "NS00" + countNhanVien + 1;
            nhanSu.Ho = dto.Ho;
            nhanSu.TenLot = dto.TenLot;
            nhanSu.TenNhanVien = dto.TenNhanVien;
            if (dto.AvatarFile != null && !string.IsNullOrWhiteSpace(dto.AvatarFile.FileBase64))
            {
                nhanSu.Avatar = SaveFile(dto.AvatarFile, tenantName + "/NhanSu/" + nhanSu.TenNhanVien);
            }
            nhanSu.CCCD = dto.CCCD;
            nhanSu.GioiTinh = dto.GioiTinh;
            nhanSu.DiaChi = dto.DiaChi;
            nhanSu.SoDienThoai = dto.SoDienThoai;
            nhanSu.NgaySinh = dto.NgaySinh;
            nhanSu.NgayCap = dto.NgayCap;
            nhanSu.NoiCap = dto.NoiCap;
            nhanSu.KieuNgaySinh = dto.KieuNgaySinh;
            nhanSu.Avatar = dto.Avatar;
            nhanSu.TenantId = AbpSession.TenantId ?? 1;
            nhanSu.CreationTime = DateTime.Now;
            nhanSu.CreatorUserId = AbpSession.UserId;
            nhanSu.LastModificationTime = DateTime.Now;
            nhanSu.LastModifierUserId = AbpSession.UserId;
            nhanSu.IsDeleted = false;
            var result = ObjectMapper.Map<NhanSuItemDto>(nhanSu);
            result.NgayVaoLam = nhanSu.CreationTime;
            result.TenChucVu = _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty) != null ? _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty).TenChucVu : string.Empty;
            await _repository.InsertAsync(nhanSu);
            var qtct = CreateFirstQuaTrinhCongTac(nhanSu.Id, dto.IdChiNhanh);
            await _quaTrinhCongTac.InsertAsync(qtct);
            return result;
        }
        [NonAction]
        public NS_QuaTrinh_CongTac CreateFirstQuaTrinhCongTac(Guid idNhanVien, Guid? idChiNhanh)
        {
            NS_QuaTrinh_CongTac qtct = new NS_QuaTrinh_CongTac();
            qtct.Id = Guid.NewGuid();
            qtct.IdNhanVien = idNhanVien;
            qtct.IdChiNhanh = idChiNhanh;
            qtct.TuNgay = DateTime.Now;
            qtct.TrangThai = 0;
            qtct.TenantId = AbpSession.TenantId ?? 1;
            qtct.CreationTime = DateTime.Now;
            qtct.CreatorUserId = AbpSession.UserId;
            qtct.IsDeleted = false;
            return qtct;
        }
        [NonAction]
        public async Task<NhanSuItemDto> Edit(CreateOrEditNhanSuDto dto, NS_NhanVien nhanSu)
        {
            nhanSu.IdChucVu = dto.IdChucVu;
            //nhanSu.IdPhongBan = dto.IdPhongBan;
            nhanSu.MaNhanVien = dto.MaNhanVien;
            nhanSu.Ho = dto.Ho;
            nhanSu.TenLot = dto.TenLot;
            nhanSu.TenNhanVien = dto.Ho + " " + dto.TenLot;
            nhanSu.CCCD = dto.CCCD;
            nhanSu.GioiTinh = dto.GioiTinh;
            nhanSu.DiaChi = dto.DiaChi;
            nhanSu.SoDienThoai = dto.SoDienThoai;
            nhanSu.NgaySinh = dto.NgaySinh;
            nhanSu.NgayCap = dto.NgayCap;
            nhanSu.NoiCap = dto.NoiCap;
            nhanSu.KieuNgaySinh = dto.KieuNgaySinh;
            nhanSu.Avatar = dto.Avatar;
            nhanSu.LastModificationTime = DateTime.Now;
            nhanSu.LastModifierUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<NhanSuItemDto>(nhanSu);
            result.NgayVaoLam = nhanSu.CreationTime;
            result.TenChucVu = _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty) != null ? _chucVuRepository.FirstOrDefault(nhanSu.IdChucVu ?? Guid.Empty).TenChucVu : string.Empty;
            await _repository.UpdateAsync(nhanSu);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanSu_Delete)]
        public async Task<NhanSuItemDto> Delete(Guid id)
        {
            var find = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (find != null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                _repository.Update(find);
                return ObjectMapper.Map<NhanSuItemDto>(find);
            }
            return new NhanSuItemDto();
        }
        public async Task<NS_NhanVien> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        [HttpPost]
        public async Task<CreateOrEditNhanSuDto> GetNhanSu(Guid id)
        {
            var nhanSu = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (nhanSu != null)
            {
                var result = ObjectMapper.Map<CreateOrEditNhanSuDto>(nhanSu);
                return result;
            }

            return new CreateOrEditNhanSuDto();
        }
        public async Task<PagedResultDto<NhanSuItemDto>> GetAll(PagedNhanSuRequestDto input)
        {
            input.Filter = (input.Filter ?? string.Empty).Trim();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.MaxResultCount = input.MaxResultCount;
            input.TenantId = input.TenantId != null ? input.TenantId : (AbpSession.TenantId ?? 1);
            return await _nhanSuRepository.GetAllNhanSu(input);
        }
        public async Task<FileDto> ExportDanhSach(PagedNhanSuRequestDto input)
        {
            input.Filter = (input.Filter ?? string.Empty).Trim();
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await GetAll(input);
            List<NhanSuItemDto> model = new List<NhanSuItemDto>();
            model = (List<NhanSuItemDto>)data.Items;
            return _nhanVienExcelExporter.ExportDanhSachNhanVien(model);
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
                                CreateOrEditNhanSuDto data = new CreateOrEditNhanSuDto();
                                data.Id = Guid.NewGuid();
                                data.TenNhanVien = worksheet.Cells[row, 2].Value?.ToString();
                                data.SoDienThoai = worksheet.Cells[row, 3].Value?.ToString();
                                var checkPhoneNumber = await _repository.FirstOrDefaultAsync(x => x.SoDienThoai == data.SoDienThoai);
                                await UnitOfWorkManager.Current.SaveChangesAsync();
                                string tenChiNhanh = worksheet.Cells[row, 10].Value?.ToString();
                                var checkChiNhanh = await _chiNhanhService.FirstOrDefaultAsync(x => x.TenChiNhanh.Trim() == tenChiNhanh.Trim());
                                await UnitOfWorkManager.Current.SaveChangesAsync();
                                if (checkPhoneNumber != null || data.TenNhanVien == null || data.SoDienThoai == null || checkChiNhanh == null)
                                {
                                    countImportLoi++;
                                    continue;
                                }
                                data.IdChiNhanh = checkChiNhanh.Id;
                                if (!string.IsNullOrEmpty(worksheet.Cells[row, 4].Value?.ToString()))
                                {
                                    data.NgaySinh = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString());
                                }
                                if (worksheet.Cells[row, 5].Value?.ToString().ToLower() == "nam")
                                {
                                    data.GioiTinh = 1;
                                }
                                else
                                {
                                    data.GioiTinh = 2;
                                }
                                data.DiaChi = worksheet.Cells[row, 6].Value?.ToString();
                                data.CCCD = worksheet.Cells[row, 7].Value?.ToString();
                                data.NgayCap = worksheet.Cells[row, 8].Value?.ToString();
                                data.NoiCap = worksheet.Cells[row, 9].Value?.ToString();
                                data.KieuNgaySinh = 1;
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
        private string SaveFile(AvatarFile file, string thumuc = "")
        {
            String path = _env.ContentRootPath + "/Common/" + thumuc; //Path

            //Check if directory exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var filename = Path.GetFileNameWithoutExtension(file.FileName).Trim();
            var newFileName = $"{filename}_{timeStamp}{Path.GetExtension(file.FileName)}";
            //set the image path
            string filePath = Path.Combine(path, newFileName);

            byte[] fileBytes = Convert.FromBase64String(file.FileBase64);

            File.WriteAllBytes(filePath, fileBytes);

            return $"/Common/{thumuc}/{newFileName}";
        }
    }
}
