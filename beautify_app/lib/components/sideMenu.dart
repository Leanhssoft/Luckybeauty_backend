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
      child: SizedBox(
        height: MediaQuery.of(context).size.height - 120,
        child: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                SingleChildScrollView(
                  scrollDirection: Axis.vertical,
                  child: Column(
                    children: [
                      ...buildDrawerItems(drawerMenu.items, context),
                    ],
                  ),
                ),
                SizedBox(
                  height: 120,
                  child: Padding(
                    padding: const EdgeInsets.only(
                        top: 8, bottom: 8, left: 16, right: 16),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      crossAxisAlignment: CrossAxisAlignment.end,
                      children: [
                        const Divider(),
                        const Spacer(),
                        SignOut(
                            buildContext: context,
                            item: DrawerItem(
                                title: "Đăng xuất",
                                icon: Icons.logout,
                                permission: null,
                                route: "/auth")),
                        const Spacer()
                      ],
                    ),
                  ),
                )
              ]),
        ),
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
              const EdgeInsets.only(top: 8, bottom: 8, left: 16, right: 16),
          child: ListTileMenu(
            buildContext: buildContext,
            item: item,
          )));
    } else {
      widgets.add(Padding(
        padding: const EdgeInsets.only(top: 8, bottom: 8, left: 16, right: 16),
        child: ExpansionTileMenu(buildContext: buildContext, item: item),
      ));
    }
  }
  return widgets;
}
