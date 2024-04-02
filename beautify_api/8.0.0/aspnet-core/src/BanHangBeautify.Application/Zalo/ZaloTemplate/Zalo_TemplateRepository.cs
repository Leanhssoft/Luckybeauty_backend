using Abp.EntityFrameworkCore;
using AutoMapper.Internal.Mappers;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.SMS.Dto;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.ZaloTemplate
{
    public class Zalo_TemplateRepository : SPARepositoryBase<Zalo_Template, Guid>, IZalo_TemplateRepository
    {
        List<Zalo_TemplateDto> _lstAllTemp;
        public Zalo_TemplateRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
            InnitData_TempZalo();
        }

        private Zalo_TemplateDto InitData_TempSinhNhat()
        {
            Guid zaloIdTemp = new("583D011C-78F5-48D4-A284-898441E00987");
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin ="Chúc mừng sinh nhật",
                //MoTaChiTiet = "Gửi lời chúc mừng đến khách hàng sinh nhật",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.SinhNhat,
                TemplateType = ZaloTemplateType.PROMOTION,
                Language = "VI"
            };

            Guid zaloIdElement = new("E6041AC0-806E-4835-8F21-DBD69CC6A170");
            List<Zalo_ElementDto> lstElm = new()
            {
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.HEADER,
                    Content = "💥💥Happy Birthday💥💥",
                    IsImage = false,
                    ThuTuSapXep = 1,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType =ZaloElementType.TEXT,
                    Content = @"<TenChiNhanh> kính chúc <TenKhachHang> có một ngày sinh nhật ý nghĩa bên người thân và gia đình",
                    IsImage = false,
                    ThuTuSapXep = 2,
                }
            };
            zalotemp.elements = lstElm;
            return zalotemp;
        }
        private Zalo_TemplateDto InitData_TempGiaoDich()
        {
            Guid zaloIdTemp = new("3ABE0F1B-D70E-4A1A-B5DE-2BC8CCD71514");
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin = "Xác nhận giao dịch",
                //MoTaChiTiet = "Gửi lời chúc mừng đến khách hàng sinh nhật",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.GiaoDich,
                TemplateType = ZaloTemplateType.TRANSACTION,
                Language = "VI"
            };

            Guid zaloIdElement = new("F8A61D16-801F-4B31-8A60-EAAE3BE72CC7");

            List<Zalo_TableDetailDto> tables = new()
            {
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Mã hóa đơn",
                    Value = "<MaHoaDon>",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Ngày mua hàng",
                    Value = "<NgayLapHoaDon>",
                    ThuTuSapXep= 2,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tổng tiền",
                    Value = "<TongTienHang>",
                    ThuTuSapXep= 3,
                }
            };

            List<Zalo_ElementDto> lstElm = new()
            {
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.BANNER,
                    Content = @"https://lh3.googleusercontent.com/d/1TDXeqE458lvu9DJXFg85FtBEuC_1OHUw",
                    IsImage = true,
                    ThuTuSapXep = 1,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.HEADER,
                    Content = @"Thông báo giao dịch",
                    IsImage = false,
                    ThuTuSapXep = 2,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.TEXT,
                    Content = @"Xin chào <TenKhachHang>, cảm ơn bạn đã mua hàng tại cửa hàng. Chúng tôi đã ghi nhận thanh toán của bạn với chi tiết như sau:",
                    IsImage = false,
                    ThuTuSapXep = 3,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.TABLE,
                    IsImage = false,
                    ThuTuSapXep = 4,
                    tables = tables,
                }
            };

            List<Zalo_ButtonDetailDto> buttons = new()
            {
                new Zalo_ButtonDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdTemplate = zaloIdTemp,
                    Type = ZaloButtonType.URL,
                    Title = "Xem chi tiết đơn hàng",
                    Payload = "https://login.luckybeauty.vn/giao-dich-thanh-toan",
                    ThuTuSapXep= 1,
                },
            };
            zalotemp.elements = lstElm;
            zalotemp.buttons = buttons;

            return zalotemp;
        }
        private Zalo_TemplateDto InitData_TempXacNhanLichHen()
        {
            Guid zaloIdTemp = new("6967F1E7-A5B3-46C4-8E26-4955C4F4801C");
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin = "Xác nhận lịch hẹn",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.LichHen,
                TemplateType = ZaloTemplateType.BOOKING,
                Language = "VI"
            };

            Guid zaloIdElement = new("0C469F21-DBC6-41D0-B7F0-5F4AB46E95AB");

            List<Zalo_TableDetailDto> tables = new()
            {
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Mã đặt lịch",
                    Value = "<SoDienThoai>",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tên khách hàng",
                    Value = "<TenKhachHang>",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Ngày đặt",
                    Value = "<BookingDate>",
                    ThuTuSapXep= 3,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tên dịch vụ",
                    Value = "<TenDichVu>",
                    ThuTuSapXep= 4,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Địa chỉ cơ sở",
                    Value = "<DiaChiChiNhanh>",
                    ThuTuSapXep= 5
                }
            };

            List<Zalo_ElementDto> lstElm = new()
            {
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.BANNER,
                    Content = @"https://lh3.googleusercontent.com/d/1TDXeqE458lvu9DJXFg85FtBEuC_1OHUw",// todo logo banner
                    IsImage = true,
                    ThuTuSapXep = 1,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.HEADER,
                    Content = "Xác nhận lịch hẹn",
                    IsImage = false,
                    ThuTuSapXep = 2,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.TEXT,
                    Content = @"Cảm ơn quý khách đã đặt lịch sử dụng dịch vụ của chúng tôi. Lịch hẹn của bạn đã được xác nhận với chi tiết như sau:",
                    IsImage = false,
                    ThuTuSapXep = 3,
                },
                new Zalo_ElementDto
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = ZaloElementType.TABLE,
                    IsImage = false,
                    ThuTuSapXep = 4,
                    tables = tables,
                }
            };

            List<Zalo_ButtonDetailDto> buttons = new()
            {
                new Zalo_ButtonDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdTemplate = zaloIdTemp,
                    Type = ZaloButtonType.PHONE,
                    Title = "Liên hệ CSKH",
                    Payload = "02473039333", //todo sdt cuahang
                    ThuTuSapXep= 1,
                },
            };
            zalotemp.elements = lstElm;
            zalotemp.buttons = buttons;

            return zalotemp;
        }
        public List<Zalo_TemplateDto> InnitData_TempZalo()
        {
            var obj1 = InitData_TempSinhNhat();
            var obj2 = InitData_TempGiaoDich();
            var obj3 = InitData_TempXacNhanLichHen();
            List<Zalo_TemplateDto> lst = new()
            {
                obj1,
                obj2,
                obj3
            };
            _lstAllTemp = lst;
            return lst;
        }
        public async Task<Zalo_TemplateDto> FindTempDefault_ByIdLoaiTin(byte idLoaiTin)
        {
            // todo get from DB (with Sp sql)
            if (_lstAllTemp == null) return null;
            var data = _lstAllTemp.Where(x => x.IdLoaiTin == idLoaiTin);
            if (data != null && data.Any())
            {
                return data.FirstOrDefault();
            }
            return null;
        }
    }
}
