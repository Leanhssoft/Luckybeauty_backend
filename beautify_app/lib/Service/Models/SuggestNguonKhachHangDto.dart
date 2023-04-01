import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class SuggestNguonKhachHang {
  String? id;
  String? tenNguonKhach;
  SuggestNguonKhachHang({
    this.id,
    this.tenNguonKhach,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenNguonKhach': tenNguonKhach,
    };
  }

  factory SuggestNguonKhachHang.fromMap(Map<String, dynamic> map) {
    return SuggestNguonKhachHang(
      id: map['id'] != null ? map['id'] as String : null,
      tenNguonKhach: map['tenNguonKhach'] != null ? map['tenNguonKhach'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory SuggestNguonKhachHang.fromJson(String source) => SuggestNguonKhachHang.fromMap(json.decode(source) as Map<String, dynamic>);
}
