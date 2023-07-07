using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Exporting
{
    public interface IQuyHoaDonExcelExporter
    {
        FileDto ExportDanhSachQuyHoaDon(List<GetAllQuyHoaDonItemDto> data);
    }
}
