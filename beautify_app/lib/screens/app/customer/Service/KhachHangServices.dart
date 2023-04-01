import 'dart:convert';

import 'package:beautify_app/BASE_CONFIG.dart';
import 'package:beautify_app/screens/app/customer/Models/CreateOrEditCustomerModel.dart';
import 'package:beautify_app/screens/app/customer/Models/KhachHangItemDto.dart';
import 'package:beautify_app/screens/app/customer/Models/PagedKhachHangRequestDto.dart';
import 'package:beautify_app/screens/app/customer/Models/PagedKhachHangResult.dart';
import 'package:beautify_app/screens/app/customer/create-or-edit-customer-modal.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';
import 'package:http/http.dart' as http;

class KhachHangServices {
  Future<bool> CreateKhachHang(CreateOrEditCustomerModel input) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse(
              '${Constants.BASE_URL}/api/services/app/KhachHang/CreateKhachHang'),
          headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json-patch+json',
            'Authorization': "Bearer $token"
          },
          body: input.toJson());
      if (response.statusCode == 200) {
        result = true;
      }
    } catch (e) {
      result = false;
      throw Exception(e);
    }
    return result;
  }

  Future<bool> UpdateKhachHang(CreateOrEditCustomerModel input) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse(
              '${Constants.BASE_URL}/api/services/app/KhachHang/EditKhachHang'),
          headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json-patch+json',
            'Authorization': "Bearer $token"
          },
          body: input.toJson());
      if (response.statusCode == 200) {
        result = true;
      }
    } catch (e) {
      result = false;
      throw Exception(e);
    }
    return result;
  }

  Future<PagedKhachHangResult> getAllKhachHang(
      PagedKhachHangRequestDto input) async {
    PagedKhachHangResult result = PagedKhachHangResult();
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse("${Constants.BASE_URL}/api/services/app/KhachHang/Search"),
          headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json-patch+json',
            'Authorization': 'Bearer $token'
          },
          body: input.toJson());
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final items = List<Map<String, dynamic>>.from(data['result']['items']);

        final totalCount = data['result']['totalCount'];
        result.items = items
            .map((json) => KhachHangItemDto(
                id: json['id'],
                tenKhachHang: json['tenKhachHang'],
                soDienThoai: json['soDienThoai'],
                gioiTinh: json['gioiTinh'],
                cuocHenGanNhat: json['cuocHenGanNhat'],
                nhanVienPhuTrach: json['nhanVienPhuTrach'],
                tenNguonKhach: json['tenNguonKhach'],
                tenNhomKhach: json['tenNhomKhach'],
                tongChiTieu: json['tongChiTieu']))
            .toList();
        result.totalCount = totalCount;
      } else {
        result.items = [];
        result.totalCount = 0;
      }
    } catch (e) {
      result.items = [];
      result.totalCount = 0;
    }
    return result;
  }

  Future<CreateOrEditCustomerModel> getKhachHang(String id) async {
    CreateOrEditCustomerModel result = CreateOrEditCustomerModel();
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.get(
          Uri.parse(
              "${Constants.BASE_URL}/api/services/app/KhachHang/GetKhachHangDetail?Id=$id"),
          headers: {'accept': 'text/plain', 'Authorization': 'Bearer $token'});
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final item = data['result'];
        result = CreateOrEditCustomerModel(
            id: id,
            avatar: item['avatar'],
            diaChi: item['diaChi'],
            email: item['email'],
            gioiTinh: item['gioiTinh'],
            idLoaiKhach: item['idLoaiKhach'],
            idNguonKhach: item['idNguonKhach'],
            idNhomKhach: item['idNhomKhach'],
            idQuanHuyen: item['idQuanHuyen'],
            idTinhThanh: item['idTinhThanh'],
            kieuNgaySinh: item['kieuNgaySinh'],
            maKhachHang: item['maKhachHang'],
            maSoThue: item['maSoThue'],
            moTa: item['moTa'],
            ngaySinh: item['ngaySinh'],
            soDienThoai: item['soDienThoai'],
            tenKhachHang: item['tenKhachHang'],
            tongTichDiem: item['tongTichDiem'],
            trangThai: item['trangThai']);
      }
    } catch (e) {
      result = CreateOrEditCustomerModel();
    }
    return result;
  }

  Future<bool> deleteKhachHang(String id) async {
    bool result = false;
    try {
      final token = await SessionManager().get("accessToken");
      final response = await http.post(
          Uri.parse(
              "${Constants.BASE_URL}/api/services/app/KhachHang/Delete?id=$id"),
          headers: {'accept': 'text/plain', 'Authorization': 'Bearer $token'});
      if (response.statusCode == 200) {
        result = true;
      }
    } catch (e) {
      result = false;
    }
    return result;
  }
}
