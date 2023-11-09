using BanHangBeautify.Storage;
using System.Collections.Generic;
using static BanHangBeautify.Configuration.Common.CommonClass;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    public interface IExcelBase
    {
        FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5, List<Excel_CellData> param = null);
    }
}
