import 'package:beautify_app/layout.dart';
import 'package:flutter/material.dart';
import 'calendar.dart';

class CalendarWorkingPage extends StatefulWidget {
  const CalendarWorkingPage({super.key});

  @override
  State<CalendarWorkingPage> createState() => _CalendarWorkingPageState();
}

class _CalendarWorkingPageState extends State<CalendarWorkingPage> {
  @override
  Widget build(BuildContext context) {
    return  SiteLayout(child:const CalendarView());
  }
}
