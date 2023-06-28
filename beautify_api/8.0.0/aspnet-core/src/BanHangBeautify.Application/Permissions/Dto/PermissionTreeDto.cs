using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Permissions.Dto
{
    public class PermissionTreeDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<PermissionTreeDto>? Children { get; set; }
        public string ParentNode { get; set; }
    }
}
