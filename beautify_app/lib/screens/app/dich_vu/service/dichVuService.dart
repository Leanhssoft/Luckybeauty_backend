import 'dart:convert';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/loai_dich_vu_model.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;
import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_filter.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/nhom_dich_vu_model.dart';
import 'package:flutter/foundation.dart';

class DichVuService {
  Future<List<DichVuViewModel>> getDichVu(DichVuFilter input) async {
    final token = await SessionManager().get("accessToken");
    final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/HangHoa/GetDMHangHoa'),
        headers: {
          'accept': 'text/plain',
          'Content-Type': 'application/json-patch+json',
          'Authorization': 'Bearer $token'
        },
        body: jsonEncode(input.toJson()));
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final items = List<Map<String, dynamic>>.from(data['result']['items']);

      final result =
          items.map((json) => DichVuViewModel.fromJson(json)).toList();
      return result;
    } else {
      throw Exception('Failed to get getDichVu: ${response.statusCode}');
    }
  }

  Future<List<LoaiDichVuDto>> getLoaiDichVu() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse('${Constants.BASE_URL}/api/services/app/LoaiHangHoa/GetAll'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result =
            items.map((json) => LoaiDichVuDto.fromJson(json)).toList();
        return result;
      } else {
        throw Exception('Failed to get LoaiDichVu: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get LoaiDichVu: $e');
    }
  }

  Future<List<NhomDichVuDto>> getNhomDichVu() async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/NhomHangHoa/GetNhomDichVu'),
        headers: {'Authorization': 'Bearer $token'},
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result =
            items.map((json) => NhomDichVuDto.fromJson(json)).toList();
        return result;
      } else {
        throw Exception('Failed to get nhomdv: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to get nhomDv: $e');
    }
  }

  Future<List<NhomDichVuDto>> insertNhomHangHoa(NhomDichVuDto input) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/NhomHangHoa/CreateNhomHangHoa'),
        headers: {'Authorization': 'Bearer $token'},
        body: jsonEncode(input.toJson()),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final result =
            items.map((json) => NhomDichVuDto.fromJson(json)).toList();
        return result;
      } else {
        throw Exception(' Exception insertNhomHangHoa: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed insertNhomHangHoa: $e');
    }
  }

  Future<String> updateNhomHangHoa(NhomDichVuDto input) async {
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
        Uri.parse(
            '${Constants.BASE_URL}/api/services/app/NhomHangHoa/UpdateNhomHangHoa'),
        headers: {'Authorization': 'Bearer $token'},
        body: jsonEncode(input.toJson()),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']);
        final mes = items.map((e) => e.toString());
        debugPrint('$mes');

        if (mes.isEmpty) {}
        return mes.first.toString();
      } else {
        throw Exception(' Exception insertNhomHangHoa: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed insertNhomHangHoa: $e');
    }
  }
}
