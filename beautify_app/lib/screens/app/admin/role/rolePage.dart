// ignore_for_file: prefer_const_constructors

import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/admin/role/roleHeader.dart';
import 'package:beautify_app/screens/app/admin/role/roleTable.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanVienHeader.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanVienTable.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class RolePage extends StatefulWidget {
  const RolePage({super.key});

  @override
  State<RolePage> createState() => _RolePageScreenState();
}

class _RolePageScreenState extends State<RolePage> {
  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: [
                RoleHeader(),
                RoleTable(parentContext: context),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
