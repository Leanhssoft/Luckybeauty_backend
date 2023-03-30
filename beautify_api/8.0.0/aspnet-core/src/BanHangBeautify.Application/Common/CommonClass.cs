using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Common
{
    public class CommonClass
    {
        public class ParamSearch
        {
            public string TextSearch { get; set; } = string.Empty;
            public int? CurrentPage { get; set; } = 0;
            public int? PageSize { get; set; } = 10;
            public string ColumnSort { get; set; } = "CreationTime";
            public string TypeSort { get; set; } = "DESC";
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
        }

        public class MaxCodeDto
        {
            public string FirstStr { get; set; } = string.Empty;
            public float? MaxVal { get; set; } = 1;
        }
    }
}
