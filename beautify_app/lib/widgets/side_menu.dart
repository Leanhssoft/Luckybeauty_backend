import 'package:beautify_app/constants/styles.dart';
import 'package:flutter/material.dart';
import '../Models/PermissionAndMenu/DrawerItem.dart';
import '../Models/PermissionAndMenu/UserPermission.dart';
import '../Service/permissionService.dart';
import '../components/ExpansionTileMenu.dart';
import '../components/ListTitleMenu.dart';
import '../helper/DrawerMenu.dart';
import '../helper/responsivesLayout.dart';

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
    double _width = MediaQuery.of(context).size.width;
    final drawerMenu = DrawerMenu(user);
    return Container(
      color: light,
      child: ListView(
        children: [
          if (ResponsiveWidget.isSmallScreen(context))
            Column(
              mainAxisSize: MainAxisSize.min,
              children: [...buildDrawerItems(drawerMenu.items, context)],
            ),
        ],
      ),
    );
  }
}

// List<Widget> buildDrawerItems(
//     List<DrawerItem> items, BuildContext buildContext) {
//   List<Widget> widgets = [];
//   for (var item in items) {
//     widgets.add(SideMenuItem(
//       itemName: item.route.toString(),
//       onTab: () {
//         if (item.route.toString() == authenticationPageRoute) {}
//         if (!menuController.isActive(item.route.toString())) {
//           menuController.changeActiveItemTo(item.route.toString());
//           if (ResponsiveWidget.isSmallScreen(buildContext)) ;
//           Get.back();
//         }
//       },
//     ));
//   }
//   return widgets;
// }

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
