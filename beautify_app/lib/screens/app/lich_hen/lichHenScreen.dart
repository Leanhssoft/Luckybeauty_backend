import 'package:beautify_app/layout.dart';
import 'package:flutter/material.dart';
import 'SfCalendarView.dart';
import 'calendar.dart';
import 'lich-hen-header.dart';

class CalendarWorkingPage extends StatefulWidget {
  const CalendarWorkingPage({super.key});

  @override
  State<CalendarWorkingPage> createState() => _CalendarWorkingPageState();
}

class _CalendarWorkingPageState extends State<CalendarWorkingPage> {
  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: const [
                Divider(
                  color: Color(0xFFE6E1E6),
                  thickness: 2,
                ),
                LichHenHeader(),
                Divider(
                  color: Color(0xFFE6E1E6),
                  thickness: 3,
                ),
                SfCalendarView(),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
