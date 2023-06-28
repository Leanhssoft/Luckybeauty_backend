// ignore_for_file: public_member_api_docs, sort_constructors_first
//import 'package:beautify_app/components/sideMenu.dart';
import 'package:beautify_app/Models/PermissionAndMenu/UserPermission.dart';
import 'package:flutter/material.dart';

import 'package:beautify_app/helper/responsivesLayout.dart';
import 'package:beautify_app/widgets/large_screen.dart';
import 'package:beautify_app/widgets/small_screen.dart';
import 'package:beautify_app/widgets/top_bar.dart';

import 'Service/permissionService.dart';
import 'components/sideMenu.dart';

class SiteLayout extends StatefulWidget {
  Widget child;

  SiteLayout({
    Key? key,
    required this.child,
  }) : super(key: key);

  @override
  State<SiteLayout> createState() => _SiteLayoutState();
}

class _SiteLayoutState extends State<SiteLayout> {
  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();
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
    return Scaffold(
      key: scaffoldKey,
      appBar: TopBarNavigation(context, scaffoldKey),
      body: ResponsiveWidget(
        largeScreen: LargeScreen(
          user: user,
          child: widget.child,
        ),
        mediumScreen: LargeScreen(
          user: user,
          child: widget.child,
        ),
        smallScreen: SmallScreen(child: widget.child),
        customScreen: LargeScreen(
          user: user,
          child: widget.child,
        ),
      ),
      drawer: SideMenu(
        user: user,
      ),
    );
  }
}
