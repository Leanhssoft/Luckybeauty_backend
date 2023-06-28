import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class BanHangItemDto {
  String? id;
  String? tenKhachHang;
  String? tenVietTat;
  String? soDieThoai;
  String? diemTichLuy;
  String? ngayHen;
  String? thoiGianHen;
  int? trangThai;
  BanHangItemDto({
    this.id,
    this.tenKhachHang,
    this.tenVietTat,
    this.soDieThoai,
    this.diemTichLuy,
    this.ngayHen,
    this.thoiGianHen,
    this.trangThai,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenKhachHang': tenKhachHang,
      'tenVietTat': tenVietTat,
      'soDieThoai': soDieThoai,
      'diemTichLuy': diemTichLuy,
      'ngayHen': ngayHen,
      'thoiGianHen': thoiGianHen,
      'trangThai': trangThai,
    };
  }

  factory BanHangItemDto.fromMap(Map<String, dynamic> map) {
    return BanHangItemDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenKhachHang: map['tenKhachHang'] != null ? map['tenKhachHang'] as String : null,
      tenVietTat: map['tenVietTat'] != null ? map['tenVietTat'] as String : null,
      soDieThoai: map['soDieThoai'] != null ? map['soDieThoai'] as String : null,
      diemTichLuy: map['diemTichLuy'] != null ? map['diemTichLuy'] as String : null,
      ngayHen: map['ngayHen'] != null ? map['ngayHen'] as String : null,
      thoiGianHen: map['thoiGianHen'] != null ? map['thoiGianHen'] as String : null,
      trangThai: map['trangThai'] != null ? map['trangThai'] as int : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory BanHangItemDto.fromJson(String source) =>
      BanHangItemDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
