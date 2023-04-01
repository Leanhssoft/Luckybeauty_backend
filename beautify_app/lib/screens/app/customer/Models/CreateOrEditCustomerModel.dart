// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class CreateOrEditCustomerModel {
  String? id;
  String? maKhachHang;
  String? tenKhachHang;
  String? soDienThoai;
  String? diaChi;
  int? gioiTinh;
  String? email;
  String? moTa;
  int? trangThai;
  double? tongTichDiem;
  String? maSoThue;
  String? avatar;
  String? ngaySinh;
  int? kieuNgaySinh;
  int? idLoaiKhach;
  String? idNhomKhach;
  String? idNguonKhach;
  String? idTinhThanh;
  String? idQuanHuyen;

  CreateOrEditCustomerModel(
      {this.id = '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      this.maKhachHang,
      this.tenKhachHang,
      this.soDienThoai,
      this.diaChi,
      this.gioiTinh,
      this.email,
      this.moTa,
      this.trangThai = 1,
      this.tongTichDiem = 0,
      this.maSoThue,
      this.avatar,
      this.ngaySinh,
      this.kieuNgaySinh = 0,
      this.idLoaiKhach = 1,
      this.idNhomKhach,
      this.idNguonKhach,
      this.idTinhThanh,
      this.idQuanHuyen});

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'maKhachHang': maKhachHang,
      'tenKhachHang': tenKhachHang,
      'soDienThoai': soDienThoai,
      'diaChi': diaChi,
      'gioiTinh': gioiTinh,
      'email': email,
      'moTa': moTa,
      'trangThai': trangThai,
      'tongTichDiem': tongTichDiem,
      'maSoThue': maSoThue,
      'avatar': avatar,
      'ngaySinh': ngaySinh,
      'kieuNgaySinh': kieuNgaySinh,
      'idLoaiKhach': idLoaiKhach,
      'idNhomKhach': idNhomKhach,
      'idNguonKhach': idNguonKhach,
      'idTinhThanh': idTinhThanh,
      'idQuanHuyen': idQuanHuyen,
    };
  }

  factory CreateOrEditCustomerModel.fromMap(Map<String, dynamic> map) {
    return CreateOrEditCustomerModel(
      id: map['id'] != null ? map['id'] as String : null,
      maKhachHang:
          map['maKhachHang'] != null ? map['maKhachHang'] as String : null,
      tenKhachHang:
          map['tenKhachHang'] != null ? map['tenKhachHang'] as String : null,
      soDienThoai:
          map['soDienThoai'] != null ? map['soDienThoai'] as String : null,
      diaChi: map['diaChi'] != null ? map['diaChi'] as String : null,
      gioiTinh: map['gioiTinh'] != null ? map['gioiTinh'] as int : 0,
      email: map['email'] != null ? map['email'] as String : null,
      moTa: map['moTa'] != null ? map['moTa'] as String : null,
      trangThai: map['trangThai'] != null ? map['trangThai'] as int : null,
      tongTichDiem:
          map['tongTichDiem'] != null ? map['tongTichDiem'] as double : 0,
      maSoThue: map['maSoThue'] != null ? map['maSoThue'] as String : null,
      avatar: map['avatar'] != null ? map['avatar'] as String : null,
      ngaySinh: map['ngaySinh'] != null ? map['ngaySinh'] as String : null,
      kieuNgaySinh:
          map['kieuNgaySinh'] != null ? map['kieuNgaySinh'] as int : 0,
      idLoaiKhach:
          map['idLoaiKhach'] != null ? map['idLoaiKhach'] as int : null,
      idNhomKhach:
          map['idNhomKhach'] != null ? map['idNhomKhach'] as String : null,
      idNguonKhach:
          map['idNguonKhach'] != null ? map['idNguonKhach'] as String : null,
      idTinhThanh:
          map['idTinhThanh'] != null ? map['idTinhThanh'] as String : null,
      idQuanHuyen:
          map['idQuanHuyen'] != null ? map['idQuanHuyen'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory CreateOrEditCustomerModel.fromJson(String source) =>
      CreateOrEditCustomerModel.fromMap(
          json.decode(source) as Map<String, dynamic>);
}
