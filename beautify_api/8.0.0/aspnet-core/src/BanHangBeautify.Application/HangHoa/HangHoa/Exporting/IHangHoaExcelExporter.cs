using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.HangHoa.HangHoa.Exporting
{
    public interface IHangHoaExcelExporter
    {
        FileDto ExportHangHoaToExcel(List<HangHoaDto> data);
    }
}
