using BanHangBeautify.NhanSu.CaLamViec.Dto;
using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.CaLamViec.Exporting
{
    public interface ICaLamViecExcelExporter
    {
        FileDto ExportDanhSachCaLamViec(List<CaLamViecDto> data);
    }
}
