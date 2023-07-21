using AutoMapper;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Dto
{
    public class QuyHoaDonMapProfile:Profile
    {
        public QuyHoaDonMapProfile()
        {
            CreateMap<CreateOrEditQuyHoaDonDto,QuyHoaDon>().ReverseMap();
            CreateMap<QuyHoaDonDto,QuyHoaDon>().ReverseMap();
            CreateMap<CreateOrEditQuyHoaDonDto,QuyHoaDon>().ReverseMap();
            CreateMap<GetAllQuyHoaDonItemDto, ExcelSoQuyDto>().ReverseMap();
        }
    }
}
