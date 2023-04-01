import 'dart:convert';
import 'package:beautify_app/screens/app/admin/user/models/PagedResultRequestDto.dart';
import 'package:beautify_app/screens/app/admin/user/models/SuggestNhanSuDto.dart';
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

  Future<UserDto> GetUser(int id) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse('${Constants.BASE_URL}/api/services/app/User/Get?Id=$id'),
        headers: {
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final item = data['result'];
        final result = UserDto(
            userName: item['userName'],
            name: item['name'],
            surname: item['surname'],
            emailAddress: item['emailAddress'],
            isActive: item['isActive'],
            fullName: item['fullName'],
            creationTime: item['creationTime'],
            roleNames: item['roleNames'],
            id: id);
        return result;
      } else {
        throw Exception('Failed to get user');
      }
    } catch (e) {
      throw Exception('Failed to get user: $e');
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

  Future<void> updateUser(UserDto input) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.put(
        Uri.parse('${Constants.BASE_URL}/api/services/app/User/Update'),
        headers: {
          'accept': 'text/plain',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: json.encode(input.toJson()), // convert data to JSON string
      );

      if (response.statusCode == 200) {
        if (kDebugMode) {
          print("Thêm mới thành công");
        }
      } else {
        throw Exception('Failed to create: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to create: $e');
    }
  }

  Future<bool> deleteUser(int id) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse('${Constants.BASE_URL}/api/services/app/User/DeleteUser'),
        headers: {
          'accept': 'text/plain',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: json.encode({'id': id}), // convert data to JSON string
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final item = data['result'];
        result = item;
      } else {
        result = false;
      }
    } catch (e) {
      result = false;
    }
    return result;
  }

  Future<List<SuggestNhanSuDto>> suggestNhanSu() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse('${Constants.BASE_URL}/api/services/app/Suggest/SuggestNhanSus'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestNhanSuDto(
                id: json['id'], tenNhanVien: json['tenNhanVien']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get chuc vu: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get chuc vu: $e');
    }
  }
}
