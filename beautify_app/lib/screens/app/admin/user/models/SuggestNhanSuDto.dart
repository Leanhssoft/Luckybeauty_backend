import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestNhanSuDto {
  late String? id;
  late String? tenNhanVien;
  late String? soDienThoai;
  SuggestNhanSuDto(
      {required this.id, required this.tenNhanVien, this.soDienThoai});

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenChucVu': tenNhanVien,
      'soDienThoai': soDienThoai
    };
  }

  factory SuggestNhanSuDto.fromMap(Map<String, dynamic> map) {
    return SuggestNhanSuDto(
        id: map['id'],
        tenNhanVien: map['tenChucVu'],
        soDienThoai: map['soDienThoai']);
  }

  String toJson() => json.encode(toMap());

  factory SuggestNhanSuDto.fromJson(String source) =>
      SuggestNhanSuDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
