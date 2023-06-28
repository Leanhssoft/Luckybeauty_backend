import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestHangHoaDto {
  String? id;
  String? loaiHangHoa;
  SuggestHangHoaDto({
    this.id,
    this.loaiHangHoa,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'loaiHangHoa': loaiHangHoa,
    };
  }

  factory SuggestHangHoaDto.fromMap(Map<String, dynamic> map) {
    return SuggestHangHoaDto(
      id: map['id'] != null ? map['id'] as String : null,
      loaiHangHoa: map['loaiHangHoa'] != null ? map['loaiHangHoa'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestHangHoaDto.fromJson(String source) => SuggestHangHoaDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
