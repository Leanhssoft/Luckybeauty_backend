﻿using Abp.Application.Services.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository
{
    public interface IChietKhauHoaDonRepository
    {
        public Task<PagedResultDto<ChietKhauHoaDonItemDto>> GetAll(PagedRequestDto input,int tenatId,Guid? idChiNhanh);
    }
}