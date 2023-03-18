import 'package:beautify_app/components/header.dart';
import 'package:beautify_app/screens/app/lich_hen/SfCalendarView.dart';
import 'package:flutter/material.dart';
import 'package:syncfusion_flutter_calendar/calendar.dart';
import 'package:syncfusion_flutter_calendar/src/calendar/common/enums.dart';

class CalendarView extends StatefulWidget {
  const CalendarView({super.key});

  @override
  State<CalendarView> createState() => _CalendarViewState();
}

class _CalendarViewState extends State<CalendarView> {
  bool _isDrawerOpen = false;

  void _openDrawerCallback() {
    setState(() {
      _isDrawerOpen = !_isDrawerOpen;
    });
    Scaffold.of(context).openDrawer();
  }

  final CalendarController _controller = CalendarController();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
          child: SingleChildScrollView(
        child: Container(
          color: const Color(0xffE8E9F2),
          child: Column(
            children: [
              Padding(
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
                        padding: const EdgeInsets.all(16.0),
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: const [
                            Text("Lịch hẹn", style: TextStyle(fontSize: 20)),
                            Text(
                              "Lịch hẹn",
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
                                    border: Border.all(
                                        color: const Color(0xFFD0D5DD))),
                                child: const Icon(Icons.more_horiz),
                              ),
                            ),
                            Padding(
                              padding:
                                  const EdgeInsets.only(right: 8.0, left: 8.0),
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
              ),
              const SfCalendarView()
            ],
          ),
        ),
      )),
    );
  }
}
