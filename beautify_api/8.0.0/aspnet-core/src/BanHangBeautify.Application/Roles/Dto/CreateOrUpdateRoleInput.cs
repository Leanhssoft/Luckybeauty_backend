using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Roles.Dto
{
    public class CreateOrUpdateRoleInput
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Tên vai trò không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tên vai trò hiển thị không được để trống")]
        public string DisplayName { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Quyền không được để trống")]
        public List<string> GrantedPermissions { get; set; }
    }
}
