
import 'package:flutter/material.dart';
import 'dashboardContent.dart';
import '../../../components/header.dart';

class DashBoardScreen extends StatefulWidget {
  const DashBoardScreen({super.key});

  @override
  State<DashBoardScreen> createState() => _DashBoardScreenState();
}

class _DashBoardScreenState extends State<DashBoardScreen> {
  bool _isDrawerOpen = false;

  void _openDrawer() {
    setState(() {
      _isDrawerOpen = !_isDrawerOpen;
    });
    Scaffold.of(context).openDrawer();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: SingleChildScrollView(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              HeaderOfPage(openDrawerCallback: _openDrawer),
              const DashboardContent(),
            ],
          ),
        ),
      ),
    );
  }
}
