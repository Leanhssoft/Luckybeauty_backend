import 'dart:convert';

import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/CreateOrEditNhanSuDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuFilter.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuItemDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/SuggestChucVuDto.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

class NhanVienService {
  Future<bool> createOrEditNhanVien(CreateOrEditNhanSuDto input) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse(
              '${Constants.BASE_URL}/api/services/app/NhanSu/CreateOrEdit'),
          headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json-patch+json',
            'Authorization': 'Bearer $token',
          },
          body: input.toJson());

      if (response.statusCode == 200) {
        result = true;
      } else {
        result = false;
        throw Exception('Failed to create role: ${response.statusCode}');
      }
    } catch (e) {
      result = false;
      throw Exception('Failed to create role: $e');
    }
    return result;
  }

  Future<List<NhanSuDto>> getAll(NhanSuFilter model) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/NhanSu/GetAll?keyWord=${model.keyWord}&skipCount=${model.skipCount}&maxResult=${model.maxResultCount}'),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result = items.map((json) => NhanSuDto.fromJson(json)).toList();
        return result;
      } else {
        throw Exception('Failed to get roles');
      }
    } catch (e) {
      throw Exception('Failed to get roles: $e');
    }
  }

  Future<List<NhanSuItemDto>> getAllNhanVien(NhanSuFilter model) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse(
              '${Constants.BASE_URL}/api/services/app/NhanSu/Search?keyWord=${model.keyWord}'),
          headers: <String, String>{
            'accept': 'text/plain',
            'Content-Type': 'application/json; charset=UTF-8',
            'Authorization': 'Bearer $token',
          },
          body: {
            {
              "maxResultCount": model.maxResultCount,
              "skipCount": model.skipCount
            }
          });
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result = items
            .map((json) => NhanSuItemDto(
                avatar: json['avatar'],
                cccd: json['cccd'],
                diaChi: json['diaChi'],
                gioiTinh: json['gioiTinh'],
                id: json['id'],
                kieuNgaySinh: json['kieuNgaySinh'],
                maNhanVien: json['maNhanVien'],
                ngayCap: json['ngayCap'],
                ngaySinh: json['ngaySinh'],
                ngayVaoLam: json['ngayVaoLam'],
                noiCap: json['noiCap'],
                soDienThoai: json['soDienThoai'],
                tenChucVu: json['tenChucVu'],
                tenNhanVien: json['tenNhanVien']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get roles');
      }
    } catch (e) {
      throw Exception('Failed to get roles: $e');
    }
  }

  Future<bool> deleteNhanSu(String id) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse('${Constants.BASE_URL}/api/services/app/NhanSu/Delete'),
          headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer $token',
          },
          body: {
            'id': id
          });

      if (response.statusCode == 200) {
        result = true;
        print("Cập nhật quyền thành công");
      } else {
        result = false;
        throw Exception('Failed to create role: ${response.statusCode}');
      }
    } catch (e) {
      result = false;
      throw Exception('Failed to create role: $e');
    }
    return result;
  }

  Future<List<SuggestChucVu>> suggestChucVu() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestChucVus'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestChucVu(
                idChucVu: json['idChucVu'], tenChucVu: json['tenChucVu']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get chuc vu: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get chuc vu: $e');
    }
  }

  Future<CreateOrEditNhanSuDto> getNhanVien(String id) async {
    CreateOrEditNhanSuDto result = CreateOrEditNhanSuDto();
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/NhanSu/GetDetail?id=$id'),
        headers: {
          'accept': 'text/plain',
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        var data = jsonDecode(response.body);
        var nhanVien = data['result'];
        result.avatar = nhanVien['avatar'];
        result.cccd = nhanVien['cccd'];
        result.diaChi = nhanVien['diaChi'];
        result.ghiChu = nhanVien['ghiChu'];
        result.gioiTinh = nhanVien['gioiTinh'];
        result.id = id;
        result.idChucVu = nhanVien['idChucVu'];
        result.kieuNgaySinh = nhanVien['kieuNgaySinh'];
        result.maNhanVien = nhanVien['maNhanVien'];
        result.ngayCap = nhanVien['ngayCap'];
        result.ngaySinh = nhanVien['ngaySinh'];
        result.noiCap = nhanVien['noiCap'];
        result.soDienThoai = nhanVien['soDienThoai'];
        result.tenNhanVien = nhanVien['tenNhanVien'];
      } else {
        result = CreateOrEditNhanSuDto();
      }
    } catch (e) {
      result = CreateOrEditNhanSuDto();
    }
    return result;
  }
}
