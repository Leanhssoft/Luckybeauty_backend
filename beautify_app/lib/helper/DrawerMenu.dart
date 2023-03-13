import 'package:flutter/material.dart';

import '../Models/PermissionAndMenu/DrawerItem.dart';
import '../Models/PermissionAndMenu/DrawerItem.dart';
import '../Models/PermissionAndMenu/UserPermission.dart';

class DrawerMenu {
  final UserPermission user;

  DrawerMenu(this.user);

  List<DrawerItem> get items {
    final allowedPermissions = user.permissions;

    final List<DrawerItem> allItems = [
      DrawerItem(
          title: 'Trang chủ',
          icon: Icons.home_rounded,
          permission: 'Pages',
          route: '/dashboard'),
      DrawerItem(
          title: 'Lịch hẹn',
          icon: Icons.calendar_month_outlined,
          permission: 'Pages',
          route: '/appointments'),
      DrawerItem(
          title: 'Bán hàng',
          icon: Icons.storefront_outlined,
          permission: 'Pages',
          route: ''),
      DrawerItem(
          title: 'Dịch vụ',
          icon: Icons.self_improvement_sharp,
          permission: 'Pages',
          route: '/dichvus'),
      DrawerItem(
          title: 'Nhân viên',
          icon: Icons.person_pin_circle_outlined,
          permission: 'Pages.Administration.Users',
          route: ''),
      DrawerItem(
          title: 'Khách hàng',
          icon: Icons.people_alt_outlined,
          permission: 'Pages',
          route: ''),
      DrawerItem(
          title: "Quản trị",
          icon: Icons.admin_panel_settings,
          permission: 'Pages.Administration',
          route: '',
          children: [
            DrawerItem(
                title: 'Role',
                icon: Icons.lock_person,
                permission: 'Pages.Administration.Roles',
                route: '/role'),
            DrawerItem(
                title: 'User',
                icon: Icons.people_alt_outlined,
                permission: 'Pages.Administration.Users',
                route: '/user'),
            DrawerItem(
                title: 'Tenant',
                icon: Icons.store,
                permission: 'Pages.Tenants',
                route: '/tenant'),
          ]),
      DrawerItem(
          title: 'Báo cáo',
          icon: Icons.bar_chart_outlined,
          permission: 'Pages',
          route: ''),
      DrawerItem(
          title: 'Settings',
          icon: Icons.settings,
          permission: 'Pages',
          route: ''),
    ];

    // filter items based on user permissions
    final filteredItems = allItems
        .where((item) => allowedPermissions.contains(item.permission))
        .toList();

    // recursively filter children of items with children
    return filteredItems.map((item) {
      if (item.children != null) {
        final List<DrawerItem> filteredChildren = item.children!
            .where((child) => allowedPermissions.contains(child.permission))
            .toList();

        return item.copyWith(children: filteredChildren);
      }

      return item;
    }).toList();
  }
}
