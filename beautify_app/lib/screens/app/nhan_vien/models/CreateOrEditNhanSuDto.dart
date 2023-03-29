// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class CreateOrEditNhanSuDto {
  late String id;
  late String maNhanVien;
  late String tenNhanVien;
  late String diaChi;
  late String soDienThoai;
  late String cccd;
  late String ngaySinh;
  late int kieuNgaySinh;
  late int gioiTinh;
  late String ngayCap;
  late String noiCap;
  late String? avatar;
  late String idChucVu;

  CreateOrEditNhanSuDto(
      {required this.id,
      required this.maNhanVien,
      required this.tenNhanVien,
      required this.diaChi,
      required this.soDienThoai,
      required this.cccd,
      required this.ngaySinh,
      required this.kieuNgaySinh,
      required this.gioiTinh,
      required this.ngayCap,
      required this.noiCap,
      this.avatar,
      required this.idChucVu});

  CreateOrEditNhanSuDto.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    maNhanVien = json['maNhanVien'];
    tenNhanVien = json['tenNhanVien'];
    diaChi = json['diaChi'];
    soDienThoai = json['soDienThoai'];
    cccd = json['cccd'];
    ngaySinh = json['ngaySinh'];
    kieuNgaySinh = json['kieuNgaySinh'];
    gioiTinh = json['gioiTinh'];
    ngayCap = json['ngayCap'];
    noiCap = json['noiCap'];
    avatar = json['avatar'];
    idChucVu = json['idChucVu'];
  }

  String toJson() => json.encode(toMap());

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'maNhanVien': maNhanVien,
      'tenNhanVien': tenNhanVien,
      'diaChi': diaChi,
      'soDienThoai': soDienThoai,
      'cccd': cccd,
      'ngaySinh': ngaySinh,
      'kieuNgaySinh': kieuNgaySinh,
      'gioiTinh': gioiTinh,
      'ngayCap': ngayCap,
      'noiCap': noiCap,
      'avatar': avatar,
      'idChucVu': idChucVu,
    };
  }
}
