import 'dart:convert';
import 'package:beautify_app/screens/app/admin/user/models/PagedResultRequestDto.dart';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/admin/user/models/userDto.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';

import '../models/CreateUserDto.dart';

class UserServices {
  Future<List<UserDto>> GetAllUser(PagedUserResultRequestDto model) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/User/GetAll?keyWord=${model.keyWord}&skipCount=${model.skipCount}&maxResult=${model.maxResultCount}&isActive=${model.isActive ?? ''}'),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result = items
            .map((json) => UserDto(
                id: json['id'],
                userName: json['userName'],
                name: json['name'],
                surname: json['surname'],
                emailAddress: json["emailAddress"],
                isActive: json['isActive'],
                fullName: json['fullName'],
                lastLoginTime: json['lastLoginTime'],
                creationTime: json['creationTime'].toString(),
                roleNames: json['roleNames'],
                nhanSuId: json['nhanSuId']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get users');
      }
    } catch (e) {
      throw Exception('Failed to get users: $e');
    }
  }

  Future<void> createUser(CreateUserDto input) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse('${Constants.BASE_URL}/api/services/app/User/Create'),
        headers: {
          'accept': 'text/plain',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: json.encode(input.toJson()), // convert data to JSON string
      );

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
}
