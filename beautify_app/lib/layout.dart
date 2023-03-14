// ignore_for_file: public_member_api_docs, sort_constructors_first
//import 'package:beautify_app/components/sideMenu.dart';
import 'package:flutter/material.dart';
import 'package:beautify_app/helper/responsivesLayout.dart';
import 'package:beautify_app/widgets/large_screen.dart';
import 'package:beautify_app/widgets/small_screen.dart';
import 'package:beautify_app/widgets/top_bar.dart';

import 'components/sideMenu.dart';

class SiteLayout extends StatelessWidget {
  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();
   SiteLayout({
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      key: scaffoldKey,
      appBar: TopBarNavigation(context, scaffoldKey),
      body:const ResponsiveWidget(
        largeScreen: LargeScreen(),
        mediumScreen: LargeScreen(),
        smallScreen: SmallScreen(),
        customScreen: LargeScreen(),
      ),
      drawer: const SideMenu(),
    );
  }
}
