﻿using Abp.Collections.Extensions;
using Abp.Dependency;
using BanHangBeautify.Net.MimeTypes;
using BanHangBeautify.Storage;
using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace BanHangBeautify.DataExporting.Excel.EpPlus
{
    public abstract class EpPlusExcelExporterBase : ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        protected EpPlusExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }

        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
        }
        protected void GetFile(string token)
        {
            _tempFileCacheManager.GetFileInfo(token);
        }
        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            _tempFileCacheManager.SetFile(file.FileToken, excelPackage.GetAsByteArray());
        }
    }
}
