// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class SuggestKhachHangDto {
  String? id;
  String? tenKhachHang;
  String? soDienThoai;
  SuggestKhachHangDto({
    this.id,
    this.tenKhachHang,
    this.soDienThoai,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenKhachHang': tenKhachHang,
      'soDienThoai': soDienThoai,
    };
  }

  factory SuggestKhachHangDto.fromMap(Map<String, dynamic> map) {
    return SuggestKhachHangDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenKhachHang: map['tenKhachHang'] != null ? map['tenKhachHang'] as String : null,
      soDienThoai: map['soDienThoai'] != null ? map['soDienThoai'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestKhachHangDto.fromJson(String source) => SuggestKhachHangDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
