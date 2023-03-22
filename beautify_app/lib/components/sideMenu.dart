// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';

import 'package:beautify_app/Models/PermissionAndMenu/UserPermission.dart';
import 'package:beautify_app/Service/permissionService.dart';
import 'package:beautify_app/components/ExpansionTileMenu.dart';
import 'package:beautify_app/components/SignOut.dart';
import 'package:beautify_app/routing/routes.dart';

import '../Models/PermissionAndMenu/DrawerItem.dart';
import '../helper/DrawerMenu.dart';
import 'ListTitleMenu.dart';

class SideMenu extends StatefulWidget {
  UserPermission user;
   SideMenu({
    Key? key,
    required this.user,
  }) : super(key: key);

  @override
  State<SideMenu> createState() => _SideMenuState();
}

class _SideMenuState extends State<SideMenu> {
  @override
  Widget build(BuildContext context) {
    final drawerMenu = DrawerMenu(widget.user);
    return Drawer(
      backgroundColor: const Color(0xFFFFFFFF),
      child: SingleChildScrollView(
        child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Column(
                children: [
                  ...buildDrawerItems(drawerMenu.items, context),
                ],
              ),
              Padding(
                padding: const EdgeInsets.only(
                    top: 8, bottom: 8, left: 16, right: 16),
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
                            route: "/auth")),
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
