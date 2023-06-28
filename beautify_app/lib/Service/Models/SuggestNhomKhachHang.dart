import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestNhomKhachHangDto {
  String? id;
  String? tenNhomKhach;
  SuggestNhomKhachHangDto({
    this.id,
    this.tenNhomKhach,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenNhomKhach': tenNhomKhach,
    };
  }

  factory SuggestNhomKhachHangDto.fromMap(Map<String, dynamic> map) {
    return SuggestNhomKhachHangDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenNhomKhach: map['tenNhomKhach'] != null ? map['tenNhomKhach'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestNhomKhachHangDto.fromJson(String source) => SuggestNhomKhachHangDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
