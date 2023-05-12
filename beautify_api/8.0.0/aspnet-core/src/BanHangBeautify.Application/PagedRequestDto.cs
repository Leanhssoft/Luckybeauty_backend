using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify
{
    public class PagedRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
