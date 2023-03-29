import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class Tenant {
  String? tenancyName;
  String? name;
  bool? isActive;
  int? id;

  Tenant({
    this.tenancyName,
    this.name,
    this.isActive,
    this.id,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'tenancyName': tenancyName,
      'name': name,
      'isActive': isActive,
      'id': id,
    };
  }

  factory Tenant.fromMap(Map<String, dynamic> map) {
    return Tenant(
      tenancyName:
          map['tenancyName'] != null ? map['tenancyName'] as String : null,
      name: map['name'] != null ? map['name'] as String : null,
      isActive: map['isActive'] != null ? map['isActive'] as bool : null,
      id: map['id'] != null ? map['id'] as int : null,
    );
  }

  String toJson() => json.encode(toMap());

  Tenant.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    name = json['name'];
    tenancyName = json['tenancyName'];
    isActive = json['isActive'];
  }
}
