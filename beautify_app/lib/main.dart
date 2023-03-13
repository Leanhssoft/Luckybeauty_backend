import 'package:beautify_app/controllers/naviagation_controller.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/account/register/RegisterScreen.dart';
import 'package:beautify_app/screens/app/admin/role/role_screen.dart';
import 'package:beautify_app/screens/app/admin/tenant/TenantScreen.dart';
import 'package:beautify_app/screens/app/admin/user/user_screen.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuScreen.dart';
import 'package:beautify_app/screens/app/lich_hen/lichHenScreen.dart';
import 'package:beautify_app/screens/main/HomeScreen.dart';
import 'package:beautify_app/widgets/AuthenWidget.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:syncfusion_localizations/syncfusion_localizations.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:month_year_picker/month_year_picker.dart';

void main() {
  // Get.put(MenuController());
  // Get.put(NavigationController());
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
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
      initialRoute: '/login',
      routes: {
        '/login': (context) => const AuthenWidget(),
        '/home': (context) => SiteLayout(route:const HomeScreen()),
        '/register': (context) =>SiteLayout(route: const RegisterScreen()),
        '/tenant': (context) => SiteLayout(route: const TenantScreen()),
        '/dashboard': (context) => SiteLayout(route: const HomeScreen()),
        '/user': (context) => SiteLayout(route: const UserScreen()),
        '/role': (context) => SiteLayout(route: const RoleScreen()),
        '/appointments': (context) =>SiteLayout(route: const CalendarWorkingPage()) ,
        '/dichvus': (context) => SiteLayout(route: const DichVuScreen()),
      },
      onGenerateRoute: (RouteSettings settings) {
        switch (settings.name) {
          case '/login':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/home':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/register':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/tenant':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/dashboard':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/user':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/role':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/appointments':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          case '/dichvus':
            MaterialPageRoute(builder: (context) =>const AuthenWidget());
            break;
          default:
            return MaterialPageRoute(builder: (context) =>const AuthenWidget());
        }
      },
      onUnknownRoute: (RouteSettings settings) {
        return MaterialPageRoute(builder: (context) => const AuthenWidget());
      },
    );
  }
}
