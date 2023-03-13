import 'dart:async';
import 'dart:convert';

import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

class LoginService {
  FutureOr<int> isTenantAvailable(String tenancyName) async {
    int? result;
    if (tenancyName.isEmpty) {
      tenancyName = "default";
    }
    final response = await http.Client().post(
      Uri.parse(
          '${Constants.BASE_URL}/api/services/app/Account/IsTenantAvailable'),
      headers: {
        'Content-Type': 'application/json',
      },
      body: jsonEncode({'tenancyName': tenancyName}),
    );
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      result = data['result']['tenantId'];
    } else {
      result = 0;
    }
    return result ?? 0;
  }

  Future<bool> login(
      String tenant, String userName, String password, bool rememberMe) async {
    var session = SessionManager();
    bool result = false;
    int tenantId = await isTenantAvailable(tenant);
    // Add code to authenticate user here
    if (tenantId > 0) {
      final response = await http.post(
        Uri.parse('${Constants.BASE_URL}/api/TokenAuth/Authenticate'),
        headers: {
          'Abp.TenantId': tenantId == 1 ? '' : tenantId.toString(),
          'Content-Type': 'application/json',
        },
        body: jsonEncode({
          'userNameOrEmailAddress': userName,
          'password': password,
          'rememberClient': rememberMe
        }),
      );
      if (response.statusCode == 200) {
        // Authentication successful
        final data = jsonDecode(response.body);
        final token = data['result']['accessToken'];
        final encryptedAccessToken = data['result']['encryptedAccessToken'];
        final expiredInSeconds = data['result']['expireInSeconds'];
        final userId = data['result']['userId'];
        session.set('accessToken', token);
        session.set("encryptedAccessToken", encryptedAccessToken);
        session.set("exprieInSeconds", expiredInSeconds);
        session.set("userId", userId ?? 1);
        result = true;
      } else {
        // Authentication failed
        result = false;
      }
    } else {
      result = false;
    }
    return result;
  }
}
