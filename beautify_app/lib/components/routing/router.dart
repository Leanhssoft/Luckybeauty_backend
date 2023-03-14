import 'package:beautify_app/components/routing/routes.dart';
import 'package:beautify_app/screens/app/account/login/LoginScreen.dart';
import 'package:beautify_app/screens/app/account/register/RegisterScreen.dart';
import 'package:beautify_app/screens/app/admin/role/role_screen.dart';
import 'package:beautify_app/screens/app/admin/tenant/TenantScreen.dart';
import 'package:beautify_app/screens/app/admin/user/user_screen.dart';
import 'package:beautify_app/screens/app/customer/customerScreen.dart';
import 'package:beautify_app/screens/app/dashboard/dashboard.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuScreen.dart';
import 'package:beautify_app/screens/app/lich_hen/calendar.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanhVienScreen.dart';
import 'package:beautify_app/widgets/AuthenWidget.dart';
import 'package:flutter/material.dart';

Route<dynamic> generateRoute(RouteSettings settings) {
  switch (settings.name) {
    case overviewPageRoute:
      return _getPageRoute(const DashBoardScreen());
    case appointmentPageRoute:
      return _getPageRoute(const CalendarView());
    case dichVuPageRoute:
      return _getPageRoute(const DichVuScreen());
    case nhanVienPageRoute:
      return _getPageRoute(const NhanVienScreen());
    case customerPageRoute:
      return _getPageRoute(const CustomerScreen());
    case rolePageRoute:
      return _getPageRoute(const RoleScreen());
    case userPageRoute:
      return _getPageRoute(const UserScreen());
    case tenantPageRoute:
      return _getPageRoute(const TenantScreen());
    case baoCaoPageDisplayName:
      return _getPageRoute(const DashBoardScreen());
    case settingsPageRoute:
      return _getPageRoute(const DashBoardScreen());
    default:
      return _getPageRoute(const DashBoardScreen());
  }
}

PageRoute _getPageRoute(Widget child) {
  return MaterialPageRoute(builder: (context) => child);
}
