import 'dart:convert';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/loai_dich_vu_model.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:get/get.dart';
import 'package:http/http.dart' as http;
import 'package:beautify_app/BASE_CONFIG.dart';

class DichVuService {
  Future<List<dynamic>> getDichVu() async {
    String token = await SessionManager().get("accessToken");
    final response = await http.get(
        Uri.parse('${Constants.BASE_URL}/api/services/app/HangHoa/GetAll'),
        headers: {'Authorization': "Bearer $token"});
    // Add code to authenticate user here
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final items = data['result']['items'];

      final result = items
          .map((json) => DichVuViewModel(
                id: json['id'],
                maHangHoa: json['maHangHoa'],
                tenHangHoa: json['tenHangHoa'],
                loaiHangHoa: json['loaiHangHoa'],
                giaBan: json['giaBan'],
                trangThaiText: json['trangThaiText'],
              ))
          .toList();
      if (kDebugMode) {
        print(result);
      }
      return result;
    } else {
      return [];
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
}
