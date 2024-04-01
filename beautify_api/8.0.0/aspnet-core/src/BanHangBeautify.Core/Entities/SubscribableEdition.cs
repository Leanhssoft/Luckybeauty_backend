using Abp.Application.Editions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Entities
{

    public class SubscribableEdition : Edition
    {
        public decimal? Price { get; set; } = 0;
    }
}
