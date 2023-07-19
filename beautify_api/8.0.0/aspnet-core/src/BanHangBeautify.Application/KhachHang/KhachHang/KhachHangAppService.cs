﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Exporting;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using BanHangBeautify.NewFolder;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BanHangBeautify.KhachHang.KhachHang
{
    [AbpAuthorize(PermissionNames.Pages_KhachHang)]
    public class KhachHangAppService : SPAAppServiceBase
    {
        private IRepository<DM_KhachHang, Guid> _repository;
        private readonly IKhachHangRespository _customerRepo;
        private readonly IRepository<DM_NhomKhachHang, Guid> _nhomKhachHangRepository;
        private readonly IRepository<DM_LoaiKhach, int> _loaiKhachHangRepository;
        private readonly IRepository<DM_NguonKhach, Guid> _nguonKhachRepository;
        private readonly IKhachHangExcelExporter _khachHangExcelExporter;
        ITempFileCacheManager _tempFileCacheManager;
        public KhachHangAppService(IRepository<DM_KhachHang, Guid> repository,
              IKhachHangRespository customerRepo,
              IRepository<DM_NhomKhachHang, Guid> nhomKhachHangRepository,
              IRepository<DM_LoaiKhach, int> loaiKhachRepository,
              IRepository<DM_NguonKhach, Guid> nguonKhachRepository,
              IKhachHangExcelExporter khachHangExcelExporter,
              ITempFileCacheManager tempFileCacheManager
              )
        {
            _repository = repository;
            _customerRepo = customerRepo;
            _loaiKhachHangRepository = loaiKhachRepository;
            _nguonKhachRepository = nguonKhachRepository;
            _nhomKhachHangRepository = nhomKhachHangRepository;
            _khachHangExcelExporter = khachHangExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
        }
        [AbpAuthorize(PermissionNames.Pages_KhachHang_Create,PermissionNames.Pages_KhachHang_Edit)]
        public async Task<KhachHangDto> CreateOrEdit(CreateOrEditKhachHangDto dto)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (checkExist != null)
            {
                return await EditKhachHang(dto, checkExist);
            }
            return await CreateKhachHang(dto);
        }
        [NonAction]
        public async Task<KhachHangDto> CreateKhachHang(CreateOrEditKhachHangDto dto)
        {
            KhachHangDto result = new KhachHangDto();
            var checkMa = _repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToList();
            var khachHang = ObjectMapper.Map<DM_KhachHang>(dto);
            khachHang.Id = Guid.NewGuid();
            khachHang.MaKhachHang = "KH00" + checkMa.Count;
            khachHang.CreationTime = DateTime.Now;
            khachHang.CreatorUserId = AbpSession.UserId;
            khachHang.LastModificationTime = DateTime.Now;
            khachHang.LastModifierUserId = AbpSession.UserId;
            khachHang.TenantId = AbpSession.TenantId ?? 1;
            khachHang.IsDeleted = false;
            await _repository.InsertAsync(khachHang);
            result = ObjectMapper.Map<KhachHangDto>(khachHang);
            return result;
        }
        [NonAction]
        public async Task<KhachHangDto> EditKhachHang(CreateOrEditKhachHangDto dto, DM_KhachHang khachHang)
        {
            KhachHangDto result = new KhachHangDto();
            khachHang.TenKhachHang = dto.TenKhachHang;
            khachHang.DiaChi = dto.DiaChi;
            khachHang.IdLoaiKhach = dto.IdLoaiKhach;
            khachHang.IdNhomKhach = dto.IdNhomKhach;
            khachHang.IdNguonKhach = dto.IdNguonKhach;
            khachHang.Email = dto.Email;
            khachHang.SoDienThoai = dto.SoDienThoai;
            khachHang.NgaySinh = dto.NgaySinh;
            khachHang.TongTichDiem = dto.TongTichDiem;
            khachHang.GioiTinhNam = dto.GioiTinh;
            khachHang.Avatar = dto.Avatar;
            khachHang.MoTa = dto.MoTa;
            khachHang.LastModificationTime = DateTime.Now;
            khachHang.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(khachHang);
            result = ObjectMapper.Map<KhachHangDto>(khachHang);
            return result;
        }
        public async Task<CreateOrEditKhachHangDto> GetKhachHang(Guid id)
        {
            var KhachHang = await _repository.GetAsync(id);
            if (KhachHang != null)
            {
                var result = ObjectMapper.Map<CreateOrEditKhachHangDto>(KhachHang);
                result.GioiTinh = (bool)KhachHang.GioiTinhNam;
                return result;
            }

            return new CreateOrEditKhachHangDto();
        }

        [HttpPost]
        public async Task<KhachHangDto> Delete(Guid id)
        {
            KhachHangDto result = new KhachHangDto();
           
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 0;
                _repository.Update(delete);
                result = ObjectMapper.Map<KhachHangDto>(delete);
            }
            return result;
        }
        public async Task<DM_KhachHang> GetKhachHangDetail(Guid Id)
        {
            var KhachHang = await _repository.GetAsync(Id);
            return KhachHang;
        }

        public async Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            int tenantId = AbpSession.TenantId ?? 1;
            return await _customerRepo.Search(input, tenantId);
        }
        public async Task<List<KhachHangView>> GetKhachHang_noBooking(PagedKhachHangResultRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            int tenantId = AbpSession.TenantId ?? 1;
            return await _customerRepo.GetKhachHang_noBooking(input, tenantId);
        }

        public async Task<PagedResultDto<KhachHangView>> GetAll(PagedKhachHangResultRequestDto input)
        {
            PagedResultDto<KhachHangView> ListResultDto = new PagedResultDto<KhachHangView>();
            var lstData = await _repository.GetAll()
                .Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.keyword))
            {
                lstData = lstData.Where(
                    x => (x.TenKhachHang != null && x.TenKhachHang.Contains(input.keyword)) ||
                    (x.MaKhachHang != null && x.MaKhachHang.Contains(input.keyword)) ||
                    (x.MaSoThue != null && x.MaSoThue.Contains(input.keyword)) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(input.keyword)) ||
                    (x.DiaChi != null && x.DiaChi.Contains(input.keyword)) ||
                    (x.Email != null && x.Email.Contains(input.keyword))
                   ).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;

            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            List<KhachHangView> items = new List<KhachHangView>();
            foreach (var item in lstData)
            {
                KhachHangView rdo = new KhachHangView();
                rdo.Id = item.Id;
                rdo.MaKhachHang = item.MaKhachHang;
                rdo.Avatar = item.Avatar;
                rdo.TenKhachHang = item.TenKhachHang;
                rdo.CuocHenGanNhat = item.CreationTime;
                rdo.GioiTinh = item.GioiTinhNam == null ? "Khác" : (item.GioiTinhNam == true ? "Nam" : "Nữ");
                rdo.SoDienThoai = item.SoDienThoai;
                var nhomKhach = _nhomKhachHangRepository.FirstOrDefault(x => x.Id == item.IdNhomKhach);
                rdo.TenNhomKhach = nhomKhach == null ? "" : nhomKhach.TenNhomKhach;
                rdo.TongChiTieu = 0;
                rdo.TongTichDiem = item.TongTichDiem;
                items.Add(rdo);
            }
            ListResultDto.Items = items;
            return ListResultDto;
        }

        public async Task<List<KhachHangView>> JqAutoCustomer(PagedKhachHangResultRequestDto input)
        {
            try
            {
                return await _customerRepo.JqAutoCustomer(input, AbpSession.TenantId ?? 1);
            }
            catch (Exception)
            {
                return new List<KhachHangView>();
            }
        }
        [HttpGet]
        public async Task<bool> CheckExistMaKhachHang(string makhachhang, Guid? id = null)
        {
            if (id != null && id != Guid.Empty)
            {
                var lst = await _repository.GetAllListAsync(x => x.Id != id && x.MaKhachHang.ToUpper() == makhachhang.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _repository.GetAllListAsync(x => x.MaKhachHang.ToUpper() == makhachhang.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        [HttpGet]
        public async Task<bool> CheckExistSoDienThoai(string phone, Guid? id = null)
        {
            if (id != null && id != Guid.Empty)
            {
                var lst = await _repository.GetAllListAsync(x => x.Id != id && x.SoDienThoai.ToUpper() == phone.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _repository.GetAllListAsync(x => x.SoDienThoai.ToUpper() == phone.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        [AbpAuthorize(PermissionNames.Pages_KhachHang_Export)]
        public async Task<FileDto> ExportDanhSach(PagedKhachHangResultRequestDto input)
        {
            input.keyword = (input.keyword ?? string.Empty).Trim();
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await Search(input);
            List<KhachHangView> model = new List<KhachHangView>();
            model = (List<KhachHangView>)data.Items;
            return _khachHangExcelExporter.ExportDanhSachKhachHang(model);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_KhachHang_Import)]
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
                                CreateOrEditKhachHangDto data = new CreateOrEditKhachHangDto();
                                data.Id = Guid.NewGuid();
                                data.TenKhachHang = worksheet.Cells[row, 2].Value?.ToString();
                                data.SoDienThoai = worksheet.Cells[row, 3].Value?.ToString();
                                var checkPhoneNumber = await _repository.FirstOrDefaultAsync(x => x.SoDienThoai == data.SoDienThoai);
                                await UnitOfWorkManager.Current.SaveChangesAsync();
                                if (checkPhoneNumber != null || data.TenKhachHang == null || data.SoDienThoai == null)
                                {
                                    countImportLoi++;
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(worksheet.Cells[row, 4].Value?.ToString()))
                                {
                                    data.NgaySinh = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString());
                                }

                                data.DiaChi = worksheet.Cells[row, 5].Value?.ToString();
                                data.Email = worksheet.Cells[row, 7].Value?.ToString();
                                if (worksheet.Cells[row, 6].Value?.ToString().ToLower() == "nam")
                                {
                                    data.GioiTinh = true;
                                }
                                else
                                {
                                    data.GioiTinh = false;
                                }
                                data.TongTichDiem = 0;
                                data.TrangThai = 1;
                                data.KieuNgaySinh = 1;
                                await CreateKhachHang(data);
                                countImportData++;
                            }

                        }
                        if (countImportData >0 )
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
