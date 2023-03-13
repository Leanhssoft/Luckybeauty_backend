import 'package:beautify_app/layout.dart';
import 'package:flutter/material.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';

import '../Models/PermissionAndMenu/UserPermission.dart';
import '../screens/app/account/login/LoginScreen.dart';
import '../screens/main/HomeScreen.dart';

class AuthenWidget extends StatefulWidget {
  const AuthenWidget({super.key});

  @override
  State<AuthenWidget> createState() => _AuthenWidgetState();
}

class _AuthenWidgetState extends State<AuthenWidget> {
  String token = "";
  late UserPermission user;
  @override
  void initState() {
    loadToken();
    super.initState();
  }

  void loadToken() async {
    token = await SessionManager().get("accessToken");
  }

  void loadPermissionUser() {
    user = UserPermission(user: "admin", permissions: [
      "Pages.Branch",
      "Pages.Branch.Create",
      //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)
      "Pages",
      "Pages.Users.Activation",
      "Pages.Tenants",
      "Pages.Tenants.Create",
      "Pages.Tenants.Edit",
      "Pages.Tenants.ChangeFeatures",
      "Pages.Tenants.Delete",
      "Pages.Administration",
      "Pages.Administration.Roles",
      "Pages.Administration.Roles.Create",
      "Pages.Administration.Roles.Edit",
      "Pages.Administration.Roles.Delete",
      "Pages.Administration.Users",
      "Pages.Administration.Users.Create",
      "Pages.Administration.Users.Edit",
      "Pages.Administration.Users.Delete",
      "Pages.Administration.Users.ChangePermissions",
      "Pages.Administration.Users.Impersonation",
      "Pages.Administration.Users.Unlock",
      "Pages.Administration.AuditLogs"
    ]);
  }

  @override
  Widget build(BuildContext context) {
    if (token == "" || token.isEmpty) {
      return const LoginScreen();
    }
    return SiteLayout(route:const HomeScreen(),);
  }
}
