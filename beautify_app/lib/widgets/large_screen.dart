// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';

import 'package:beautify_app/Models/PermissionAndMenu/UserPermission.dart';

import '../components/sideMenu.dart';

class LargeScreen extends StatefulWidget {
  UserPermission user;
  Widget child;
  LargeScreen({
    Key? key,
    required this.user,
    required this.child,
  }) : super(key: key);

  @override
  State<LargeScreen> createState() => _LargeScreenState();
}

class _LargeScreenState extends State<LargeScreen> {
  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(child: SideMenu(user: widget.user,)),
        Expanded(flex: 5, child: widget.child)
      ],
    );
  }
}
