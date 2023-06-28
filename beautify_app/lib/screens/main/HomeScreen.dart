import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/dashboard/dashboard.dart';
import 'package:flutter/material.dart';

import '../../helper/responsivesLayout.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return SiteLayout(child: const DashBoardScreen());
  }
}
