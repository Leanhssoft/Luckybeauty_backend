import 'package:beautify_app/screens/app/account/register/RegisterScreen.dart';
import 'package:beautify_app/screens/app/admin/role/role_screen.dart';
import 'package:beautify_app/screens/app/admin/tenant/TenantScreen.dart';
import 'package:beautify_app/screens/app/admin/user/user_screen.dart';
import 'package:beautify_app/screens/app/customer/customerScreen.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuPage.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuScreen.dart';
import 'package:beautify_app/screens/app/lich_hen/lichHenScreen.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanhVienScreen.dart';
import 'package:beautify_app/screens/main/HomeScreen.dart';
import 'package:beautify_app/widgets/AuthenWidget.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:syncfusion_localizations/syncfusion_localizations.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:month_year_picker/month_year_picker.dart';
import 'routing/routes.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      initialRoute: '/auth',
      routes: {
        rootRoute: (context) => const AuthenWidget(),
        "/home": (context) => const HomeScreen(),
        overviewPageRoute: (context) => const HomeScreen(),
        nhanVienPageRoute: (context) => const NhanVienScreen(),
        userPageRoute: (context) => const UserScreen(),
        rolePageRoute: (context) => const RoleScreen(),
        tenantPageRoute: (context) => const TenantScreen(),
        appointmentPageRoute: (context) => const CalendarWorkingPage(),
        customerPageRoute: (context) => const KhachHangScreen(),
        settingsPageRoute: (context) => const HomeScreen(),
        baoCaoPageRoute: (context) => const HomeScreen(),

        '/auth': (context) => const AuthenWidget(),
        registerPageRoute: (context) => const RegisterScreen(),
        dichVuPageRoute: (context) => const DichVuPage(),
      },
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
