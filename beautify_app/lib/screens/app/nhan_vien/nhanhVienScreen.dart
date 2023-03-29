// ignore_for_file: prefer_const_constructors

import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanVienHeader.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanVienTable.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class NhanVienScreen extends StatefulWidget {
  const NhanVienScreen({super.key});

  @override
  State<NhanVienScreen> createState() => _NhanVienScreenState();
}

class _NhanVienScreenState extends State<NhanVienScreen> {
  bool checkAll = false;
  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: const [
                NhanVienHeader(),
                NhanVienTable(),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
