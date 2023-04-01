// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/screens/app/account/login/LoginScreen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:month_year_picker/month_year_picker.dart';
import 'package:syncfusion_localizations/syncfusion_localizations.dart';

import 'package:beautify_app/screens/app/account/register/RegisterScreen.dart';
import 'package:beautify_app/screens/app/admin/role/rolePage.dart';
import 'package:beautify_app/screens/app/admin/tenant/TenantScreen.dart';
import 'package:beautify_app/screens/app/admin/user/userPage.dart';
import 'package:beautify_app/screens/app/customer/customerScreen.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuPage.dart';
import 'package:beautify_app/screens/app/lich_hen/lichHenScreen.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanhVienScreen.dart';
import 'package:beautify_app/screens/main/HomeScreen.dart';
import 'package:beautify_app/widgets/AuthenWidget.dart';
import 'package:timezone/data/latest.dart' as tz;
import 'routing/routes.dart';

final storage = SessionManager();

bool isLoggedIn = false;

void main() async {
  // Kiểm tra xem người dùng đã đăng nhập hay chưa
  final token = await storage.get('accessToken');
  if (token != null) {
    isLoggedIn = true;
  }
  tz.initializeTimeZones();
  runApp(MyApp(
    token: token,
  ));
}

class MyApp extends StatelessWidget {
  final String? token;
  const MyApp({
    Key? key,
    this.token,
  }) : super(key: key);
  // This widget is the root of your application.

  Route<dynamic> _onGenerateRoute(RouteSettings settings) {
    if (token != null && token!.isNotEmpty) {
      switch (settings.name) {
        case overviewPageRoute:
          return MaterialPageRoute(builder: (context) => const HomeScreen());
        case nhanVienPageRoute:
          return MaterialPageRoute(
              builder: (context) => const NhanVienScreen());
        case userPageRoute:
          return MaterialPageRoute(builder: (context) => const UserPage());
        case rolePageRoute:
          return MaterialPageRoute(builder: (context) => const RolePage());
        case tenantPageRoute:
          return MaterialPageRoute(builder: (context) => const TenantScreen());
        case appointmentPageRoute:
          return MaterialPageRoute(
              builder: (context) => const CalendarWorkingPage());
        case customerPageRoute:
          return MaterialPageRoute(
              builder: (context) => const KhachHangScreen());
        case settingsPageRoute:
          return MaterialPageRoute(
              builder: (context) => const KhachHangScreen());
        case baoCaoPageRoute:
          return MaterialPageRoute(builder: (context) => const HomeScreen());
        case dichVuPageRoute:
          return MaterialPageRoute(builder: (context) => const DichVuPage());
        default:
          return MaterialPageRoute(builder: (context) => const HomeScreen());
      }
    } else {
      return MaterialPageRoute(builder: (context) => const AuthenWidget());
    }
  }

  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      initialRoute: isLoggedIn ? overviewPageRoute : authenticationPageRoute,
      routes: {
        rootRoute: (context) => const AuthenWidget(),
        '/home': (context) => const HomeScreen(),
        '/auth': (context) => const AuthenWidget(),
        registerPageRoute: (context) => const RegisterScreen(),
      },
      onGenerateRoute: _onGenerateRoute,
      debugShowCheckedModeBanner: false,
      color: Colors.white,
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        SfGlobalLocalizations.delegate,
        MonthYearPickerLocalizations.delegate,
      ],
      supportedLocales: const [
        Locale('vi', 'VN'),
        Locale('en'),
        Locale('ja'),
        Locale('en', 'US'), // American English
        Locale('he', 'IL')
      ],
      locale: const Locale('vi', 'VN'),
      theme: ThemeData(
        scaffoldBackgroundColor: Colors.white,
        textTheme: GoogleFonts.mulishTextTheme(Theme.of(context).textTheme)
            .apply(bodyColor: Colors.black),
        pageTransitionsTheme: const PageTransitionsTheme(builders: {
          TargetPlatform.iOS: FadeUpwardsPageTransitionsBuilder(),
          TargetPlatform.android: FadeUpwardsPageTransitionsBuilder(),
        }),
        primarySwatch: Colors.blue,
      ),
    );
  }
}
