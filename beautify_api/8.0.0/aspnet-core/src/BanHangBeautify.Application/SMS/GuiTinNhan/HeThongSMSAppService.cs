using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.ESMS;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.SMS.GuiTinNhan
{
    [AbpAuthorize(PermissionNames.Pages_HeThongSMS)]
    public class HeThongSMSAppService : SPAAppServiceBase
    {
        private readonly IRepository<HeThong_SMS, Guid> _hethongSMS;
        private readonly IHeThongSMSRepository _repoSMS;
        private readonly IExcelBase _excelBase;
        private readonly IESMS _eSMS;

        public HeThongSMSAppService(IRepository<HeThong_SMS, Guid> repository, IHeThongSMSRepository repoSMS, IExcelBase excelBase, IESMS eSMS)
        {
            _hethongSMS = repository;
            _repoSMS = repoSMS;
            _excelBase = excelBase;
            _eSMS = eSMS;
        }
        [HttpPost]
        public async Task<CreateOrEditHeThongSMSDto> Insert_HeThongSMS(CreateOrEditHeThongSMSDto input)
        {
            HeThong_SMS data = ObjectMapper.Map<HeThong_SMS>(input);
            data.Id = Guid.NewGuid();
            if (input.IdKhachHang == Guid.Empty)
            {
                data.IdKhachHang = null;
            }
            data.ThoiGianGui = DateTime.Now;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IdNguoiGui = AbpSession.UserId;
            data.IsDeleted = false;
            await _hethongSMS.InsertAsync(data);
            CreateOrEditHeThongSMSDto result = ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            return result;
        }

        [HttpPost]
        public async Task<CreateOrEditHeThongSMSDto> Update_HeThongSMS(CreateOrEditHeThongSMSDto input)
        {
            HeThong_SMS data = _hethongSMS.FirstOrDefault(input.Id);
            if (data == null) return null;
            data.ThoiGianGui = DateTime.Now;
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = AbpSession.UserId;
            data.IdNguoiGui = AbpSession.UserId;
            data.SoTinGui = input.SoTinGui ?? 0;
            if (input.IdKhachHang == Guid.Empty)
            {
                data.IdKhachHang = null;
            }
            else
            {
                data.IdKhachHang = input.IdKhachHang;
            }
            data.IdHoaDon = input.IdHoaDon;
            data.IdTinNhan = input.IdTinNhan;
            data.SoDienThoai = input.SoDienThoai;
            data.NoiDungTin = input.NoiDungTin;
            data.IdLoaiTin = (byte)input.IdLoaiTin;
            data.TrangThai = input.TrangThai;
            data.GiaTienMoiTinNhan = input.GiaTienMoiTinNhan;
            data.HinhThucGui = input.HinhThucGui;
            await _hethongSMS.UpdateAsync(data);
            CreateOrEditHeThongSMSDto result = ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> GuiLai_TinNhan_ThatBai(List<Guid> listID, string brandname)
        {
            try
            {
                int countSuccess = 0, status = 0;
                if (string.IsNullOrEmpty(brandname))
                {
                    return new JsonResult(new { Success = 0, Err = listID.Count, MessageStatus = ConstSMS.ESMS_TrangThaiTin.BRANDNAME_NOTEXIST });
                }

                List<HeThong_SMS> lst = _hethongSMS.GetAllList(x => listID.Contains(x.Id));
                foreach (var item in lst)
                {
                    ESMSDto eSMSDto = new()
                    {
                        Phone = item.SoDienThoai,
                        Content = item.NoiDungTin,
                        Brandname = brandname,
                    };
                    ResultSMSDto smsResult = await _eSMS.SendSMS_Json(eSMSDto);

                    HeThong_SMS objUp = _hethongSMS.FirstOrDefault(item.Id);
                    if (objUp != null)
                    {
                        objUp.LastModificationTime = DateTime.Now;
                        objUp.LastModifierUserId = AbpSession.UserId;
                        objUp.IdTinNhan = smsResult.MessageId;
                        objUp.TrangThai = smsResult.MessageStatus;
                        await _hethongSMS.UpdateAsync(objUp);

                        if (smsResult.MessageStatus == 100)
                        {
                            countSuccess++;
                        }
                        else
                        {
                            status = smsResult.MessageStatus;
                        }
                    }
                }
                var data = new { Success = countSuccess, Err = lst.Count - countSuccess, MessageStatus = status };
                return new JsonResult(data);
            }
            catch (Exception)
            {
                return new JsonResult(new { Success = 0, Err = listID.Count, MessageStatus = ConstSMS.ESMS_TrangThaiTin.ERROR_UNDEFINED });
            }
        }
        [HttpGet]
        public async Task<CreateOrEditHeThongSMSDto> HeThongSMS_DeleteById(Guid id)
        {
            var data = await _hethongSMS.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _hethongSMS.UpdateAsync(data);
                return ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            }
            return new CreateOrEditHeThongSMSDto();
        }
        [HttpGet]
        public async Task<CreateOrEditHeThongSMSDto> GetForEdit(Guid id)
        {
            var data = await _hethongSMS.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditHeThongSMSDto>(data);
            }
            return new CreateOrEditHeThongSMSDto();
        }
        [HttpPost]
        public async Task<PagedResultDto<CreateOrEditHeThongSMSDto>> GetListSMS(ParamSearch input)
        {
            return await _repoSMS.GetListSMS(input);
        }
        [HttpPost]
        public async Task<List<PageKhachHangSMSDto>> JqAutoCustomer_byIdLoaiTin(ParamSearchSMS input, int? idLoaiTin = 1)
        {
            //try
            //{
                var data =  await _repoSMS.GetListCustomer_byIdLoaiTin(input, idLoaiTin);
                return (List<PageKhachHangSMSDto>)data.Items;
            //}
            //catch (Exception)
            //{
            //    return new List<CustomerWithZOA>();
            //}
        }

        [HttpPost]
        public async Task<PagedResultDto<PageKhachHangSMSDto>> GetListCustomer_byIdLoaiTin(ParamSearchSMS input, int? idLoaiTin = 1)
        {
            try
            {
                return await _repoSMS.GetListCustomer_byIdLoaiTin(input, idLoaiTin);
            }
            catch (Exception)
            {
                return new PagedResultDto<PageKhachHangSMSDto>();
            }
        }
        public async Task<bool> ThemMoi_NhatKyGuiTin(NhatKyGuiTinSMSDto input)
        {
            var data = await _repoSMS.InsertNhatKyGuiTinSMS(input, AbpSession.TenantId ?? 1);
            return data > 0; ;
        }

        #region Export Excel
        [HttpPost]
        public async Task<FileDto> ExportToExcel_DanhSachTinNhan(ParamSearch input)
        {
            var data = await _repoSMS.GetListSMS(input);
            var dataExcel = ObjectMapper.Map<List<CreateOrEditHeThongSMSDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.ThoiGianGui,
                x.TenKhachHang,
                x.SoDienThoai,
                x.LoaiTin,
                x.NoiDungTin,
            }).ToList();

            string fileName = string.Empty, fileTitle = string.Empty;
            if (input.TrangThais != null && input.TrangThais.Count > 0)
            {
                var sTrangThai = String.Join(",", input.TrangThais);
                switch (sTrangThai)
                {
                    case "1":
                        fileName = "SMSTinNhap_";
                        fileTitle = "DANH SÁCH TIN NHẮN LƯU NHÁP";
                        break;
                    case "100":
                        fileName = "SMSDaGui_";
                        fileTitle = "DANH SÁCH TIN NHẮN ĐÃ GỬI";
                        break;
                    default:
                        fileName = "SMSKhongThanhCong_";
                        fileTitle = "DANH SÁCH TIN NHẮN GỬI KHÔNG THÀNH CÔNG";
                        break;

                }
            }

            List<Excel_CellData> lst = new()
            {
                new Excel_CellData { RowIndex = 1, ColumnIndex = 1, CellValue = fileTitle }
            };
            return _excelBase.WriteToExcel(fileName, @"SMS\SMS_DanhSachTinNhan_Template.xlsx", dataNew, 4, lst);
        }

        [HttpPost]
        public async Task<FileDto> ExportToExcel_DanhSachKhachHang_SMS(ParamSearchSMS input, int? idLoaiTin = 1)
        {
            var data = await _repoSMS.GetListCustomer_byIdLoaiTin(input, idLoaiTin);
            var dataExcel = ObjectMapper.Map<List<PageKhachHangSMSDto>>(data.Items);

            FileDto fileReturn = new FileDto();
            string fileName = string.Empty, dateFromTo = string.Empty;
            if (input.FromDate != null && input.ToDate != null)
            {
                dateFromTo = $"Thời gian: {input.FromDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}" +
                    $" - {input.ToDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
            }
            List<Excel_CellData> lst = new();
            switch (idLoaiTin)
            {
                case 2:// sinhnhat 
                    {
                        fileName = "DSKhachHang_SinhNhat_";
                        var dataNew = dataExcel.Select(x => new
                        {
                            x.TenKhachHang,
                            x.SoDienThoai,
                            x.NgaySinh,
                            x.STrangThaiGuiTinNhan,
                        }).ToList();
                        lst.Add(new Excel_CellData { RowIndex = 2, ColumnIndex = 1, CellValue = dateFromTo });
                        fileReturn = _excelBase.WriteToExcel(fileName, @"SMS\SMS_KhachHangSinhNhat_Template.xlsx", dataNew, 4, lst);
                    }
                    break;
                case 3:
                    {
                        fileName = "DSKhachHang_LichHen_";
                        var dataNew = dataExcel.Select(x => new
                        {
                            x.TenKhachHang,
                            x.SoDienThoai,
                            x.BookingDate,
                            x.ThoiGianHen,
                            x.TenHangHoa,
                            x.STrangThaiGuiTinNhan,
                        }).ToList();
                        lst.Add(new Excel_CellData { RowIndex = 2, ColumnIndex = 1, CellValue = dateFromTo });
                        fileReturn = _excelBase.WriteToExcel(fileName, @"SMS\SMS_KhachHangLichHen_Template.xlsx", dataNew, 4, lst);
                    }
                    break;
                case 4:
                    {
                        fileName = "DSKhachHang_GiaoDicH_";
                        var dataNew = dataExcel.Select(x => new
                        {
                            x.TenKhachHang,
                            x.SoDienThoai,
                            x.MaHoaDon,
                            x.NgayLapHoaDon,
                            x.STrangThaiGuiTinNhan,
                        }).ToList();
                        lst.Add(new Excel_CellData { RowIndex = 2, ColumnIndex = 1, CellValue = dateFromTo });
                        fileReturn = _excelBase.WriteToExcel(fileName, @"SMS\SMS_KhachHangGiaoDich_Template.xlsx", dataNew, 4, lst);
                    }
                    break;
            }
            return fileReturn;
        }
        #endregion
    }
}
