import 'package:flutter/material.dart';

import '../../Models/PermissionAndMenu/DrawerItem.dart';
import '../../Models/PermissionAndMenu/UserPermission.dart';

const rootRoute = "/";

const overviewPageDisplayName = "Overview";
const overviewPageRoute = "/overview";

const appointmentPageDisplayName = "Lịch hẹn";
const appointmentPageRoute = '/lich-hens';

const dichVuPageDisplayName = "Dịch vụ";
const dichVuPageRoute = "/dich-vus";

const nhanVienPageDisplayName = "Nhân viên";
const nhanVienPageRoute = "/nhan-viens";

const customerPageDisplayName = "Khách hàng";
const customerPageRoute = "/khach-hangs";

const adminPageDisplayName = "Quản trị";
const rolePageDisplayName = "Quyền";
const rolePageRoute = "/roles";
const userPageDisplayName = "Tài khoản";
const userPageRoute = "/users";
const tenantPageDisplayName = "Tenant";
const tenantPageRoute = "/tenants";

const baoCaoPageDisplayName = "Báo cáo";
const baoCaoPageRoute = "/bao-caos";

const settingsPageDisplayName = "Settings";
const settingsPageRoute = "/settings";

const authenticationPageDisplayName = "Log out";
const authenticationPageRoute = "/auth";

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
          route: overviewPageRoute),
      DrawerItem(
          title: appointmentPageDisplayName,
          icon: Icons.calendar_month_outlined,
          permission: 'Pages',
          route: appointmentPageRoute),
      DrawerItem(
          title: dichVuPageDisplayName,
          icon: Icons.self_improvement_sharp,
          permission: 'Pages',
          route: dichVuPageRoute),
      DrawerItem(
          title: nhanVienPageDisplayName,
          icon: Icons.person_pin_circle_outlined,
          permission: 'Pages.Administration.Users',
          route: nhanVienPageRoute),
      DrawerItem(
          title: customerPageDisplayName,
          icon: Icons.people_alt_outlined,
          permission: 'Pages',
          route: customerPageRoute),
      DrawerItem(
          title: adminPageDisplayName,
          icon: Icons.admin_panel_settings,
          permission: 'Pages.Administration',
          route: '',
          children: [
            DrawerItem(
                title: rolePageDisplayName,
                icon: Icons.lock_person,
                permission: 'Pages.Administration.Roles',
                route: rolePageRoute),
            DrawerItem(
                title: userPageDisplayName,
                icon: Icons.people_alt_outlined,
                permission: 'Pages.Administration.Users',
                route: userPageRoute),
            DrawerItem(
                title: tenantPageDisplayName,
                icon: Icons.store,
                permission: 'Pages.Tenants',
                route: tenantPageRoute),
          ]),
      DrawerItem(
          title: baoCaoPageDisplayName,
          icon: Icons.bar_chart_outlined,
          permission: 'Pages',
          route: baoCaoPageRoute),
      DrawerItem(
          title: settingsPageDisplayName,
          icon: Icons.settings,
          permission: 'Pages',
          route: settingsPageRoute),
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
