import 'package:beautify_app/controllers/naviagation_controller.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/account/login/LoginScreen.dart';
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

import 'components/routing/routes.dart';
import 'controllers/menu_controller.dart';

void main() {
  Get.put(MenuBeautyController());
  Get.put(NavigationController());
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      initialRoute: '/auth',
        unknownRoute: GetPage(name: '/not-found', page: () =>const LoginScreen(), transition: Transition.fadeIn),
        getPages: [
        GetPage(name: rootRoute, page: () {
          return SiteLayout();
        }),
        GetPage(name: authenticationPageRoute, page: () =>const AuthenWidget()),
        GetPage(name: registerPageRoute, page: () =>const RegisterScreen()),
      ],
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
    );
  }
}
