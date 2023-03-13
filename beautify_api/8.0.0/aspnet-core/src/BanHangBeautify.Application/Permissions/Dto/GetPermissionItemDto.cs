using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BanHangBeautify.Permissions.Dto
{
    public class GetPermissionDto 
    {
        public string Name { set; get; }
        public List<string> Permissions { set; get; }
    }
  
}
