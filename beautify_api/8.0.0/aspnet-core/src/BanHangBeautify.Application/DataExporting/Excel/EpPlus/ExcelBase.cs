using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Common;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using System.Data;
using NPOI.HPSF;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using BanHangBeautify.Quy.DM_QuyHoaDon.Exporting;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    internal class ExcelBase: EpPlusExcelExporterBase, IExcelBase
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IHostingEnvironment _env;
        public ExcelBase(
            ITempFileCacheManager tempFileCacheManager,
            ITimeZoneConverter timeZoneConverter,
            IHostingEnvironment env,
            IAbpSession abpSession) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _env = env;
        }

        public FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 5)
        {
            var path = Path.Combine(_env.WebRootPath, $"ExcelTemplate", string.Format(fileTemplate));// string.Format(fileTemplate) = $"fileName.xlsx"
            string fileNameNew = fileName + DateTime.Now.Ticks.ToString() + ".xlsx";

            var file = new FileDto(fileNameNew, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            using (var excelPack = new ExcelPackage(new FileInfo(path)))
            {
                var ws = excelPack.Workbook.Worksheets[0];
                ws.Cells[startRow, 1].LoadFromCollection(listData);
                Save(excelPack, file);
            }
            return file;
        }
    }
}
