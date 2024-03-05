using BanHangBeautify.NhanSu.CaLamViec.Dto;
using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.NhanSu.CaLamViec.Exporting
{
    public interface ICaLamViecExcelExporter
    {
        FileDto ExportDanhSachCaLamViec(List<CaLamViecDto> data);
    }
}
