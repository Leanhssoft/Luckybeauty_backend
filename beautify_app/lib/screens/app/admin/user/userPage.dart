// ignore_for_file: prefer_const_constructors

import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/admin/user/userHeader.dart';
import 'package:beautify_app/screens/app/admin/user/userTable.dart';
import 'package:flutter/material.dart';

class UserPage extends StatefulWidget {
  const UserPage({super.key});

  @override
  State<UserPage> createState() => _UserPageScreenState();
}

class _UserPageScreenState extends State<UserPage> {
  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: const [
                UserHeader(),
                UserTable(),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
