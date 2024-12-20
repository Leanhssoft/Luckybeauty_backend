﻿using Abp.EntityFrameworkCore;
using Abp.ObjectMapping;
using AutoMapper.Internal.Mappers;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.EntityFrameworkCore;
using BanHangBeautify.EntityFrameworkCore.Repositories;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Zalo.GuiTinNhan;
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
        public Zalo_TemplateRepository(IDbContextProvider<SPADbContext> dbContextProvider) : base(dbContextProvider)
        {
            InnitData_TempZalo();
        }

        private Zalo_TemplateDto InitData_TempSinhNhat()
        {
            Guid zaloIdTemp = Guid.NewGuid();
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin = "Chúc mừng sinh nhật",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.SinhNhat,
                TemplateType = ZaloTemplateType.PROMOTION,
                Language = "VI",
                IsSystem = true,
            };

            Guid zaloIdElement = Guid.NewGuid();
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
                    Content = @"{TenChiNhanh} kính chúc {TenKhachHang} có một ngày sinh nhật ý nghĩa bên người thân và gia đình",
                    IsImage = false,
                    ThuTuSapXep = 2,
                }
            };
            zalotemp.elements = lstElm;
            return zalotemp;
        }
        private Zalo_TemplateDto InitData_TempGiaoDich()
        {
            Guid zaloIdTemp = Guid.NewGuid();
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin = "Xác nhận giao dịch",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.GiaoDich,
                TemplateType = ZaloTemplateType.TRANSACTION,
                Language = "VI",
                IsSystem = true
            };

            Guid zaloIdElement = Guid.NewGuid();

            List<Zalo_TableDetailDto> tables = new()
            {
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Mã hóa đơn",
                    Value = "{MaHoaDon}",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Ngày mua hàng",
                    Value = "{NgayLapHoaDon}",
                    ThuTuSapXep= 2,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tổng tiền",
                    Value = "{TongTienHang}",
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
                    Content = @"Xin chào {TenKhachHang}, cảm ơn bạn đã mua hàng tại cửa hàng. Chúng tôi đã ghi nhận thanh toán của bạn với chi tiết như sau:",
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
            Guid zaloIdTemp = Guid.NewGuid();
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin = "Xác nhận lịch hẹn",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.LichHen,
                TemplateType = ZaloTemplateType.BOOKING,
                Language = "VI",
                IsSystem = true
            };

            Guid zaloIdElement = Guid.NewGuid();

            List<Zalo_TableDetailDto> tables = new()
            {
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Mã đặt lịch",
                    Value = "{SoDienThoai}",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tên khách hàng",
                    Value = "{TenKhachHang}",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Ngày đặt",
                    Value = "{BookingDate}",
                    ThuTuSapXep= 3,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tên dịch vụ",
                    Value = "{TenDichVu}",
                    ThuTuSapXep= 4,
                },
                new Zalo_TableDetailDto
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Địa chỉ cơ sở",
                    Value = "{DiaChiChiNhanh}",
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
                    Payload = "02437919191",// todo sdt cửa hàng
                    ThuTuSapXep= 1,
                },
            };
            zalotemp.elements = lstElm;
            zalotemp.buttons = buttons;

            return zalotemp;
        }
        private Zalo_TemplateDto InitData_NhacLichHen()
        {
            Guid zaloIdTemp = Guid.NewGuid();
            Zalo_TemplateDto zalotemp = new()
            {
                Id = zaloIdTemp,
                TenMauTin = "Nhắc lịch hẹn",
                IsDefault = true,
                IdLoaiTin = ConstSMS.LoaiTin.LichHen,
                TemplateType = ZaloTemplateType.PROMOTION,
                Language = "VI",
                IsSystem = true
            };

            Guid zaloIdElement = Guid.NewGuid();

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
                    Content = @"Quý khách {TenKhachHang} có lịch hẹn vào lúc {BookingDate} tại {DiaChiChiNhanh} với mã đặt lịch {SoDienThoai}",
                    IsImage = false,
                    ThuTuSapXep = 3,
                },
                //new Zalo_ElementDto
                //{
                //    Id = zaloIdElement,
                //    IdTemplate = zaloIdTemp,
                //    ElementType = ZaloElementType.TEXT,
                //    Content = @"Vui lòng đến đúng giờ hẹn để được phục vụ tốt nhất. Hẹn gặp quý khách tại cơ sở của chúng tôi!",
                //    IsImage = false,
                //    ThuTuSapXep = 4,
                //}
            };

            List<Zalo_ButtonDetailDto> buttons = new()
            {
                new Zalo_ButtonDetailDto
                {
                    Id = Guid.NewGuid(),
                    IdTemplate = zaloIdTemp,
                    Type = ZaloButtonType.PHONE,
                    Title = "Liên hệ CSKH",
                    Payload = "02437919191",// todo sdt cửa hàng
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
            var obj4 = InitData_NhacLichHen();
            List<Zalo_TemplateDto> lst = new()
            {
                obj1,
                obj2,
                obj3,
                obj4
            };
            return lst;
        }
        public async Task<Zalo_TemplateDto> FindTempDefault_ByIdLoaiTin(byte idLoaiTin)
        {
            var dbContext = GetDbContext();
            var objFind = dbContext.Set<Zalo_Template>().Where(x => x.IdLoaiTin == idLoaiTin && (x.IsDefault ?? false))
                .Select(x => new Zalo_TemplateDto
                {
                    Id = x.Id,
                    TenMauTin = x.TenMauTin,
                    IdLoaiTin = x.IdLoaiTin,
                    IsDefault = x.IsDefault,
                    TemplateType = x.TemplateType,
                    Language = x.Language,
                    IsSystem = false
                }).ToList();
            if (objFind != null && objFind.Count > 0)
            {
                return GetZaloTemplate_byId(objFind.FirstOrDefault().Id);
            }
            return null;
        }

        public Zalo_TemplateDto GetZaloTemplate_byId(Guid id)
        {
            try
            {
                var dbContext = GetDbContext();
                var objFind = dbContext.Set<Zalo_Template>().Where(x => x.Id == id).Select(x => new Zalo_TemplateDto
                {
                    Id = x.Id,
                    TenMauTin = x.TenMauTin,
                    IdLoaiTin = x.IdLoaiTin,
                    IsDefault = x.IsDefault,
                    TemplateType = x.TemplateType,
                    Language = x.Language,
                    IsSystem = false
                }).ToList();

                if (objFind != null && objFind.Count > 0)
                {
                    var zaloTemp = objFind.FirstOrDefault();
                    var lstBtn = dbContext.Set<Zalo_ButtonDetail>().Where(x => x.IdTemplate == id)
                        .Select(x => new Zalo_ButtonDetailDto
                        {
                            Id = x.Id,
                            IdTemplate = x.IdTemplate,
                            Type = x.Type,
                            Title = x.Title,
                            Payload = x.Payload,
                            ImageIcon = x.ImageIcon,
                            ThuTuSapXep = x.ThuTuSapXep
                        }).OrderBy(x => x.ThuTuSapXep).ToList();

                    var lstElm = dbContext.Set<Zalo_Element>().Where(x => x.IdTemplate == id)
                        .Select(x => new Zalo_ElementDto
                        {
                            Id = x.Id,
                            IdTemplate = x.IdTemplate,
                            ElementType = x.ElementType,
                            IsImage = x.IsImage,
                            Content = x.Content,
                            ThuTuSapXep = x.ThuTuSapXep,
                            tables = dbContext.Set<Zalo_TableDetail>().Where(o => o.IdElement == x.Id)
                            .Select(o => new Zalo_TableDetailDto
                            {
                                Id = o.Id,
                                IdElement = o.IdElement,
                                Key = o.Key,
                                Value = o.Value,
                                ThuTuSapXep = o.ThuTuSapXep,
                            }).OrderBy(o => o.ThuTuSapXep).ToList(),
                        }).OrderBy(x => x.ThuTuSapXep).ToList();

                    zaloTemp.buttons = lstBtn;
                    zaloTemp.elements = lstElm;
                    return zaloTemp;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
