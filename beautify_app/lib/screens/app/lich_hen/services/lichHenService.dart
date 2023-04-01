import 'dart:convert';

import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/lich_hen/models/CreateBookingDto.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

class LichHenService {
  Future<bool> createBooking(CreateBookingDto input) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse(
              '${Constants.BASE_URL}/api/services/app/Booking/CreateBooking'),
          headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json-patch+json',
            'Authorization': 'Bearer $token'
          },
          body: input.toJson());
      print(input.toJson());
      // ignore: unrelated_type_equality_checks
      if (response.statusCode == 200) {
        var data = jsonDecode(response.body);
        result = data['success'];
      }
    } catch (e) {
      result = false;
      throw Exception(e);
    }
    return result;
  }
}
