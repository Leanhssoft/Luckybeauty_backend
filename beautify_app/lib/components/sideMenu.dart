import 'package:beautify_app/Models/PermissionAndMenu/UserPermission.dart';
import 'package:beautify_app/Service/permissionService.dart';
import 'package:beautify_app/components/ExpansionTileMenu.dart';
import 'package:beautify_app/components/SignOut.dart';
import 'package:flutter/material.dart';

import '../Models/PermissionAndMenu/DrawerItem.dart';
import '../helper/DrawerMenu.dart';
import 'ListTitleMenu.dart';

class SideMenu extends StatefulWidget {
  const SideMenu({super.key});

  @override
  State<SideMenu> createState() => _SideMenuState();
}

class _SideMenuState extends State<SideMenu> {
  UserPermission user = UserPermission(user: '', permissions: []);

  @override
  void initState() {
    super.initState();
    getUserPermissions();
  }

  Future<void> getUserPermissions() async {
    final user = await UserPermissionServices().getUserPermission();
    setState(() {
      this.user = user;
    });
  }

  @override
  Widget build(BuildContext context) {
    final drawerMenu = DrawerMenu(user);
    return Drawer(
      backgroundColor: const Color(0xFFFFFFFF),
      child: SingleChildScrollView(
        child:
          Column(mainAxisAlignment: MainAxisAlignment.spaceBetween, children: [
          Column(
            children: [
              ...buildDrawerItems(drawerMenu.items, context),
            ],
          ),
          Padding(
            padding:
                const EdgeInsets.only(top: 8, bottom: 8, left: 16, right: 16),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                 const Divider(),
                SignOut(
                    buildContext: context,
                    item: DrawerItem(
                        title: "Đăng xuất",
                        icon: Icons.logout,
                        permission: null,
                        route: "/login")),
              ],
            ),
          )
        ]),
      ),
    );
  }
}

List<Widget> buildDrawerItems(
    List<DrawerItem> items, BuildContext buildContext) {
  List<Widget> widgets = [];
  for (var item in items) {
    if (item.children == null) {
      widgets.add(Padding(
          padding:
              const EdgeInsets.only(top: 4, bottom: 4, left: 16, right: 16),
          child: ListTileMenu(
            buildContext: buildContext,
            item: item,
          )));
    } else {
      widgets.add(Padding(
        padding: const EdgeInsets.only(top: 4, bottom: 4, left: 16, right: 16),
        child: ExpansionTileMenu(buildContext: buildContext, item: item),
      ));
    }
  }
  return widgets;
}
