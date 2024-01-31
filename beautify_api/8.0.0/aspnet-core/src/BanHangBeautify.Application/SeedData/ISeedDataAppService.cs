using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SeedData
{
    public interface ISeedDataAppService
    {
        void InnitData(int? tenantId);
    }
}
