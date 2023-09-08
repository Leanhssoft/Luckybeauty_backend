﻿using System;
using System.Collections.Generic;

namespace BanHangBeautify.Configuration.Common
{
    public class CommonClass
    {
        public class ParamSearch
        {
            public int? TenantId { get; set; }
            public List<string> IdChiNhanhs { get; set; }
            public string TextSearch { get; set; } = string.Empty;
            public int? CurrentPage { get; set; } = 0;
            public int? PageSize { get; set; } = 10;
            public string ColumnSort { get; set; } = "CreationTime";
            public string TypeSort { get; set; } = "DESC";
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
        }

        public class MaxCodeDto
        {
            public string FirstStr { get; set; } = string.Empty;
            public float? MaxVal { get; set; } = 1;
        }
        public class ExcelErrorDto
        {
            public int RowNumber { get; set; }
            public string TenTruongDuLieu { get; set; }
            public string GiaTriDuLieu { get; set; }
            public string DienGiai { get; set; }
            public int LoaiErr { get; set; }//  -1. exception, 0. lỗi định dạng file, 1. lỗi dữ liệu, 2.lỗi import không thành công
        }
    }
}
