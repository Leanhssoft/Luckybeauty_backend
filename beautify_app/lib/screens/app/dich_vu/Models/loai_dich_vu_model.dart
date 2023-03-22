import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class LoaiDichVuDto {
  String? id;
  String? maLoai;
  String? tenLoai;
  List<String>? dichVus;

  LoaiDichVuDto({this.id, this.maLoai, this.tenLoai, this.dichVus});

  LoaiDichVuDto.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    maLoai = json['maLoai'];
    tenLoai = json['tenLoai'];
    dichVus = json['dichVus'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['id'] = id;
    data['maLoai'] = maLoai;
    data['tenLoai'] = tenLoai;
    data['dichVus'] = dichVus;
    return data;
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'maLoai': maLoai,
      'tenLoai': tenLoai,
      'dichVus': dichVus,
    };
  }
}
