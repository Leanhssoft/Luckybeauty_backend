import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/Models/PermissionAndMenu/UserPermission.dart';

import 'dart:async';
import 'dart:convert';

import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

class UserPermissionServices {
  Future<UserPermission> getUserPermission() async {
    String token = await SessionManager().get("accessToken");
    var userId = await SessionManager().get("userId");
    final response = await http.post(
      Uri.parse(
          "${Constants.BASE_URL}/api/services/app/Permission/GetAllPermissionByRole?UserId=$userId"),
      headers: {"accept": "text/plain", "Authorization": "Bearer $token"},
    );
    if (response.statusCode == 200) {
      // Authentication successful
      final data = jsonDecode(response.body);
      var user = data["result"]["name"];
      var permissions = data["result"]["permissions"];
      return UserPermission(user: user, permissions: permissions);
    } else {
      // Authentication failed
      return UserPermission(user: '', permissions: []);
    }
  }
}
