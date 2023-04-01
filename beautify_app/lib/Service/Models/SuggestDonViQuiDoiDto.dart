import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestDonViQuiDoi {
  String? id;
  String? tenDonVi;
  SuggestDonViQuiDoi({
    this.id,
    this.tenDonVi,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenDonVi': tenDonVi,
    };
  }

  factory SuggestDonViQuiDoi.fromMap(Map<String, dynamic> map) {
    return SuggestDonViQuiDoi(
      id: map['id'] != null ? map['id'] as String : null,
      tenDonVi: map['tenDonVi'] != null ? map['tenDonVi'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestDonViQuiDoi.fromJson(String source) => SuggestDonViQuiDoi.fromMap(json.decode(source) as Map<String, dynamic>);
}
