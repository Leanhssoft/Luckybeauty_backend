import 'dart:convert';

import 'package:beautify_app/screens/app/admin/tenant/Models/PagedTenantResultRequestDto.dart';
import 'package:beautify_app/screens/app/admin/tenant/Models/Tenant.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

import '../../../../BASE_CONFIG.dart';
import '../../../../Models/TenanlModels/CreateTenantDto.dart';

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

  Future<List<Tenant>> getAll(PagedTenantResultRequestDto input) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Tenant/GetAll?keyWord=${input.keyword}&skipCount=${input.skipCount}&maxResult=${input.maxResultCount}&isActive= ${input.isActive ?? ''}'),
        headers: <String, String>{
          'accept': 'plain',
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result = items
            .map((json) => Tenant(
                id: json['id'],
                isActive: json['isActive'],
                name: json['name'],
                tenancyName: json['tenancyName']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get roles');
      }
    } catch (e) {
      throw Exception('Failed to get roles: $e');
    }
  }
}
