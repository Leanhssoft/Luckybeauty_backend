using BanHangBeautify.Authorization.Users;
using System.Security.Claims;

namespace BanHangBeautify.Authorization.Impersonation.Dto
{
    public class UserAndIdentity
    {
        public User User { get; set; }

        public ClaimsIdentity Identity { get; set; }

        public UserAndIdentity(User user, ClaimsIdentity identity)
        {
            User = user;
            Identity = identity;
        }
    }
}
