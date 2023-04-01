import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestPhongBanDto {
  String? id;
  String? tenPhongBan;
  SuggestPhongBanDto({
    this.id,
    this.tenPhongBan,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenPhongBan': tenPhongBan,
    };
  }

  factory SuggestPhongBanDto.fromMap(Map<String, dynamic> map) {
    return SuggestPhongBanDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenPhongBan: map['tenPhongBan'] != null ? map['tenPhongBan'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestPhongBanDto.fromJson(String source) => SuggestPhongBanDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
