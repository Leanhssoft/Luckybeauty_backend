import 'dart:convert';
import 'package:beautify_app/Service/Models/SuggestCaLamViecDto.dart';
import 'package:beautify_app/Service/Models/SuggestChiNhanhDto.dart';
import 'package:beautify_app/Service/Models/SuggestDonViQuiDoiDto.dart';
import 'package:beautify_app/Service/Models/SuggestHangHoaDto.dart';
import 'package:beautify_app/Service/Models/SuggestKhachHangDto.dart';
import 'package:beautify_app/Service/Models/SuggestLoaiHangHoaDto.dart';
import 'package:beautify_app/Service/Models/SuggestNguonKhachHangDto.dart';
import 'package:beautify_app/Service/Models/SuggestNhomKhachHang.dart';
import 'package:beautify_app/Service/Models/SuggestPhongBanDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/SuggestChucVuDto.dart';
import 'package:http/http.dart' as http;
import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/admin/user/models/SuggestNhanSuDto.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';

import 'Models/SuggestLoaiKhachHangDto.dart';

class SuggestServices {
  Future<List<SuggestChucVu>> suggestChucVus() async {
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

  Future<List<SuggestNhanSuDto>> suggestNhanSu() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestNhanSus'),
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
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestKhachHangDto>> suggestKhachHang() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestKhachHangs'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestKhachHangDto(
                id: json['id'],
                tenKhachHang: json['tenKhachHang'],
                soDienThoai: json['soDienThoai']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get khach hang: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get khach hang: $e');
    }
  }

  Future<List<SuggestLoaiKhachHangDto>> suggestLoaiKhachHang() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestLoaiKhachHangs'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestLoaiKhachHangDto(
                id: json['id'], tenLoai: json['tenLoai']))
            .toList();
        return result;
      } else {
        throw Exception(
            'Failed to get loai khach hang: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get loai khach hang: $e');
    }
  }

  Future<List<SuggestNhomKhachHangDto>> suggestNhomKhachHang() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestNhomKhachHangs'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestNhomKhachHangDto(
                id: json['id'], tenNhomKhach: json['tenNhomKhach']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhom khach: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhom khach: $e');
    }
  }

  Future<List<SuggestNguonKhachHang>> suggestNguonKhachHang() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestNguonKhachHangs'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestNguonKhachHang(
                id: json['id'], tenNguonKhach: json['tenNguonKhach']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestHangHoaDto>> suggestHangHoa() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestHangHoas'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestHangHoaDto(
                id: json['id'], loaiHangHoa: json['loaiHangHoa']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestLoaiHangHoaDto>> suggestLoaiHangHoa() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestLoaiHangHoas'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) =>
                SuggestLoaiHangHoaDto(id: json['id'], tenLoai: json['tenLoai']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestDonViQuiDoi>> suggestDonViQuiDoi() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestDonViQuiDois'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) =>
                SuggestDonViQuiDoi(id: json['id'], tenDonVi: json['tenDonVi']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestChiNhanhDto>> suggestChiNhanh() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestChiNhanhs'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestChiNhanhDto(
                id: json['id'], tenChiNhanh: json['tenChiNhanh']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestCaLamViecDto>> suggestCaLamViec() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestCaLamViecs'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) =>
                SuggestCaLamViecDto(id: json['id'], tenCa: json['tenCa']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }

  Future<List<SuggestPhongBanDto>> suggestPhongBan() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/Suggest/SuggestPhongBans'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);

        final result = items
            .map((json) => SuggestPhongBanDto(
                id: json['id'], tenPhongBan: json['tenPhongBan']))
            .toList();
        return result;
      } else {
        throw Exception('Failed to get nhan su: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhan su: $e');
    }
  }
}
