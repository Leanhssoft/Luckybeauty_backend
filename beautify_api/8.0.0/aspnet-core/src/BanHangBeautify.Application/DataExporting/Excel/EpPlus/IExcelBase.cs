using BanHangBeautify.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    public interface IExcelBase
    {
        FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5);
    }
}
