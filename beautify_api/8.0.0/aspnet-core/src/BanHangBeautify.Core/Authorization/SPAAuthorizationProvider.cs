using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace BanHangBeautify.Authorization
{
    public class SPAAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ?? context.CreatePermission(PermissionNames.Pages, L("Pages"));




            //Công ty
            var congTy = pages.CreateChildPermission(PermissionNames.Pages_CongTy, L("Company"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Create, L("CreateCompany"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Edit, L("EditCompany"));
            congTy.CreateChildPermission(PermissionNames.Pages_CongTy_Delete, L("DeleteCompany"));

            //Chi nhánh
            var chiNhanh = pages.CreateChildPermission(PermissionNames.Pages_ChiNhanh, L("Branch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Create, L("CreateBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Edit, L("EditBranch"));
            chiNhanh.CreateChildPermission(PermissionNames.Pages_ChiNhanh_Delete, L("DeleteBranch"));


            //adminsitrantion
            var administration = pages.CreateChildPermission(PermissionNames.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(PermissionNames.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(PermissionNames.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(PermissionNames.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));

            administration.CreateChildPermission(PermissionNames.Pages_Administration_AuditLogs, L("AuditLogs"));

            //HOST-SPECIFIC PERMISSIONS

            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SPAConsts.LocalizationSourceName);
        }
    }
}
