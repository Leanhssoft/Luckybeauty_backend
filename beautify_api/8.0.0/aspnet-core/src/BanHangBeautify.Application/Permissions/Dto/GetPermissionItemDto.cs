using System.Collections.Generic;

namespace BanHangBeautify.Permissions.Dto
{
    public class GetPermissionDto
    {
        public string Name { set; get; }
        public List<string> Permissions { set; get; }
    }

}
