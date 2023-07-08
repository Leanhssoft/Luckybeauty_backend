using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa.Exporting
{
    public interface IHangHoaExcelExporter
    {
        FileDto ExportHangHoaToExcel(List<HangHoaDto> data);
    }
}
