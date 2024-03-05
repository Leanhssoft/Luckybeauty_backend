using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.LoadFunctions.Params;
using System;
using System.Collections.Generic;
using System.IO;
using static BanHangBeautify.AppCommon.CommonClass;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    internal class ExcelBase : EpPlusExcelExporterBase, IExcelBase
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="fileTemplate">path file template</param>
        /// <param name="listData"></param>
        /// <param name="startRow"></param>
        /// <param name="lstDataCell"></param>
        /// <returns></returns>
        public FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 4,
            List<Excel_CellData> lstDataCell = null)
        {
            var path = Path.Combine(_env.WebRootPath, @"ExcelTemplate\", fileTemplate);

            string fileNameNew = fileName + DateTime.Now.Ticks.ToString() + ".xlsx";

            var file = new FileDto(fileNameNew, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            using (var excelPack = new ExcelPackage(new FileInfo(path)))
            {
                var ws = excelPack.Workbook.Worksheets[0];
                if (lstDataCell != null)
                {
                    foreach (var item in lstDataCell)
                    {
                        ws.Cells[item.RowIndex, item.ColumnIndex].Value = item.CellValue;
                    }
                }
                ws.Cells[startRow, 1].LoadFromCollection(listData);
                Save(excelPack, file);
            }
            return file;
        }
    }
}
