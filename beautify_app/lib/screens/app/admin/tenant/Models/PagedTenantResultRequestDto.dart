// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class PagedTenantResultRequestDto {
  String? keyword;
  bool? isActive;
  int? skipCount;
  int? maxResultCount;
  PagedTenantResultRequestDto({
    this.keyword,
    this.isActive,
    this.skipCount = 0,
    this.maxResultCount= 10,
  });


  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'keyword': keyword,
      'isActive': isActive,
      'skipCount': skipCount,
      'maxResultCount': maxResultCount,
    };
  }

  factory PagedTenantResultRequestDto.fromMap(Map<String, dynamic> map) {
    return PagedTenantResultRequestDto(
      keyword: map['keyword'],
      isActive: map['isActive'],
      skipCount: map['skipCount'],
      maxResultCount: map['maxResultCount'],
    );
  }

  String toJson() => json.encode(toMap());

  factory PagedTenantResultRequestDto.fromJson(String source) => PagedTenantResultRequestDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
