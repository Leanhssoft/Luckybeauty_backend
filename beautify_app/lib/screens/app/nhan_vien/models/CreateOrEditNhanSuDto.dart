// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class CreateOrEditNhanSuDto {
  late String? id;
  late String? maNhanVien;
  late String? tenNhanVien;
  late String? diaChi;
  late String? soDienThoai;
  late String? cccd;
  late String? ngaySinh;
  late int? kieuNgaySinh;
  late int? gioiTinh;
  late String? ngayCap;
  late String? noiCap;
  late String? avatar;
  late String? idChucVu;
  late String? ghiChu;
  CreateOrEditNhanSuDto({
    this.id = '3fa85f64-5717-4562-b3fc-2c963f66afa6',
    this.maNhanVien = "NS000",
    this.tenNhanVien,
    this.diaChi,
    this.soDienThoai,
    this.cccd,
    this.ngaySinh,
    this.kieuNgaySinh = 0,
    this.gioiTinh = 0,
    this.ngayCap,
    this.noiCap,
    this.avatar,
    this.idChucVu,
    this.ghiChu,
  });

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
    ghiChu = json['ghiChu'];
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
      'ghiChu': ghiChu
    };
  }
}
