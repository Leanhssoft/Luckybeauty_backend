import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestLoaiHangHoaDto {
  String? id;
  String? tenLoai;
  SuggestLoaiHangHoaDto({
    this.id,
    this.tenLoai,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenLoai': tenLoai,
    };
  }

  factory SuggestLoaiHangHoaDto.fromMap(Map<String, dynamic> map) {
    return SuggestLoaiHangHoaDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenLoai: map['tenLoai'] != null ? map['tenLoai'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestLoaiHangHoaDto.fromJson(String source) => SuggestLoaiHangHoaDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
