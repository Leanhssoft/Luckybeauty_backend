using BanHangBeautify.Storage;
using System.Collections.Generic;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    public interface IExcelBase
    {
        FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5);
    }
}
