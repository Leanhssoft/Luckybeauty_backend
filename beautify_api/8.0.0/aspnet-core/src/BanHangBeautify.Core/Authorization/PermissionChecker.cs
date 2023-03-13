using Abp.Authorization;
using BanHangBeautify.Authorization.Roles;
using BanHangBeautify.Authorization.Users;

namespace BanHangBeautify.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
