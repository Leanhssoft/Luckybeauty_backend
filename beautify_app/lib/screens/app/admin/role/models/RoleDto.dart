import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class RoleDto {
  late String name;
  late String displayName;
  String? description;
  late int id;
  late String normalizedName;
  List<dynamic>? grantedPermissions;
  RoleDto({
    required this.name,
    required this.displayName,
    this.description,
    required this.id,
    required this.normalizedName,
    this.grantedPermissions,
  });
  RoleDto.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    name = json['name'];
    displayName = json['displayName'];
    description = json['description'];
    grantedPermissions = json['grantedPermissions'];
    normalizedName = json['normalizedName'];
    id = json['id'];
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'name': name,
      'displayName': displayName,
      'description': description,
      'grantedPermissions': grantedPermissions,
      'normalizedName': normalizedName,
      'id': id,
    };
  }

  factory RoleDto.fromMap(Map<String, dynamic> map) {
    return RoleDto(
      name: map['name'],
      displayName: map['displayName'],
      description: map['description'],
      normalizedName: map['normalizedName'],
      grantedPermissions: map['grantedPermissions'],
      id: map['id'],
    );
  }

  String toJson() => json.encode(toMap());
  
  //factory PermissionViewModel.fromJson(dynamic source) => PermissionViewModel.fromMap(json.decode(source) as Map<String, dynamic>);

  @override
  String toString() {
    return 'RoleDto(name: $name, displayName: $displayName, description: $description,normalizedName:$normalizedName, id: $id,grantedPermissions:$grantedPermissions)';
  }
  Map<String, dynamic> toJsonConvert() {
        return {
          'name': name,
          'displayName': displayName,
          'description': description,
          'id': id,
          'grantedPermissions': grantedPermissions,
        };
      }
    }
  List<Map<String, dynamic>> convertToMapList(List<RoleDto> data) {
  List<Map<String, dynamic>> mapList = [];
  data.forEach((element) {
    mapList.add(element.toJsonConvert());
  });
  return mapList;
}
