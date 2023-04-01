import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestLoaiKhachHangDto {
  String? id;
  String? tenLoai;
  SuggestLoaiKhachHangDto({
    this.id,
    this.tenLoai,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenLoai': tenLoai,
    };
  }

  factory SuggestLoaiKhachHangDto.fromMap(Map<String, dynamic> map) {
    return SuggestLoaiKhachHangDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenLoai: map['tenLoai'] != null ? map['tenLoai'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestLoaiKhachHangDto.fromJson(String source) => SuggestLoaiKhachHangDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
