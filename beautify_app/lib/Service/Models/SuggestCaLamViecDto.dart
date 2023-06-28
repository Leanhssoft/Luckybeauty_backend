import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestCaLamViecDto {
  String? id;
  String? tenCa;
  SuggestCaLamViecDto({
    this.id,
    this.tenCa,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenCa': tenCa,
    };
  }

  factory SuggestCaLamViecDto.fromMap(Map<String, dynamic> map) {
    return SuggestCaLamViecDto(
      id: map['id'] != null ? map['id'] as String : null,
      tenCa: map['tenCa'] != null ? map['tenCa'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestCaLamViecDto.fromJson(String source) => SuggestCaLamViecDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
