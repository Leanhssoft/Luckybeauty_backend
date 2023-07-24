using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Storage;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon.Exporting
{
    public interface IQuyHoaDonExcelExporter
    {
        FileDto ExportDanhSachQuyHoaDon(List<GetAllQuyHoaDonItemDto> data);
        FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5);
    }
}
