import 'dart:convert';

import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

import '../BASE_CONFIG.dart';
import '../Models/TenanlModels/CreateTenantDto.dart';

class TenantService {
  // ignore: non_constant_identifier_names
  void CreateTenant(CreateTenantDto input) async {
    String token = await SessionManager().get("accessToken");
    // ignore: unused_local_variable
    final responsive = await http.post(
        Uri.parse("${Constants.BASE_URL}/api/services/app/Tenant/Create"),
        headers: {
          'accept': 'text/plant',
          'Content-Type': 'application/json-patch+json',
          'Authorization': 'Bearer $token'
        },
        body: jsonEncode(input));
  }
  
}
