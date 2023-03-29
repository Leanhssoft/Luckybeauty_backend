import 'package:flutter/material.dart';
import 'package:syncfusion_flutter_calendar/calendar.dart';

class SfCalendarView extends StatelessWidget {
  const SfCalendarView({super.key});

  @override
  Widget build(BuildContext context) {
    final List<CalendarView> allowedViews = <CalendarView>[
      CalendarView.day,
      CalendarView.week,
      CalendarView.month,
    ];
    return Container(
      height: MediaQuery.of(context).size.height,
      decoration: BoxDecoration(
          color: Colors.white, borderRadius: BorderRadius.circular(0)),
      child: SfCalendar(
        allowedViews: allowedViews,
        monthViewSettings: const MonthViewSettings(
            showAgenda: true,
            appointmentDisplayMode: MonthAppointmentDisplayMode.appointment,
            showTrailingAndLeadingDates: true),
        allowDragAndDrop: true,
        showNavigationArrow: true,
        initialSelectedDate: DateTime.now(),
        initialDisplayDate: DateTime.now(),
        view: CalendarView.week,
        allowViewNavigation: true,
        showDatePickerButton: true,
        blackoutDatesTextStyle: const TextStyle(
            decoration: TextDecoration.lineThrough, color: Colors.red),
        viewNavigationMode: ViewNavigationMode.snap,
        timeSlotViewSettings: const TimeSlotViewSettings(
            numberOfDaysInView: 7,
            minimumAppointmentDuration: Duration(minutes: 60)),
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
