using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Roles.Dto
{
    public class CreateOrUpdateRoleInput
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public string Description { get; set; }

        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}
