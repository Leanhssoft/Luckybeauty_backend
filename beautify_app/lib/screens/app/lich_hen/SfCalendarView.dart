import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:month_year_picker/month_year_picker.dart';
import 'package:syncfusion_flutter_calendar/calendar.dart';

class SfCalendarView extends StatelessWidget {
  const SfCalendarView({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height,
      decoration: BoxDecoration(
          color: Colors.white, borderRadius: BorderRadius.circular(0)),
      child: SfCalendar(
        allowedViews: const [
          CalendarView.timelineDay,
          CalendarView.timelineWeek,
          CalendarView.timelineWorkWeek,
          CalendarView.timelineMonth
        ],
        monthViewSettings: const MonthViewSettings(showAgenda: true,appointmentDisplayMode: MonthAppointmentDisplayMode.appointment),
        showNavigationArrow: true,
        initialSelectedDate: DateTime.now(),
        initialDisplayDate: DateTime.now(),
        view: CalendarView.month,
        allowViewNavigation: true,
        showDatePickerButton: true,
        firstDayOfWeek: 1,
        timeZone: 'SE Asia Standard Time',
        todayHighlightColor: Colors.blue,
        resourceViewSettings: const ResourceViewSettings(
            displayNameTextStyle: TextStyle(
                fontSize: 11,
                color: Colors.redAccent,
                fontStyle: FontStyle.italic)),
      ),
    );
  }
}
