import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestChucVu {
  late String? idChucVu;
  late String? tenChucVu;
  SuggestChucVu({
    required this.idChucVu,
    required this.tenChucVu,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': idChucVu,
      'tenChucVu': tenChucVu,
    };
  }

  factory SuggestChucVu.fromMap(Map<String, dynamic> map) {
    return SuggestChucVu(
      idChucVu: map['id'],
      tenChucVu: map['tenChucVu'],
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestChucVu.fromJson(String source) =>
      SuggestChucVu.fromMap(json.decode(source) as Map<String, dynamic>);
}
