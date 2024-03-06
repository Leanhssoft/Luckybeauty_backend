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
        /// <param name="indexRowSum">Nếu muốn in thêm dòng tổng cộng, truyền index của row tổng (để copy công thức)</param>
        /// <returns></returns>
        public FileDto WriteToExcel<T>(string fileName, string fileTemplate, List<T> listData, int startRow = 4,
            List<Excel_CellData> lstDataCell = null, int? indexRowSum = 0)
        {
            var path = Path.Combine(_env.WebRootPath, @"ExcelTemplate\", fileTemplate);

            string fileNameNew = fileName + DateTime.Now.Ticks.ToString() + ".xlsx";

            var file = new FileDto(fileNameNew, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            using (var excelPack = new ExcelPackage(new FileInfo(path)))
            {
                var ws = excelPack.Workbook.Worksheets[0];

                int sourceRow = indexRowSum ?? 0; // index của dòng tổng (trong template)
                if (sourceRow > 0)
                {
                    int columnCount = ws.Dimension.End.Column;  // Số cột trong bảng tính
                    // lấy toàn bộ dòng tổng (dòng sourceRow, cột 1 đến cột columnCount)
                    ExcelRange rowSumTemp = ws.Cells[sourceRow, 1, sourceRow, columnCount];
                    // tạo dòng tổng mới (cộng thêm 1 vì lát sẽ xóa 1 dòng)
                    int indexLastRow = listData.Count + startRow + 1;
                    ExcelRange lastRowData = ws.Cells[indexLastRow, 1, indexLastRow, columnCount];
                    // Sao chép nội dung từ dòng tổng (temp) sang dòng tổng mới
                    rowSumTemp.Copy(lastRowData);
                    ws.DeleteRow(sourceRow);// xóa dòng cũ đi (vì nếu data bị ăn theo định dạng của dòng tổng)
                }

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
