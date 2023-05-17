using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto
{
    public class QuyHoaDonChiTietMapProfile:Profile
    {
        public QuyHoaDonChiTietMapProfile()
        {
            CreateMap<CreateOrEditQuyHoaDonCTDto, QuyHoaDon_ChiTiet>().ReverseMap();
            CreateMap<QuyHoaDon_ChiTiet>
        }
    }
}
