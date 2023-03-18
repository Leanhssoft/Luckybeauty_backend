import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class CreateRoleDto {
  String name;
  String displayName;
  String? description;
  String? normalizedName;
  List<dynamic>? grantedPermissions;
  CreateRoleDto({
    required this.name,
    required this.displayName,
    this.description,
    this.normalizedName,
    this.grantedPermissions,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'name': name,
      'displayName': displayName,
      'description': description,
      'normalizedName': normalizedName,
      'grantedPermissions': grantedPermissions,
    };
  }

  factory CreateRoleDto.fromMap(Map<String, dynamic> map) {
    return CreateRoleDto(
      name: map['name'],
      displayName: map['displayName'],
      description: map['description'],
      normalizedName: map['normalizedName'],
      grantedPermissions: map['grantedPermissions'],
    );
  }

  String toJson() => json.encode(toMap());

  factory CreateRoleDto.fromJson(String source) => CreateRoleDto.fromMap(json.decode(source) as Map<String, dynamic>);

  CreateRoleDto copyWith({
    String? name,
    String? displayName,
    String? description,
    String? normalizedName,
    List<String>? grantedPermissions,
  }) {
    return CreateRoleDto(
      name: name ?? this.name,
      displayName: displayName ?? this.displayName,
      description: description ?? this.description,
      normalizedName: normalizedName ?? this.normalizedName,
      grantedPermissions: grantedPermissions ?? this.grantedPermissions,
    );
  }

  @override
  String toString() {
    return 'CreateRoleDto(name: $name, displayName: $displayName, description: $description, normalizedName: $normalizedName, grantedPermissions: $grantedPermissions)';
  }
}
