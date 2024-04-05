using Abp;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Common.Dto
{
    public class FindUsersInput: PagedResultRequestDto
    {
        public string Keyword {  get; set; }    
        public int? TenantId { get; set; }

        public bool ExcludeCurrentUser { get; set; }

    }
}
