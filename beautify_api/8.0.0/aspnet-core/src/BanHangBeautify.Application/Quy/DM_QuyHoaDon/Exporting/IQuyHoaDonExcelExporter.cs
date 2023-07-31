using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Exporting
{
    public interface IQuyHoaDonExcelExporter
    {
        FileDto ExportDanhSachQuyHoaDon(List<GetAllQuyHoaDonItemDto> data);
        FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5);
    }
}
