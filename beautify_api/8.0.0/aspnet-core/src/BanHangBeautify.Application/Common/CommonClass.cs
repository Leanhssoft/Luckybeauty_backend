using Abp.Application.Services.Dto;
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
        public class PagedResultDtoAction<T> : PagedResultDto<T>
        {
            public bool? res { get; set; } = true;
            public string mes { get; set; } = string.Empty;

            public PagedResultDtoAction()
            {
            }
        }
        public class ResultItemDtoAction<T> 
        {
            public bool? res { get; set; } = true;
            public string mes { get; set; } = string.Empty;
            public T Item { get; set; }

            public ResultItemDtoAction()
            {
            }
        }
    }
}
