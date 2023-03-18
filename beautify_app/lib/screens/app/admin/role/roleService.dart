import 'dart:convert';
import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/admin/role/models/createRoleDto.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;
import 'models/PagedRoleResultRequestDto.dart';
import 'models/RoleDto.dart';
import 'models/permissionViewModel.dart';

class RoleService {
  Future<List<PermissionViewModel>> getAllPermission() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Role/GetAllPermissions'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result =
            items.map((json) => PermissionViewModel.fromJson(json)).toList();
        return result;
      } else {
        throw Exception('Failed to get LoaiDichVu: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get LoaiDichVu: $e');
    }
  }

  Future<void> createRole(CreateRoleDto input) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse('${Constants.BASE_URL}/api/services/app/Role/Create'),
          headers: {
            'accept': 'text/plant',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer $token',
          },
          body: input.toJson());

      if (response.statusCode == 200) {
        if (kDebugMode) {
          print("Thêm mới quyền thành công");
        }
      } else {
        throw Exception('Failed to create role: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to create role: $e');
    }
  }

  Future<List<RoleDto>> getRoles(
      PagedRoleResultRequestDto model) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Role/GetAll?keyWord=${model.keyWord}&skipCount=${model.skipCount}&maxResult=${model.maxResultCount}'),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result =
            items.map((json) => RoleDto.fromJson(json)).toList();
        return result;
      } else {
        throw Exception('Failed to get roles');
      }
    } catch (e) {
      throw Exception('Failed to get roles: $e');
    }
  }
}
