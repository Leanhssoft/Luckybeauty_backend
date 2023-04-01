import 'dart:convert';
import 'package:beautify_app/screens/app/lich_hen/models/BookingDto.dart';
import 'package:flutter/material.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:syncfusion_flutter_calendar/calendar.dart';
import 'package:http/http.dart' as http;
import '../../../BASE_CONFIG.dart';

class SfCalendarView extends StatefulWidget {
  const SfCalendarView({super.key});

  @override
  State<SfCalendarView> createState() => _SfCalendarViewState();
}

class _SfCalendarViewState extends State<SfCalendarView> {
  late AppointmentDataSource _event;
  late List<Appointment> _shiftCollection = <Appointment>[];
  Future<List<Appointment>> getAppointments() async {
    List<Appointment> appoiments = <Appointment>[];
    List<BookingDto> bookings = <BookingDto>[];
    final token = await SessionManager().get("accessToken");
    final response = await http.get(
        Uri.parse('${Constants.BASE_URL}/api/services/app/Booking/GetAll'),
        headers: {'accept': 'text/plain', 'Authorization': 'Bearer $token'});
    if (response.statusCode == 200) {
      // Handle successful response
      final body = jsonDecode(response.body);
      final items = List<Map<String, dynamic>>.from(body['result']);
      final bookings = items
          .map((json) => BookingDto(
              startTime: DateTime.parse(json['startTime']),
              endTime: DateTime.parse(json['endTime']),
              color: json['color'],
              noiDung: json['noiDung']))
          .toList();
      for (var e in bookings) {
        appoiments.add(Appointment(
          startTime: e.startTime,
          endTime: e.endTime,
          color: Colors.pinkAccent,
          subject: e.noiDung.toString(),
        ));
      }
    } else {
      appoiments = [];
    }
    return appoiments;
  }

  void getData() async {
    _shiftCollection = await getAppointments();
    _event = AppointmentDataSource(_shiftCollection);
    setState(() {});
  }

  @override
  void initState() {
    super.initState();
    getData();
  }

  @override
  Widget build(BuildContext context) {
    final List<CalendarView> allowedViews = <CalendarView>[
      CalendarView.day,
      CalendarView.timelineDay,
      CalendarView.week,
      CalendarView.month,
    ];
    return Container(
      height: MediaQuery.of(context).size.height,
      decoration: BoxDecoration(
          color: Colors.white, borderRadius: BorderRadius.circular(0)),
      child: SfCalendar(
        allowedViews: allowedViews,
        dataSource: _event,
        monthViewSettings: const MonthViewSettings(
            showAgenda: true,
            appointmentDisplayMode: MonthAppointmentDisplayMode.appointment,
            showTrailingAndLeadingDates: true),
        allowDragAndDrop: false,
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

class AppointmentDataSource extends CalendarDataSource {
  AppointmentDataSource(List<Appointment> source) {
    appointments = source;
  }
}
