using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Users.Dto
{
    public class UserIdentifierDto
    {
        public int? TenantId { get; protected set; }
        public long UserId { get; protected set; }
    }
}
