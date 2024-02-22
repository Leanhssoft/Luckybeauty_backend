using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Editions.Dto
{
    public class EditionListDto:EntityDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
        public  decimal? Price { get; set; }
    }
}
