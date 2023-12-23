using BanHangBeautify.Storage;
using System.Collections.Generic;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    public interface IExcelBase
    {
        FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 4, List<Excel_CellData> param = null);
    }
}
