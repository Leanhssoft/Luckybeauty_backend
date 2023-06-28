import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestChiNhanhDto {
  String? id;
  String? tenChiNhanh;
  SuggestChiNhanhDto({
    this.id,
    this.tenChiNhanh,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenChiNhanh': tenChiNhanh,
    };
  }

  factory SuggestChiNhanhDto.fromMap(Map<String, dynamic> map) {
    return SuggestChiNhanhDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenChiNhanh: map['tenChiNhanh'] != null ? map['tenChiNhanh'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestChiNhanhDto.fromJson(String source) => SuggestChiNhanhDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
