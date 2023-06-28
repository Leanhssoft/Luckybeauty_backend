// ignore_for_file: prefer_const_constructors

import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/customer/customerHeader.dart';
import 'package:beautify_app/screens/app/customer/customerTable.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class KhachHangScreen extends StatefulWidget {
  const KhachHangScreen({super.key});

  @override
  State<KhachHangScreen> createState() => _KhachHangScreenState();
}

class _KhachHangScreenState extends State<KhachHangScreen> {
  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: const [
                KhachHangHeader(),
                KhachHangTable(),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
