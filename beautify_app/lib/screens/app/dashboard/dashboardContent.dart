import 'package:flutter/material.dart';
import 'dashboard_appointments.dart';
import 'dashboard_sales.dart';
import 'over_view/OverView.dart';

class DashboardContent extends StatelessWidget {
  const DashboardContent({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      child: Container(
        color: const Color(0xffE8E9F2),
        child: Column(
          children:  [Header(),const Overview(),const Appointments(),const Sales()],
        ),
      ),
    );
  }
}

class Header extends StatelessWidget implements PreferredSizeWidget {
  final GlobalKey<ScaffoldState> _scaffoldKey = GlobalKey<ScaffoldState>();

   Header({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(2),
      child: Container(
        height: 90,
        decoration: const BoxDecoration(
          color: Colors.white,
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: const [
                  Text("Trang chủ", style: TextStyle(fontSize: 20)),
                  Text(
                    "Tổng quan",
                    style: TextStyle(fontSize: 28),
                  ),
                ],
              ),
            ),
            Padding(
              padding: const EdgeInsets.all(16),
              child: Row(
                children: [
                  SizedBox(
                    width: 40,
                    height: 40,
                    child: Container(
                      decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(5),
                          color: Colors.white,
                          border: Border.all(color: const Color(0xFFD0D5DD))),
                      child: IconButton(
                        icon: const Icon(Icons.menu),
                        onPressed: () {
                          _scaffoldKey.currentState!.openDrawer();
                        },
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(right: 8.0, left: 8.0),
                    child: SizedBox(
                      height: 40,
                      child: ElevatedButton.icon(
                        onPressed: () {},
                        icon: const Icon(Icons.add),
                        label: const Text("Thêm"),
                        style: ElevatedButton.styleFrom(
                          backgroundColor:
                              const Color(0xFF7C3367), // background
                          foregroundColor: Colors.white, // foreground
                        ),
                      ),
                    ),
                  )
                ],
              ),
            )
          ],
        ),
      ),
    );
  }

  @override
  
  Size get preferredSize => const Size.fromHeight(90);
}