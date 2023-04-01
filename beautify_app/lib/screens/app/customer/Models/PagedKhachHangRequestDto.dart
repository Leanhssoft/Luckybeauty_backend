import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class PagedKhachHangRequestDto {
  int maxResultCount;
  int skipCount;
  String keyword;
  PagedKhachHangRequestDto({
    this.maxResultCount = 10,
    this.skipCount =0,
    this.keyword = '',
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'maxResultCount': maxResultCount,
      'skipCount': skipCount,
      'keyword': keyword,
    };
  }

  factory PagedKhachHangRequestDto.fromMap(Map<String, dynamic> map) {
    return PagedKhachHangRequestDto(
      maxResultCount: map['maxResultCount'] as int,
      skipCount: map['skipCount'] as int,
      keyword: map['keyword'] as String,
    );
  }

  String toJson() => json.encode(toMap());

  factory PagedKhachHangRequestDto.fromJson(String source) => PagedKhachHangRequestDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
