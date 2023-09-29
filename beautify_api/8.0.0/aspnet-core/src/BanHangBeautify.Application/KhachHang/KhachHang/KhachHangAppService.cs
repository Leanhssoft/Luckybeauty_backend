using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Exporting;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using BanHangBeautify.NewFolder;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static BanHangBeautify.Configuration.Common.CommonClass;
using static BanHangBeautify.Configuration.Common.ObjectHelper;


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
        private readonly IRepository<Booking, Guid> _bookingRepository;
        private readonly IKhachHangExcelExporter _khachHangExcelExporter;
        ITempFileCacheManager _tempFileCacheManager;
        public KhachHangAppService(IRepository<DM_KhachHang, Guid> repository,
              IKhachHangRespository customerRepo,
              IRepository<DM_NhomKhachHang, Guid> nhomKhachHangRepository,
              IRepository<DM_LoaiKhach, int> loaiKhachRepository,
              IRepository<DM_NguonKhach, Guid> nguonKhachRepository,
              IRepository<Booking, Guid> bookingRepository,
              IKhachHangExcelExporter khachHangExcelExporter,
              ITempFileCacheManager tempFileCacheManager
              )
        {
            _repository = repository;
            _customerRepo = customerRepo;
            _loaiKhachHangRepository = loaiKhachRepository;
            _nguonKhachRepository = nguonKhachRepository;
            _nhomKhachHangRepository = nhomKhachHangRepository;
            _bookingRepository = bookingRepository;
            _khachHangExcelExporter = khachHangExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
        }
        [AbpAuthorize(PermissionNames.Pages_KhachHang_Create, PermissionNames.Pages_KhachHang_Edit)]
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
            
            
            var khachHang = ObjectMapper.Map<DM_KhachHang>(dto);
            khachHang.Id = Guid.NewGuid();
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var checkMa = _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).ToList();
                khachHang.MaKhachHang = "KH00" + (checkMa.Count + 1).ToString();
            }
            khachHang.CreationTime = DateTime.Now;
            khachHang.GioiTinhNam = dto.GioiTinh;
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
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_CaLamViec_Delete)]
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            var checkExists = await _repository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
            if (checkExists != null && checkExists.Count > 0)
            {
                _repository.RemoveRange(checkExists);
                result.Status = "success";
                result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
            }
            return result;
        }
        [HttpPost]
        public async Task DeleteMultipleCustomer(List<Guid> lstId)
        {
            _repository.GetAllList(x => lstId.Contains(x.Id)).ForEach(x =>
            {
                x.TrangThai = 0;
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
            });
            // todo remove image in google api
        }
        [HttpPost]
        public async Task ChuyenNhomKhachHang(List<Guid> lstIdKhachHang, Guid idNhomKhach)
        {
            _repository.GetAllList(x => lstIdKhachHang.Contains(x.Id)).ForEach(x =>
            {
                x.IdNhomKhach = idNhomKhach;
                x.LastModifierUserId = AbpSession.UserId;
                x.LastModificationTime = DateTime.Now;
            });
        }
        public async Task<KhachHangDetailDto> GetKhachHangDetail(Guid id)
        {
            KhachHangDetailDto result = new KhachHangDetailDto();
            var khachHang = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (khachHang != null)
            {
                result.Id = khachHang.Id;
                result.Avatar = khachHang.Avatar;
                result.TenKhachHang = khachHang.TenKhachHang;
                result.MaKhachHang = khachHang.MaKhachHang;
                result.NgaySinh = khachHang.NgaySinh.HasValue ? khachHang.NgaySinh.Value.ToString("dd/MM/yyyy") : "";
                result.SoDienThoai = khachHang.SoDienThoai;
                result.DiaChi = khachHang.DiaChi;
                result.Email = khachHang.Email;
                result.GioiTinh = khachHang.GioiTinhNam.HasValue ? khachHang.GioiTinhNam.Value ? "Nam" : "Nữ" : "Khác";
                result.DiemThuong = khachHang.TongTichDiem ?? 0;
                result.MaSoThue = khachHang.MaSoThue;
                var loaiKhach = await _loaiKhachHangRepository.FirstOrDefaultAsync(x => x.Id == khachHang.IdLoaiKhach);
                result.LoaiKhach = loaiKhach != null ? loaiKhach.TenLoaiKhachHang : "";
                var nhomKhach = await _nhomKhachHangRepository.FirstOrDefaultAsync(h => h.Id == khachHang.IdNhomKhach);
                result.NhomKhach = nhomKhach != null ? nhomKhach.TenNhomKhach : "";
                var nguonKhach = await _nguonKhachRepository.FirstOrDefaultAsync(x => x.Id == khachHang.IdNguonKhach);
                result.NguonKhach = nguonKhach != null ? nguonKhach.TenNguon : "";
            }
            return result;
        }
        public async Task<PagedResultDto<LichSuDatLichDto>> LichSuDatLich(Guid idKhachHang)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            return await _customerRepo.LichSuDatLich(idKhachHang, tenantId);
        }
        public async Task<PagedResultDto<LichSuHoaDonDto>> LichSuGiaoDich(Guid idKhachHang)
        {
            int tenantId = AbpSession.TenantId ?? 1;
            return await _customerRepo.LichSuGiaoDich(idKhachHang, tenantId);
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
                var booking = await _bookingRepository.GetAll().Where(x => x.IdKhachHang == item.Id && x.IsDeleted == false).OrderByDescending(x => x.BookingDate).ToListAsync();
                if (booking != null && booking.Count > 0)
                {
                    rdo.CuocHenGanNhat = booking[0].BookingDate;
                }
                else
                {
                    rdo.CuocHenGanNhat = item.CreationTime;
                }
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
        public async Task<FileDto> ExporSelectedtDanhSach(List<Guid> IdKhachHangs)
        {
            PagedKhachHangResultRequestDto input = new PagedKhachHangResultRequestDto();
            input.keyword = "";
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var data = await Search(input);
            List<KhachHangView> model = new List<KhachHangView>();
            model = (List<KhachHangView>)data.Items.Where(x=>IdKhachHangs.Contains(x.Id)).ToList();
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
        public async Task<List<ExcelErrorDto>> ImportFile_DanhMucKhachHang(FileUpload file)
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
                // cột C: số điện thoại (cột thứ 3), đọc bắt đàu từ dòng số 3 --> rowCount
                var errDuplicate = Excel_CheckDuplicateData(worksheet, "C", 3, 3, rowCount);
                if (errDuplicate.Count > 0)
                {
                    foreach (var item in errDuplicate)
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = item.RowNumber,
                            TenTruongDuLieu = "Số điện thoại",
                            GiaTriDuLieu = item.GiaTriDuLieu,
                            DienGiai = "Số điện thoại bị trùng lặp",
                            LoaiErr = 1,
                        });
                    }
                }
                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string tenNhomKhachHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string tenKhachHang = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    string soDienThoai = worksheet.Cells[i, 3].Value?.ToString().Trim();
                    string ngaySinh = worksheet.Cells[i, 4].Value?.ToString();
                    string gioiTinh = worksheet.Cells[i, 5].Value?.ToString();
                    string diaChi = worksheet.Cells[i, 6].Value?.ToString();
                    string ghiChu = worksheet.Cells[i, 7].Value?.ToString();

                    // nếu dòng trống: bỏ qua và nhảy sang dòng tiếp theo
                    if (!string.IsNullOrEmpty(tenNhomKhachHang)
                            || !string.IsNullOrEmpty(tenKhachHang)
                           || !string.IsNullOrEmpty(soDienThoai)
                           || !string.IsNullOrEmpty(ngaySinh)
                           || !string.IsNullOrEmpty(diaChi)
                           )
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }

                    if (string.IsNullOrEmpty(tenKhachHang))
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Tên khách hàng",
                            GiaTriDuLieu = tenKhachHang,
                            DienGiai = "Tên khách hàng không được để trống",
                            LoaiErr = 1,
                        });
                    }

                    //if (string.IsNullOrEmpty(soDienThoai))
                    //{
                    //    lstErr.Add(new ExcelErrorDto
                    //    {
                    //        RowNumber = i,
                    //        TenTruongDuLieu = "Số điện thoại",
                    //        GiaTriDuLieu = soDienThoai,
                    //        DienGiai = "Số điện thoại không được để trống",
                    //        LoaiErr = 1,
                    //    });
                    //}
                    //else
                    //{
                    //    // sodienthoai exists DB
                    //}

                    if (!string.IsNullOrEmpty(ngaySinh))
                    {
                        try
                        {
                            Convert.ToDateTime(ngaySinh);
                        }
                        catch (Exception)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Ngày sinh",
                                GiaTriDuLieu = ngaySinh,
                                DienGiai = "Ngày sinh không đúng định dạng",
                                LoaiErr = 1,
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(gioiTinh))
                    {
                        string[] arrLoai = { "Nam", "Nữ" };
                        if (!arrLoai.Contains(gioiTinh))
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Giới tính",
                                GiaTriDuLieu = gioiTinh,
                                DienGiai = "Giới tính không đúng định dạng",
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
                lstErr = await Execute_ImportDanhMucKhachHang(file);// phải load lại file, vì Excel bị dispose
            }
            return lstErr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<ExcelErrorDto>> Execute_ImportDanhMucKhachHang(FileUpload file)
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
                    string tenNhomKhachHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string tenKhachHang = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    string soDienThoai = worksheet.Cells[i, 3].Value?.ToString().Trim();
                    string ngaySinh = worksheet.Cells[i, 4].Value?.ToString();
                    string gioiTinh = worksheet.Cells[i, 5].Value?.ToString();
                    string diaChi = worksheet.Cells[i, 6].Value?.ToString();
                    string ghiChu = worksheet.Cells[i, 7].Value?.ToString();

                    if (!string.IsNullOrEmpty(tenNhomKhachHang)
                             || !string.IsNullOrEmpty(tenKhachHang)
                            || !string.IsNullOrEmpty(soDienThoai)
                            || !string.IsNullOrEmpty(ngaySinh)
                            || !string.IsNullOrEmpty(diaChi)
                            )
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }

                    DateTime? ngaySinhFormat = null;
                    if (!string.IsNullOrEmpty(ngaySinh))
                    {
                        ngaySinhFormat = Convert.ToDateTime(ngaySinh);
                    }

                    ImportExcelKhachHangDto newObj = new()
                    {
                        TenNhomKhachHang = tenNhomKhachHang,
                        TenKhachHang = tenKhachHang,
                        SoDienThoai = soDienThoai,
                        GioiTinhNam = gioiTinh == "Nam" ? true : false,
                        NgaySinh = ngaySinhFormat,
                        DiaChi = diaChi,
                        MoTa = ghiChu
                    };
                    try
                    {
                        await _customerRepo.ImportDanhMucKhachHang(AbpSession.TenantId ?? 1, AbpSession.UserId, newObj);
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
                    TenTruongDuLieu = "Exception",
                    GiaTriDuLieu = "Exception",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            return lstErr;
        }
    }
}
