using Abp.Authorization.Users;
using Abp.Extensions;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanHangBeautify.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
        [Required(AllowEmptyStrings = true)]
        public override string EmailAddress { get; set; }
        public Guid? NhanSuId { get; set; }
        public Guid? IdChiNhanhMacDinh { get; set; }
        [ForeignKey("IdChiNhanhMacDinh")]
        public DM_ChiNhanh DM_ChiNhanh { get; set; }

        public bool IsAdmin { set; get; }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                IsAdmin = true,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();
            return user;
        }
    }
}
