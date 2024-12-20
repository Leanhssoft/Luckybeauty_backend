﻿using Abp.Application.Services.Dto;
using BanHangBeautify.AppCommon;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BanHangBeautify.KhachHang.KhachHang.Repository
{
    public interface IKhachHangRespository
    {
        Task<double> GetMaxNumber_ofMaKhachHang(int idLoaiDoiTuong);
        Task ImportDanhMucKhachHang(int? tenantId, long? userId, ImportExcelKhachHangDto data);
        Task<PagedResultDto<KhachHangView>> GetKhachHang_noBooking(PagedKhachHangResultRequestDto input, int? tenantId);
        Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input, int tenantId);
        Task<List<KhachHangView>> JqAutoCustomer(PagedKhachHangResultRequestDto input, int? tenantId);
        Task<PagedResultDto<LichSuHoaDonDto>> LichSuGiaoDich(Guid idKhachHang, int tenantId, PagedRequestDto input);
        Task<PagedResultDto<LichSuDatLichDto>> LichSuDatLich(Guid idKhachHang, int tenantId, PagedRequestDto input);
        Task<CustomerDetail_FullInfor> GetCustomerDetail_FullInfor(Guid idKhachHang);
        Task<List<HoatDongKhachHang>> GetNhatKyHoatDong_ofKhachHang(Guid idKhachHang);
    }
}
