﻿using Abp.Application.Services.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.LoaiKhach.Dto;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.LoaiKhach
{
    public interface ILoaiKhachAppService
    {
        Task<LoaiKhachDto> CreateLoaiKhach(CreateOrEditLoaiKhachDto dto);
        Task<LoaiKhachDto> Delete(int id);
        Task<LoaiKhachDto> EditLoaiKhach(CreateOrEditLoaiKhachDto dto);
        Task<ListResultDto<DM_LoaiKhach>> GetAll(PagedLoaiKhachResultRequestDto input);
        Task<DM_LoaiKhach> GetLoaiKhachDetail(int Id);
    }
}