import 'package:beautify_app/components/sideMenu.dart';
import 'package:flutter/material.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'dashboardContent.dart';
import '../../../components/header.dart';

class DashBoardScreen extends StatefulWidget {
  const DashBoardScreen({super.key});

  @override
  State<DashBoardScreen> createState() => _DashBoardScreenState();
}

class _DashBoardScreenState extends State<DashBoardScreen> {
  bool _isDrawerOpen = false;
  
@override
  void initState() {
    super.initState();
  }
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
            children: const [
              //HeaderOfPage(openDrawerCallback: _openDrawer),
              DashboardContent(),
            ],
          ),
        ),
      ),
    );
  }
}
